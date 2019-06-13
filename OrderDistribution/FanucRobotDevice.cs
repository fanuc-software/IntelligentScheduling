using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DeviceAsset;

namespace OrderDistribution
{
    public class FanucRobotDevice : IOrderDevice
    {
        private FanucRobotDataConfig m_OrderModeConfig;
        private FanucRobotDataConfig m_OrderAllowConfig;
        private FanucRobotDataConfig m_ProductType;
        private FanucRobotDataConfig m_Quantity;
        private FanucRobotDataConfig m_CheckProductType;
        private FanucRobotDataConfig m_CheckQuantity;
        private FanucRobotDataConfig m_OrderAlarm;
        private FanucRobotDataConfig m_OrderReset;
        private FanucRobotDataConfig m_OrderConfirm;
        private FanucRobotDataConfig m_OrderConfirmReset;
        private FanucRobotDataConfig m_OrderProcess;

        private FanucRobotModbus m_FanucRobotDevice;

        public FanucRobotDevice()
        {
            m_FanucRobotDevice = new FanucRobotModbus("192.168.1.1");

            m_OrderModeConfig = new FanucRobotDataConfig { DataType = FanucRobotDataTypeEnum.DO, DataAdr = "1" };
            m_OrderAllowConfig = new FanucRobotDataConfig { DataType = FanucRobotDataTypeEnum.DO, DataAdr = "1" };
            m_ProductType = new FanucRobotDataConfig { DataType = FanucRobotDataTypeEnum.GI, DataAdr = "1" };
            m_Quantity = new FanucRobotDataConfig { DataType = FanucRobotDataTypeEnum.GI, DataAdr = "1" };
            m_CheckProductType = new FanucRobotDataConfig { DataType = FanucRobotDataTypeEnum.GO, DataAdr = "1" };
            m_CheckQuantity = new FanucRobotDataConfig { DataType = FanucRobotDataTypeEnum.GO, DataAdr = "1" };
            m_OrderAlarm = new FanucRobotDataConfig { DataType = FanucRobotDataTypeEnum.DI, DataAdr = "1" };
            m_OrderReset = new FanucRobotDataConfig { DataType = FanucRobotDataTypeEnum.DO, DataAdr = "1" };
            m_OrderConfirm = new FanucRobotDataConfig { DataType = FanucRobotDataTypeEnum.DI, DataAdr = "1" };
            m_OrderConfirmReset = new FanucRobotDataConfig { DataType = FanucRobotDataTypeEnum.DO, DataAdr = "1" };
            m_OrderProcess = new FanucRobotDataConfig { DataType = FanucRobotDataTypeEnum.GO, DataAdr = "1" };

        }

        public bool Temp_S_Order_AllowMES_Last { get; set; }

        /// <summary>
        /// 获得订单下方模式
        /// </summary>
        /// <param name="mode">true：采用MES下发订单； false： 不采用MES下发订单</param>
        /// <returns>true：读取正常； false：读取异常</returns>
        public bool GetOrderMode(ref bool mode)
        {
            var ret = m_FanucRobotDevice.Read(m_OrderModeConfig);
            if (ret.IsSuccess == false) return false;

            bool temp = false;
            var pret = bool.TryParse(ret.Content, out temp);
            if (pret == false) return false;

            mode = temp;
            return true;
        }

        /// <summary>
        /// 获得订单允许下发信号状态
        /// </summary>
        /// <param name="allow">false→true 上升沿时下发订单； 其他：等待</param>
        /// <returns>true：读取正常； false：读取异常</returns>
        public bool GetOrderAllow(ref bool allow)
        {
            var ret = m_FanucRobotDevice.Read(m_OrderAllowConfig);
            if (ret.IsSuccess == false) return false;

            bool temp = false;
            var pret = bool.TryParse(ret.Content, out temp);
            if (pret == false) return false;

            allow = temp;
            return true;
        }

        /// <summary>
        /// 设定订单产品种类
        /// </summary>
        /// <param name="ptype">产品种类数据</param>
        /// <returns>true：设定正常； false：设定异常</returns>
        public bool SetProductType(int ptype)
        {
            var ret = m_FanucRobotDevice.Write(m_ProductType, ptype.ToString());
            if (ret.IsSuccess == false) return false;

            return true;
        }

        /// <summary>
        /// 设定订单数量
        /// </summary>
        /// <param name="quantity">订单数量</param>
        /// <returns>true：设定正常； false：设定异常</returns>
        public bool SetQuantity(int quantity)
        {
            var ret = m_FanucRobotDevice.Write(m_Quantity, quantity.ToString());
            if (ret.IsSuccess == false) return false;

            return true;
        }

        /// <summary>
        /// 获得订单产品种类
        /// </summary>
        /// <param name="ptype">产品种类数据</param>
        /// <returns>true：设定正常； false：设定异常</returns>
        public bool GetProductType(ref int ptype)
        {
            var ret = m_FanucRobotDevice.Read(m_ProductType);
            if (ret.IsSuccess == false) return false;

            int temp = 0;
            var pret = int.TryParse(ret.Content, out temp);
            if (pret == false) return false;

            ptype = temp;
            return true;
        }

        /// <summary>
        /// 获得订单数量
        /// </summary>
        /// <param name="quantity">订单数量</param>
        /// <returns>true：设定正常； false：设定异常</returns>
        public bool GetQuantity(ref int quantity)
        {
            var ret = m_FanucRobotDevice.Read(m_Quantity);
            if (ret.IsSuccess == false) return false;

            int temp = 0;
            var pret = int.TryParse(ret.Content, out temp);
            if (pret == false) return false;

            quantity = temp;
            return true;
        }

