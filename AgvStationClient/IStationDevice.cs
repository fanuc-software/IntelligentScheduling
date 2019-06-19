using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgvStationClient
{
    public interface IStationDevice
    {

        /// <summary>
        /// 获得原料输入请求
        /// </summary>
        /// <param name="raw_in">false→true:请求生效</param>
        /// <returns>true：读取正常； false：读取异常</returns>
        bool GetRawInRequireState(ref bool raw_in);

        /// <summary>
        /// 获得空箱回库请求
        /// </summary>
        /// <param name="empty_out">false→true:请求生效</param>
        /// <returns>true：读取正常； false：读取异常</returns>
        bool GetEmptyOutState(ref bool empty_out);

        /// <summary>
        /// 获得成品空箱输入请求
        /// </summary>
        /// <param name="empty_in">false→true:请求生效</param>
        /// <returns>true：读取正常； false：读取异常</returns>
        bool GetEmptyInState(ref bool empty_in);

        /// <summary>
        /// 获得成品回库请求
        /// </summary>
        /// <param name="fin_out">false→true:请求生效</param>
        /// <returns>true：读取正常； false：读取异常</returns>
        bool GetFinOutState(ref bool fin_out);

        /// <summary>
        /// 设定原料输入请求完成
        /// </summary>
        /// <param name="raw_in_fin">true:请求完成</param>
        /// <returns>true：读取正常； false：读取异常</returns>
        bool SetRawInFin(bool raw_in_fin);

        /// <summary>
        /// 设定空箱回库请求完成
        /// </summary>
        /// <param name="empty_out_fin">true:请求完成</param>
        /// <returns>true：读取正常； false：读取异常</returns>
        bool SetEmptyOutFin(ref bool empty_out_fin);

        /// <summary>
        /// 设定成品空箱输入请求完成
        /// </summary>
        /// <param name="empty_in_fin">true:请求完成</param>
        /// <returns>true：读取正常； false：读取异常</returns>
        bool SetEmptyInFin(ref bool empty_in_fin);

        /// <summary>
        /// 设定成品回库请求完成
        /// </summary>
        /// <param name="fin_out_fin">true:请求完成</param>
        /// <returns>true：读取正常； false：读取异常</returns>
        bool SetFinOutFin(ref bool fin_out_fin);

        /// <summary>
        /// 设定报警
        /// </summary>
        /// <param name="alarm">true:报警中；false：未报警</param>
        /// <returns>true：读取正常； false：读取异常</returns>
        bool SetAlarm(bool alarm);

        /// <summary>
        /// 获得复位
        /// </summary>
        /// <param name="reset">true:复位中；false：非复位</param>
        /// <returns>true：读取正常； false：读取异常</returns>
        bool GetReset(ref bool reset);

        /// <summary>
        /// 设定复位
        /// </summary>
        /// <param name="reset">true:复位中；false：非复位</param>
        /// <returns>true：读取正常； false：读取异常</returns>
        bool SetReset(bool reset);

        /// <summary>
        /// 获得原料输入允许信号（硬件IO）
        /// </summary>
        /// <param name="reset">true:允许；false：不允许</param>
        /// <returns>true：读取正常； false：读取异常</returns>
        bool CheckRawInRequireAllow(ref bool raw_in);

        /// <summary>
        /// 获得成品空箱输入允许信号（硬件IO）
        /// </summary>
        /// <param name="reset">true:允许；false：不允许</param>
        /// <returns>true：读取正常； false：读取异常</returns>
        bool CheckEmptyInAllow(ref bool empty_in);
        
    }
}
