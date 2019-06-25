using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace OrderDistribution
{
    public enum OrderServiceEnum
    {
        OrderDistribution
    }

    public abstract class BaseOrderService
    {

        public  event Action<OrderItem, OrderServiceEnum, int> OrderStateChangeEvent;
        public  event Func<OrderServiceEnum, OrderItem> GetFirstOrderEvent;
        public  event Action<OrderServiceEnum, int> UpdateOrderActualQuantityEvent;
        public event Action<OrderServiceState> SendOrderServiceStateMessage;
        public OrderServiceEnum ServiceEnum;

        public abstract IOrderDevice Device { get; }

        CancellationTokenSource token = new CancellationTokenSource();
  
        public BaseOrderService(OrderServiceEnum orderServiceEnum)
        {
            ServiceEnum = orderServiceEnum;
        }


        public void Start()
        {

            Task.Factory.StartNew(() =>
            {

                SendOrderServiceStateMessage?.Invoke(
                    new OrderServiceState { State = OrderServiceStateEnum.WARN, Message = "ORDER SERVICE START!", ErrorCode = OrderServiceErrorCodeEnum.NORMAL });

                Device.Temp_S_Order_AllowMES_Last = false;

                bool ret = false;

                #region 初始化
                bool temp_S_Order_AllowMES_Last = false;
                ret = Device.GetOrderAllow(ref temp_S_Order_AllowMES_Last);
                if (ret == false)
                {
                    while (ret == false)
                    {
                        ret = Device.SetOrderAlarm(true);
                        SendOrderServiceStateMessage?.Invoke(
                            new OrderServiceState { State = OrderServiceStateEnum.ERROR, Message = "初始化失败,发送错误信息至设备!" });
                        Thread.Sleep(1000);
                    }


                    bool dev_reset = false;
                    while (dev_reset == false)
                    {
                        Device.GetOrderReset(ref dev_reset);
                        SendOrderServiceStateMessage?.Invoke(
                            new OrderServiceState { State = OrderServiceStateEnum.INFO, Message = "初始化失败,等待设备的复位信号" });
                        Thread.Sleep(1000);
                    }
                }
                Device.Temp_S_Order_AllowMES_Last = temp_S_Order_AllowMES_Last;

                #endregion

                while (!token.IsCancellationRequested)
                {

                    //订单下发服务
                    var srv_ret = OrderService();
                    if (srv_ret.Item1 == false)
                    {
                        ret = false;
                        while (ret == false)
                        {
                            ret = Device.SetOrderAlarm(true);
                            SendOrderServiceStateMessage?.Invoke(
                                new OrderServiceState { State = OrderServiceStateEnum.ERROR, Message = "订单下发失败,发送错误信息至设备!" , ErrorCode = srv_ret.Item2});
                            Thread.Sleep(1000);
                        }


                        bool dev_reset = false;
                        while (dev_reset == false)
                        {
                            Device.GetOrderReset(ref dev_reset);
                            SendOrderServiceStateMessage?.Invoke(
                                new OrderServiceState { State = OrderServiceStateEnum.INFO, Message = "订单下发失败,等待设备的复位信号" ,ErrorCode=OrderServiceErrorCodeEnum.NORMAL});
                            Thread.Sleep(1000);
                        }

                        //复位所有信号
                        Device.OrderDeviceReset();
                    }
                }
            }, token.Token);
        }

        //TODO
        public void Stop()
        {

        }

        //TODO
        public void Suspend()
        {

        }

       
        private Tuple<bool, OrderServiceErrorCodeEnum> OrderService()
        {
            bool ret = false;

            bool mode = false;
            ret = Device.GetOrderMode(ref mode);
            if (ret != true)
            {
                return new Tuple<bool, OrderServiceErrorCodeEnum>(false, OrderServiceErrorCodeEnum.GETMODE);
            }
         
            if (mode == true)
            {
                bool S_Order_AllowMES = false;
                ret = Device.GetOrderAllow(ref S_Order_AllowMES);
                if (ret != true)
                {
                    return new Tuple<bool, OrderServiceErrorCodeEnum>(false, OrderServiceErrorCodeEnum.GETALLOW);
                }

                //if (S_Order_AllowMES == true && Device.Temp_S_Order_AllowMES_Last == false)
                if(S_Order_AllowMES == true)
                {
                   
                    //如果DOWORK订单为2个，将第一个的状态置为DONE
                    OrderStateChangeEvent?.Invoke(null, ServiceEnum,-1);//test

                    //防错处理
                    ret = CheckOnBegin();
                    if (ret != true)
                    {
                        Device.Temp_S_Order_AllowMES_Last = S_Order_AllowMES;
                        return new Tuple<bool, OrderServiceErrorCodeEnum>(false, OrderServiceErrorCodeEnum.CHECKONBEGIN);
                    }

                    //下单
                    var first_order = GetFirstOrderEvent?.Invoke(ServiceEnum);//test
            
                    if (first_order == null)
                    {
                        return new Tuple<bool, OrderServiceErrorCodeEnum>(true, OrderServiceErrorCodeEnum.NORMAL); ;
                    }
                    else
                    {
                        Device.Temp_S_Order_AllowMES_Last = S_Order_AllowMES;

                        //向PLC写入订单信息
                        ret = Device.SetProductType(first_order.Type);
                        if (ret != true)
                        {
                            return new Tuple<bool, OrderServiceErrorCodeEnum>(false, OrderServiceErrorCodeEnum.SETPRODUCTTYPE);
                        }
                        ret = Device.SetQuantity(first_order.Quantity);
                        if (ret != true)
                        {
                            return new Tuple<bool, OrderServiceErrorCodeEnum>(false, OrderServiceErrorCodeEnum.SETQUANTITY);
                        }

                        //检查下单是否成功
                        DateTime temp_time = DateTime.Now;
                        bool check_state = false;
                        while (check_state == false && (DateTime.Now - temp_time).TotalSeconds < 20)
                        {
                            int check_type = 0;
                            int check_quantity = 0;
                            Device.GetCheckProductType(ref check_type);
                            Device.GetCheckQuantity(ref check_quantity);

                            if (check_type == first_order.Type && check_quantity == first_order.Quantity)
                            {
                                check_state = true;
                            }
                        }
                        if (check_state == false)
                        {

                            return new Tuple<bool, OrderServiceErrorCodeEnum>(false, OrderServiceErrorCodeEnum.CHECKCONFIRM); 
                        }

                        //发出二次确认信号
                        ret = Device.SetOrderConfirm(true);
                        if (ret != true)
                        {
                            return new Tuple<bool, OrderServiceErrorCodeEnum>(false, OrderServiceErrorCodeEnum.SETCONFIRRM); 
                        }

                        //变更软件订单状态
                        first_order.State = OrderItemStateEnum.DOWORK;//test
                        OrderStateChangeEvent?.Invoke(first_order, ServiceEnum, 0);
                    }
                }
                else
                {
                    Device.Temp_S_Order_AllowMES_Last = S_Order_AllowMES;

                    int process = 0;
                    ret = Device.GetOrderProcess(ref process);
                    if (ret == true)
                    {
                        //更新订单完成数量
                        UpdateOrderActualQuantityEvent?.Invoke(ServiceEnum, process);//test
                    }
                }

            }

            return new Tuple<bool, OrderServiceErrorCodeEnum>(true, OrderServiceErrorCodeEnum.NORMAL); ;

        }

        private bool CheckOnBegin()
        {
            bool ret = false;
            
            //ret = Device.SetOrderConfirm(false);
            //if (ret != true) return ret;
            
            ret = Device.SetProductType(0);
            if (ret != true) return ret;
            
            ret = Device.SetQuantity(0);
            if (ret != true) return ret;
            
            ret = Device.SetCheckProductType(0);
            if (ret != true) return ret;
            
            ret = Device.SetCheckQuantity(0);
            if (ret != true) return ret;
            
            ret = Device.SetOrderReset(false);
            if (ret != true) return ret;
            
            ret = Device.SetOrderAlarm(false);
            if (ret != true) return ret;

            return true;
        }
    }

}