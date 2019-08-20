using AgvStationClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AGV.Web.Service.Service
{
    public class TestStationDevice : IStationDevice
    {
        public string RawIn_Prod { get; set; }
        public string RawIn_Mate { get; set; }
        /// <summary>
        /// 毛坯出库→单元
        /// </summary>
        public bool RawIn_Req { get; set; }
        public bool RawIn_Fin { get; set; }
        public string EmptyOut_Prod { get; set; }
        public string EmptyOut_Mate { get; set; }
        /// <summary>
        /// 空箱回库→料库
        /// </summary>
        public bool EmptyOut_Req { get; set; }
        public bool EmptyOut_Fin { get; set; }
        public string EmptyIn_Prod { get; set; }
        public string EmptyIn_Mate { get; set; }
        /// <summary>
        /// 空箱出库→单元
        /// </summary>
        public bool EmptyIn_Req { get; set; }
        public bool EmptyIn_Fin { get; set; }
        public string FinOut_Prod { get; set; }
        public string FinOut_Mate { get; set; }
        /// <summary>
        /// 成品回库→料库
        /// </summary>
        public bool FinOut_Req { get; set; }
        public bool FinOut_Fin { get; set; }
        public bool Alarm { get; set; }
        public bool Reset { get; set; }
        public bool InFeedingSignal { get; set; }
        public bool OutFeedingSignal { get; set; }

        public TestStationDevice()
        {

        }
        /// <summary>
        /// 获得原料输入请求
        /// </summary>
        /// <param name="raw_in">false→true:请求生效</param>
        /// <returns>true：读取正常； false：读取异常</returns>
        public bool GetRawInRequireState(ref bool raw_in)
        {
            raw_in = RawIn_Req;
            return true;
        }

        /// <summary>
        /// 获得空箱回库请求
        /// </summary>
        /// <param name="empty_out">false→true:请求生效</param>
        /// <returns>true：读取正常； false：读取异常</returns>
        public bool GetEmptyOutState(ref bool empty_out)
        {

            empty_out = EmptyOut_Req;
            return true;
        }

        /// <summary>
        /// 获得成品空箱输入请求
        /// </summary>
        /// <param name="empty_in">false→true:请求生效</param>
        /// <returns>true：读取正常； false：读取异常</returns>
        public bool GetEmptyInState(ref bool empty_in)
        {

            empty_in = EmptyIn_Req;
            return true;
        }

        /// <summary>
        /// 获得成品回库请求
        /// </summary>
        /// <param name="fin_out">false→true:请求生效</param>
        /// <returns>true：读取正常； false：读取异常</returns>
        public bool GetFinOutState(ref bool fin_out)
        {

            fin_out = FinOut_Req;
            return true;
        }

        /// <summary>
        /// 设定原料输入请求完成
        /// </summary>
        /// <param name="raw_in_fin">true:请求完成</param>
        /// <returns>true：读取正常； false：读取异常</returns>
        public bool SetRawInFin(bool raw_in_fin)
        {
            RawIn_Fin = raw_in_fin;

            return true;
        }

        /// <summary>
        /// 设定空箱回库请求完成
        /// </summary>
        /// <param name="empty_out_fin">true:请求完成</param>
        /// <returns>true：读取正常； false：读取异常</returns>
        public bool SetEmptyOutFin(bool empty_out_fin)
        {
            EmptyOut_Fin = empty_out_fin;

            return true;
        }

        /// <summary>
        /// 设定成品空箱输入请求完成
        /// </summary>
        /// <param name="empty_in_fin">true:请求完成</param>
        /// <returns>true：读取正常； false：读取异常</returns>
        public bool SetEmptyInFin(bool empty_in_fin)
        {
            EmptyIn_Fin = empty_in_fin;

            return true;
        }

        /// <summary>
        /// 设定成品回库请求完成
        /// </summary>
        /// <param name="fin_out_fin">true:请求完成</param>
        /// <returns>true：读取正常； false：读取异常</returns>
        public bool SetFinOutFin(bool fin_out_fin)
        {
            FinOut_Fin = fin_out_fin;

            return true;
        }

        /// <summary>
        /// 设定报警
        /// </summary>
        /// <param name="alarm">true:报警中；false：未报警</param>
        /// <returns>true：读取正常； false：读取异常</returns>
        public bool SetAlarm(bool alarm)
        {
            Alarm = alarm;

            return true;
        }

        /// <summary>
        /// 获得复位
        /// </summary>
        /// <param name="reset">true:复位中；false：非复位</param>
        /// <returns>true：读取正常； false：读取异常</returns>
        public bool GetReset(ref bool reset)
        {
            reset = Reset;
            return true;
        }

        /// <summary>
        /// 设定复位
        /// </summary>
        /// <param name="reset">true:复位中；false：非复位</param>
        /// <returns>true：读取正常； false：读取异常</returns>
        public bool SetReset(bool reset)
        {
            Reset = reset;

            return true;
        }

        /// <summary>
        /// 获得原料输入允许信号（硬件IO）
        /// </summary>
        /// <param name="reset">true:允许；false：不允许</param>
        /// <returns>true：读取正常； false：读取异常</returns>
        public bool GetRawInFeedingSignal(ref bool raw_in)
        {
            raw_in = InFeedingSignal;
            return true;
        }

        /// <summary>
        /// 获得成品空箱输入允许信号（硬件IO）
        /// </summary>
        /// <param name="reset">true:允许；false：不允许</param>
        /// <returns>true：读取正常； false：读取异常</returns>
        public bool GetEmptyInFeedingSignal(ref bool empty_in)
        {
            empty_in = OutFeedingSignal;
            return true;
        }

        /// <summary>
        /// 获得毛坯种类
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns>true：读取正常； false：读取异常</returns>
        public bool GetRawInProductType(ref string type)
        {
            type = RawIn_Prod;
            return true;
        }

        /// <summary>
        /// 获得毛坯物料类型
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns>true：读取正常； false：读取异常</returns>
        public bool GetRawInMaterialType(ref string type)
        {
            type = RawIn_Mate;
            return true;
        }

        /// <summary>
        /// 获得成品空箱种类
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns>true：读取正常； false：读取异常</returns>
        public bool GetEmptyInProductType(ref string type)
        {
            type = EmptyIn_Prod;
            return true;
        }

        /// <summary>
        /// 获得成品空箱物料类型
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns>true：读取正常； false：读取异常</returns>
        public bool GetEmptyInMaterialType(ref string type)
        {
            type = EmptyIn_Mate;
            return true;
        }

        /// <summary>
        /// 获得毛坯空箱回库种类
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns>true：读取正常； false：读取异常</returns>
        public bool GetEmptyOutProductType(ref string type)
        {
            type = EmptyOut_Prod;
            return true;
        }

        /// <summary>
        /// 获得毛坯空箱回库物料类型
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns>true：读取正常； false：读取异常</returns>
        public bool GetEmptyOutMaterialType(ref string type)
        {
            type = EmptyOut_Mate;
            return true;
        }

        /// <summary>
        /// 获得成品回库种类
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns>true：读取正常； false：读取异常</returns>
        public bool GetFinOutProductType(ref string type)
        {
            type = FinOut_Prod;
            return true;
        }

        /// <summary>
        /// 获得成品回库物料类型
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns>true：读取正常； false：读取异常</returns>
        public bool GetFinOutMaterialType(ref string type)
        {
            type = FinOut_Mate;
            return true;
        }

    }
}