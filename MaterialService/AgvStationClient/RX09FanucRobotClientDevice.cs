using DeviceAsset;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgvStationClient
{
    public class RX09FanucRobotClientDevice : IStationDevice
    {
        //IP地址 192.168.1.121

        //DO 181--上料位呼叫毛坯
        //DO 182--上料位请求拿走毛坯空箱
        //D0 183--下料请求成品回库
        //D0 184--下料位成品空箱请求

        //DI 181--上料位呼叫毛坯完成
        //DI 182--上料位请求拿走毛坯空箱完成
        //DI 183--下料请求成品回库完成
        //DI 184--下料位成品空箱请求完成

        //DI 101--下料位检测
        //DI 102--上料位检测

        private FanucRobotDataConfig m_RawInRequireStateConfig;
        private FanucRobotDataConfig m_EmptyOutStateConfig;
        private FanucRobotDataConfig m_EmptyInStateConfig;
        private FanucRobotDataConfig m_FinOutStateConfig;
        private FanucRobotDataConfig m_RawInFinConfig;
        private FanucRobotDataConfig m_EmptyOutFinConfig;
        private FanucRobotDataConfig m_EmptyInFinConfig;
        private FanucRobotDataConfig m_FinOutFinConfig;
        private FanucRobotDataConfig m_AlarmConfig;
        private FanucRobotDataConfig m_ResetConfig;
        private FanucRobotDataConfig m_RawInRequireAllowConfig;
        private FanucRobotDataConfig m_EmptyInAllowConfig;
        private FanucRobotDataConfig m_RawInProductTypeConfig;
        private FanucRobotDataConfig m_RawInMaterialTypeConfig;
        private FanucRobotDataConfig m_EmptyInProductTypeConfig;
        private FanucRobotDataConfig m_EmptyInMaterialTypeConfig;
        private FanucRobotDataConfig m_EmptyOutProductTypeConfig;
        private FanucRobotDataConfig m_EmptyOutMaterialTypeConfig;
        private FanucRobotDataConfig m_FinOutProductTypeConfig;
        private FanucRobotDataConfig m_FinOutMaterialTypeConfig;

        private FanucRobotModbus m_FanucRobotDevice;

        public string RawIn_Prod { get; set; }
        public string RawIn_Mate { get; set; }

        public RX09FanucRobotClientDevice()
        {
            m_FanucRobotDevice = new FanucRobotModbus("192.168.1.121");

            m_RawInRequireStateConfig = new FanucRobotDataConfig { DataType = FanucRobotDataTypeEnum.DO, DataAdr = "196" };
            m_EmptyOutStateConfig = new FanucRobotDataConfig { DataType = FanucRobotDataTypeEnum.DO, DataAdr = "197" };
            m_EmptyInStateConfig = new FanucRobotDataConfig { DataType = FanucRobotDataTypeEnum.DO, DataAdr = "195" };
            m_FinOutStateConfig = new FanucRobotDataConfig { DataType = FanucRobotDataTypeEnum.DO, DataAdr = "194" };
            m_RawInFinConfig = new FanucRobotDataConfig { DataType = FanucRobotDataTypeEnum.DI, DataAdr = "196" };
            m_EmptyOutFinConfig = new FanucRobotDataConfig { DataType = FanucRobotDataTypeEnum.DI, DataAdr = "197" };
            m_EmptyInFinConfig = new FanucRobotDataConfig { DataType = FanucRobotDataTypeEnum.DI, DataAdr = "195" };
            m_FinOutFinConfig = new FanucRobotDataConfig { DataType = FanucRobotDataTypeEnum.DI, DataAdr = "194" };

            m_RawInRequireAllowConfig = new FanucRobotDataConfig { DataType = FanucRobotDataTypeEnum.DO, DataAdr = "102" };
            m_EmptyInAllowConfig = new FanucRobotDataConfig { DataType = FanucRobotDataTypeEnum.DO, DataAdr = "101" };

        }

        /// <summary>
        /// 获得原料输入请求
        /// </summary>
        /// <param name="raw_in">false→true:请求生效</param>
        /// <returns>true：读取正常； false：读取异常</returns>
        public bool GetRawInRequireState(ref bool raw_in)
        {
            var ret = m_FanucRobotDevice.Read(m_RawInRequireStateConfig);
            if (ret.IsSuccess == false) return false;

            bool temp = false;
            var pret = bool.TryParse(ret.Content, out temp);
            if (pret == false) return false;

            raw_in = temp;
            return true;
        }

        /// <summary>
        /// 获得空箱回库请求
        /// </summary>
        /// <param name="empty_out">false→true:请求生效</param>
        /// <returns>true：读取正常； false：读取异常</returns>
        public bool GetEmptyOutState(ref bool empty_out)
        {

            var ret = m_FanucRobotDevice.Read(m_EmptyOutStateConfig);
            if (ret.IsSuccess == false) return false;

            bool temp = false;
            var pret = bool.TryParse(ret.Content, out temp);
            if (pret == false) return false;

            empty_out = temp;
            return true;
        }

        /// <summary>
        /// 获得成品空箱输入请求
        /// </summary>
        /// <param name="empty_in">false→true:请求生效</param>
        /// <returns>true：读取正常； false：读取异常</returns>
        public bool GetEmptyInState(ref bool empty_in)
        {
            var ret = m_FanucRobotDevice.Read(m_EmptyInStateConfig);
            if (ret.IsSuccess == false) return false;

            bool temp = false;
            var pret = bool.TryParse(ret.Content, out temp);
            if (pret == false) return false;

            empty_in = temp;
            return true;
        }

        /// <summary>
        /// 获得成品回库请求
        /// </summary>
        /// <param name="fin_out">false→true:请求生效</param>
        /// <returns>true：读取正常； false：读取异常</returns>
        public bool GetFinOutState(ref bool fin_out)
        {
            // var rets = m_FanucRobotDevice.Write(new FanucRobotDataConfig { DataType = FanucRobotDataTypeEnum.DI, DataAdr = "196" }, "True");

            var ret = m_FanucRobotDevice.Read(m_FinOutStateConfig);
            if (ret.IsSuccess == false) return false;

            bool temp = false;
            var pret = bool.TryParse(ret.Content, out temp);
            if (pret == false) return false;

            fin_out = temp;
            return true;
        }

        /// <summary>
        /// 设定原料输入请求完成
        /// </summary>
        /// <param name="raw_in_fin">true:请求完成</param>
        /// <returns>true：读取正常； false：读取异常</returns>
        public bool SetRawInFin(bool raw_in_fin)
        {
            var ret = m_FanucRobotDevice.Write(m_RawInFinConfig, raw_in_fin.ToString());
            if (ret.IsSuccess == false) return false;

            return true;
        }

        /// <summary>
        /// 设定空箱回库请求完成
        /// </summary>
        /// <param name="empty_out_fin">true:请求完成</param>
        /// <returns>true：读取正常； false：读取异常</returns>
        public bool SetEmptyOutFin(bool empty_out_fin)
        {
            var ret = m_FanucRobotDevice.Write(m_EmptyOutFinConfig, empty_out_fin.ToString());
            if (ret.IsSuccess == false) return false;

            return true;
        }

        /// <summary>
        /// 设定成品空箱输入请求完成
        /// </summary>
        /// <param name="empty_in_fin">true:请求完成</param>
        /// <returns>true：读取正常； false：读取异常</returns>
        public bool SetEmptyInFin(bool empty_in_fin)
        {
            var ret = m_FanucRobotDevice.Write(m_EmptyInFinConfig, empty_in_fin.ToString());
            if (ret.IsSuccess == false) return false;

            return true;
        }

        /// <summary>
        /// 设定成品回库请求完成
        /// </summary>
        /// <param name="fin_out_fin">true:请求完成</param>
        /// <returns>true：读取正常； false：读取异常</returns>
        public bool SetFinOutFin(bool fin_out_fin)
        {
            var ret = m_FanucRobotDevice.Write(m_FinOutFinConfig, fin_out_fin.ToString());
            if (ret.IsSuccess == false) return false;

            return true;
        }

        /// <summary>
        /// 设定报警
        /// </summary>
        /// <param name="alarm">true:报警中；false：未报警</param>
        /// <returns>true：读取正常； false：读取异常</returns>
        public bool SetAlarm(bool alarm)
        {
            //var ret = m_FanucRobotDevice.Write(m_AlarmConfig, alarm.ToString());
            //if (ret.IsSuccess == false) return false;

            //DEBUG:输出至界面

            return true;
        }

        /// <summary>
        /// 获得复位
        /// </summary>
        /// <param name="reset">true:复位中；false：非复位</param>
        /// <returns>true：读取正常； false：读取异常</returns>
        public bool GetReset(ref bool reset)
        {
            //DEBUG:从界面获取

            return true;
        }

        /// <summary>
        /// 设定复位
        /// </summary>
        /// <param name="reset">true:复位中；false：非复位</param>
        /// <returns>true：读取正常； false：读取异常</returns>
        public bool SetReset(bool reset)
        {
            //DEBUG:输出至界面

            return true;
        }

        /// <summary>
        /// 获得原料输入允许信号（硬件IO）
        /// </summary>
        /// <param name="reset">true:允许；false：不允许</param>
        /// <returns>true：读取正常； false：读取异常</returns>
        public bool GetRawInFeedingSignal(ref bool raw_in)
        {
            var ret = m_FanucRobotDevice.Read(m_RawInRequireAllowConfig);
            if (ret.IsSuccess == false) return false;

            bool temp = false;
            var pret = bool.TryParse(ret.Content, out temp);
            if (pret == false) return false;

            raw_in = temp;
            return true;
        }

        /// <summary>
        /// 获得成品空箱输入允许信号（硬件IO）
        /// </summary>
        /// <param name="reset">true:允许；false：不允许</param>
        /// <returns>true：读取正常； false：读取异常</returns>
        public bool GetEmptyInFeedingSignal(ref bool empty_in)
        {
            var ret = m_FanucRobotDevice.Read(m_EmptyInAllowConfig);
            if (ret.IsSuccess == false) return false;

            bool temp = false;
            var pret = bool.TryParse(ret.Content, out temp);
            if (pret == false) return false;

            empty_in = temp;
            return true;
        }

        /// <summary>
        /// 获得毛坯种类
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns>true：读取正常； false：读取异常</returns>
        public bool GetRawInProductType(ref string type)
        {
            //TODO:从配置文件获取
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
            //TODO:从配置文件获取
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
            //TODO:从配置文件获取
            type = RawIn_Prod;

            return true;
        }

        /// <summary>
        /// 获得成品空箱物料类型
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns>true：读取正常； false：读取异常</returns>
        public bool GetEmptyInMaterialType(ref string type)
        {
            //TODO:从配置文件获取
            type = RawIn_Mate;


            return true;
        }

        /// <summary>
        /// 获得毛坯空箱回库种类
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns>true：读取正常； false：读取异常</returns>
        public bool GetEmptyOutProductType(ref string type)
        {
            //TODO:从配置文件获取
            type = RawIn_Prod;

            return true;
        }

        /// <summary>
        /// 获得毛坯空箱回库物料类型
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns>true：读取正常； false：读取异常</returns>
        public bool GetEmptyOutMaterialType(ref string type)
        {
            //TODO:从配置文件获取
            type = RawIn_Mate;


            return true;
        }

        /// <summary>
        /// 获得成品回库种类
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns>true：读取正常； false：读取异常</returns>
        public bool GetFinOutProductType(ref string type)
        {
            //TODO:从配置文件获取
            type = RawIn_Prod;

            return true;
        }

        /// <summary>
        /// 获得成品回库物料类型
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns>true：读取正常； false：读取异常</returns>
        public bool GetFinOutMaterialType(ref string type)
        {
            //TODO:从配置文件获取
            type =RawIn_Mate;

            return true;
        }
    }
}