        /// <summary>
        /// 获得校验订单产品种类
        /// </summary>
        /// <param name="ptype">校验产品种类数据</param>
        /// <returns>true：获得正常； false：获得异常</returns>
        public bool GetCheckProductType(ref int ptype)
        {
            var ret = m_FanucRobotDevice.Read(m_CheckProductType);
            if (ret.IsSuccess == false) return false;

            int temp = 0;
            var pret = int.TryParse(ret.Content, out temp);
            if (pret == false) return false;

            ptype = temp;
            return true;
        }

        /// <summary>
        /// 获得校验订单数量
        /// </summary>
        /// <param name="quantity">校验订单数量</param>
        /// <returns>true：获得正常； false：获得异常</returns>
        public bool GetCheckQuantity(ref int quantity)
        {
            var ret = m_FanucRobotDevice.Read(m_CheckQuantity);
            if (ret.IsSuccess == false) return false;

            int temp = 0;
            var pret = int.TryParse(ret.Content, out temp);
            if (pret == false) return false;

            quantity = temp;
            return true;
        }

        /// <summary>
        /// 设定订单下发错误信息（软件）
        /// </summary>
        /// <param name="alarm">订单下发错误 true：错误； false：正常</param>
        /// <returns>true：设定正常； false：设定异常</returns>
        public bool SetOrderAlarm(bool alarm)
        {
            var ret = m_FanucRobotDevice.Write(m_OrderAlarm, alarm.ToString());
            if (ret.IsSuccess == false) return false;

            return true;
        }

        /// <summary>
        /// 获得订单下发错误信息状态（软件）
        /// </summary>
        /// <param name="alarm">订单下发错误 true：错误； false：正常</param>
        /// <returns>true：获得正常； false：获得异常</returns>
        public bool GetOrderAlarm(ref bool alarm)
        {
            var ret = m_FanucRobotDevice.Read(m_OrderAlarm);
            if (ret.IsSuccess == false) return false;

            bool temp = false;
            var pret = bool.TryParse(ret.Content, out temp);
            if (pret == false) return false;

            alarm = temp;
            return true;
        }

        /// <summary>
        /// 获得订单下发复位状态
        /// </summary>
        /// <param name="reset">订单下发复位 true：复位状态； false：非复位状态</param>
        /// <returns>true：获得正常； false：获得异常</returns>
        public bool GetOrderReset(ref bool reset)
        {
            var ret = m_FanucRobotDevice.Read(m_OrderReset);
            if (ret.IsSuccess == false) return false;

            bool temp = false;
            var pret = bool.TryParse(ret.Content, out temp);
            if (pret == false) return false;

            reset = temp;
            return true;
        }

        /// <summary>
        /// 设定订单二次确认信号
        /// </summary>
        /// <param name="alarm">订单二次确认信号 true：确认； false：非确认</param>
        /// <returns>true：设定正常； false：设定异常</returns>
        public bool SetOrderConfirm(bool confirm)
        {


            var ret = m_FanucRobotDevice.Write(m_OrderConfirm, confirm.ToString());
            if (ret.IsSuccess == false) return false;

            var start_time = DateTime.Now;
            bool reset_confirm = false;
            while(reset_confirm==false && (DateTime.Now-start_time).TotalSeconds<20)
            {
                var read_ret =  m_FanucRobotDevice.Read(m_OrderConfirmReset);
                if(ret.IsSuccess == true)
                {
                    bool temp = false;
                    var pret = bool.TryParse(read_ret.Content, out temp);
                    if (pret == true) reset_confirm = temp;
                }
            }

            if(reset_confirm == false)
            {
                return false;
            }

            ret = m_FanucRobotDevice.Write(m_ProductType, "0");
            if (ret.IsSuccess == false) return false;

            ret = m_FanucRobotDevice.Write(m_Quantity, "0");
            if (ret.IsSuccess == false) return false;

            ret = m_FanucRobotDevice.Write(m_OrderConfirm, "false");
            if (ret.IsSuccess == false) return false;
            return true;
        }

        /// <summary>
        /// 获得订单二次确认信号
        /// </summary>
        /// <param name="alarm">订单二次确认信号 true：确认； false：非确认</param>
        /// <returns>true：获得正常； false：获得异常</returns>
        public bool GetOrderConfirm(ref bool confirm)
        {
            var ret = m_FanucRobotDevice.Read(m_OrderConfirm);
            if (ret.IsSuccess == false) return false;

            bool temp = false;
            var pret = bool.TryParse(ret.Content, out temp);
            if (pret == false) return false;

            confirm = temp;
            return true;
        }

        /// <summary>
        /// 获得当前订单的加工数量
        /// </summary>
        /// <param name="quantity">订单二次确认信号 true：确认； false：非确认</param>
        /// <returns>true：获得正常； false：获得异常</returns>
        public bool GetOrderProcess(ref int quantity)
        {
            var ret = m_FanucRobotDevice.Read(m_OrderProcess);
            if (ret.IsSuccess == false) return false;

            int temp = 0;
            var pret = int.TryParse(ret.Content, out temp);
            if (pret == false) return false;

            quantity = temp;
            return true;
        }

        public bool OrderDeviceReset()
        {
            var ret = m_FanucRobotDevice.Write(m_ProductType, "0");
            if (ret.IsSuccess == false) return false;

            ret = m_FanucRobotDevice.Write(m_Quantity, "0");
            if (ret.IsSuccess == false) return false;
            
            ret = m_FanucRobotDevice.Write(m_OrderConfirm, "false");
            if (ret.IsSuccess == false) return false;

            ret = m_FanucRobotDevice.Write(m_OrderAlarm, "false");
            if (ret.IsSuccess == false) return false;

            return true;
        }
    }
}
