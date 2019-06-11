using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OrderDistribution
{
    public class OrderDistributionService
    {
        public IOrderDevice device;
        CancellationTokenSource token = new CancellationTokenSource();

        public event Action<OrderItem, int> OrderStateChangeEvent;
        public event Func<OrderItem> GetFirstOrderEvent;
        public event Action<int> UpdateOrderActualQuantityEvent;

        public OrderDistributionService()
        {
            device = new AllenBradleyDevice();
        }

        public void Start()
        {

            Task.Factory.StartNew(() =>
            {

                device.Temp_S_Order_AllowMES_Last = false;

                bool ret = false;

                #region 初始化
                bool temp_S_Order_AllowMES_Last = false;
                ret = device.GetOrderAllow(ref temp_S_Order_AllowMES_Last);
                if (ret == false)
                {
                    while (ret == false)
                    {
                        ret = device.SetOrderAlarm(true);
                        SendOrderServiceStateMessage(
                            new OrderServiceState { State = OrderServiceStateEnum.ERROR, Message = "初始化失败,发送错误信息至设备!" });
                        Thread.Sleep(1000);
                    }


                    bool dev_reset = false;
                    while (dev_reset == false)
                    {
                        device.GetOrderReset(ref dev_reset);
                        SendOrderServiceStateMessage(
                            new OrderServiceState { State = OrderServiceStateEnum.INFO, Message = "初始化失败,等待设备的复位信号" });
                        Thread.Sleep(1000);
                    }
                }
                device.Temp_S_Order_AllowMES_Last = temp_S_Order_AllowMES_Last;

                #endregion

                while (!token.IsCancellationRequested)
                {

                    //订单下发服务
                    ret = OrderService();
                    if (ret == false)
                    {
                        while (ret == false)
                        {
                            ret = device.SetOrderAlarm(true);
                            SendOrderServiceStateMessage(
                                new OrderServiceState { State = OrderServiceStateEnum.ERROR, Message = "订单下发失败,发送错误信息至设备!" });
                            Thread.Sleep(1000);
                        }


                        bool dev_reset = false;
                        while (dev_reset == false)
                        {
                            device.GetOrderReset(ref dev_reset);
                            SendOrderServiceStateMessage(
                                new OrderServiceState { State = OrderServiceStateEnum.INFO, Message = "订单下发失败,等待设备的复位信号" });
                            Thread.Sleep(1000);
                        }
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
        
        //TODO
        private void SendOrderServiceStateMessage(OrderServiceState state)
        {

        }

        private bool OrderService()
        {
            bool ret = false;

            bool mode = false;
            ret = device.GetOrderMode(ref mode);
            if (ret != true) return ret;

            if (mode == true)
            {
                bool S_Order_AllowMES = false;
                ret = device.GetOrderAllow(ref S_Order_AllowMES);
                if (ret != true) return ret;

                if (S_Order_AllowMES == true && device.Temp_S_Order_AllowMES_Last == false)
                {
                    //如果DOWORK订单为2个，将第一个的状态置为DONE
                    OrderStateChangeEvent?.Invoke(null, -1);

                    //防错处理
                    ret = CheckOnBegin();
                    if (ret != true)
                    {
                        device.Temp_S_Order_AllowMES_Last = S_Order_AllowMES;
                        return ret;
                    }

                    //下单
                    var first_order = GetFirstOrderEvent?.Invoke();
                    if (first_order == null)
                    {
                        return true;
                    }
                    else
                    {
                        device.Temp_S_Order_AllowMES_Last = S_Order_AllowMES;

                        //向PLC写入订单信息
                        ret = device.SetProductType(first_order.Type);
                        if (ret != true) return ret;
                        ret = device.SetQuantity(first_order.Quantity);
                        if (ret != true) return ret;

                        //检查下单是否成功
                        DateTime temp_time = DateTime.Now;
                        bool check_state = false;
                        while (check_state == false && (DateTime.Now - temp_time).TotalSeconds < 20)
                        {
                            int check_type = 0;
                            int check_quantity = 0;
                            device.GetCheckProductType(ref check_type);
                            device.GetCheckQuantity(ref check_quantity);

                            if (check_type == first_order.Type && check_quantity == first_order.Quantity)
                            {
                                check_state = true;
                            }
                        }
                        if (check_state == false) return false;

                        //发出二次确认信号
                        ret = device.SetOrderConfirm(true);
                        if (ret != true) return ret;

                        //变更软件订单状态
                        first_order.State = OrderItemStateEnum.DOWORK;
                        OrderStateChangeEvent?.Invoke(first_order, 0);


                    }
                }
                else
                {
                    device.Temp_S_Order_AllowMES_Last = S_Order_AllowMES;

                    int process = 0;
                    ret = device.GetOrderProcess(ref process);
                    if (ret == true)
                    {
                        //更新订单完成数量

                        UpdateOrderActualQuantityEvent?.Invoke(process);
                    }
                }

            }

            return true;

        }

        private bool CheckOnBegin()
        {
            bool ret = false;

            bool confirm = false;
            ret = device.GetOrderConfirm(ref confirm);
            if (ret != true) return ret;

            int type = 0;
            ret = device.GetProductType(ref type);
            if (ret != true) return ret;

            int quantity = 0;
            ret = device.GetQuantity(ref quantity);
            if (ret != true) return ret;

            int check_type = 0;
            ret = device.GetCheckProductType(ref check_type);
            if (ret != true) return ret;

            int check_quantity = 0;
            ret = device.GetCheckQuantity(ref check_quantity);
            if (ret != true) return ret;

            bool reset = false;
            ret = device.GetOrderReset(ref reset);
            if (ret != true) return ret;

            bool alarm = false;
            ret = device.GetOrderAlarm(ref alarm);
            if (ret != true) return ret;

            if (confirm != false || type != 0 || quantity != 0 || check_type != 0 || check_quantity != 0 || reset != false || alarm != false)
            {
                return false;
            }

            return true;
        }

    }
}
