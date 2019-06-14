using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderDistribution
{
    public interface IOrderDevice
    {
        bool Temp_S_Order_AllowMES_Last { get; set; }

        /// <summary>
        /// 获得订单下方模式
        /// </summary>
        /// <param name="mode">true：采用MES下发订单； false： 不采用MES下发订单</param>
        /// <returns>true：读取正常； false：读取异常</returns>
        bool GetOrderMode(ref bool mode);

        /// <summary>
        /// 获得订单允许下发信号状态
        /// </summary>
        /// <param name="allow">false→true 上升沿时下发订单； 其他：等待</param>
        /// <returns>true：读取正常； false：读取异常</returns>
        bool GetOrderAllow(ref bool allow);

        /// <summary>
        /// 设定订单产品种类
        /// </summary>
        /// <param name="ptype">产品种类数据</param>
        /// <returns>true：设定正常； false：设定异常</returns>
        bool SetProductType(int ptype);

        /// <summary>
        /// 设定订单数量
        /// </summary>
        /// <param name="quantity">订单数量</param>
        /// <returns>true：设定正常； false：设定异常</returns>
        bool SetQuantity(int quantity);

        /// <summary>
        /// 获得订单产品种类
        /// </summary>
        /// <param name="ptype">产品种类数据</param>
        /// <returns>true：设定正常； false：设定异常</returns>
        bool GetProductType(ref int ptype);

        /// <summary>
        /// 获得订单数量
        /// </summary>
        /// <param name="quantity">订单数量</param>
        /// <returns>true：设定正常； false：设定异常</returns>
        bool GetQuantity(ref int quantity);

        /// <summary>
        /// 获得校验订单产品种类
        /// </summary>
        /// <param name="ptype">校验产品种类数据</param>
        /// <returns>true：获得正常； false：获得异常</returns>
        bool GetCheckProductType(ref int ptype);

        /// <summary>
        /// 获得校验订单数量
        /// </summary>
        /// <param name="quantity">校验订单数量</param>
        /// <returns>true：获得正常； false：获得异常</returns>
        bool GetCheckQuantity(ref int quantity);

        /// <summary>
        /// 设定订单下发错误信息（软件）
        /// </summary>
        /// <param name="alarm">订单下发错误 true：错误； false：正常</param>
        /// <returns>true：设定正常； false：设定异常</returns>
        bool SetOrderAlarm(bool alarm);

        /// <summary>
        /// 获得订单下发错误信息状态（软件）
        /// </summary>
        /// <param name="alarm">订单下发错误 true：错误； false：正常</param>
        /// <returns>true：获得正常； false：获得异常</returns>
        bool GetOrderAlarm(ref bool alarm);

        /// <summary>
        /// 获得订单下发复位状态
        /// </summary>
        /// <param name="reset">订单下发复位 true：复位状态； false：非复位状态</param>
        /// <returns>true：获得正常； false：获得异常</returns>
        bool GetOrderReset(ref bool reset);

        /// <summary>
        /// 设定订单二次确认信号
        /// </summary>
        /// <param name="alarm">订单二次确认信号 true：确认； false：非确认</param>
        /// <returns>true：设定正常； false：设定异常</returns>
        bool SetOrderConfirm(bool confirm);

        /// <summary>
        /// 获得订单二次确认信号
        /// </summary>
        /// <param name="alarm">订单二次确认信号 true：确认； false：非确认</param>
        /// <returns>true：获得正常； false：获得异常</returns>
        bool GetOrderConfirm(ref bool confirm);

        /// <summary>
        /// 获得当前订单的加工数量
        /// </summary>
        /// <param name="quantity">订单二次确认信号 true：确认； false：非确认</param>
        /// <returns>true：获得正常； false：获得异常</returns>
        bool GetOrderProcess(ref int quantity);

        /// <summary>
        /// 复位设备
        /// </summary>
        /// <param name="reset">订单下发复位 true：复位状态； false：非复位状态</param>
        /// <returns>true：获得正常； false：获得异常</returns>
        bool OrderDeviceReset();
    }
}
