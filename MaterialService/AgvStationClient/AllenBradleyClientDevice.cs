using DeviceAsset;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgvStationClient
{
    public class AllenBradleyClientDevice: IStationDevice
    {
        private AllenBradleyDataConfig m_RawInRequireStateConfig;
        private AllenBradleyDataConfig m_EmptyOutStateConfig;
        private AllenBradleyDataConfig m_EmptyInStateConfig;
        private AllenBradleyDataConfig m_FinOutStateConfig;
        private AllenBradleyDataConfig m_RawInFinConfig;
        private AllenBradleyDataConfig m_EmptyOutFinConfig;
        private AllenBradleyDataConfig m_EmptyInFinConfig;
        private AllenBradleyDataConfig m_FinOutFinConfig;
        private AllenBradleyDataConfig m_AlarmConfig;
        private AllenBradleyDataConfig m_ResetConfig;
        private AllenBradleyDataConfig m_RawInRequireAllowConfig;
        private AllenBradleyDataConfig m_EmptyInAllowConfig;
        private AllenBradleyDataConfig m_RawInProductTypeConfig;
        private AllenBradleyDataConfig m_RawInMaterialTypeConfig;
        private AllenBradleyDataConfig m_EmptyInProductTypeConfig;
        private AllenBradleyDataConfig m_EmptyInMaterialTypeConfig;
        private AllenBradleyDataConfig m_EmptyOutProductTypeConfig;
        private AllenBradleyDataConfig m_EmptyOutMaterialTypeConfig;
        private AllenBradleyDataConfig m_FinOutProductTypeConfig;
        private AllenBradleyDataConfig m_FinOutMaterialTypeConfig;

        private AllenBradley m_ABDevice;

        public AllenBradleyClientDevice()
        {
            m_ABDevice = new AllenBradley("192.168.1.81", 44818);//RX07 AB  PLC的IP

            m_RawInRequireStateConfig = new AllenBradleyDataConfig { DataType = AllenBradleyDataTypeEnum.SHORT, DataAdr = "PLC_MES_COMM.AGV_Fix_Robot_IN.Product_Type" };
            m_RawInRequireStateConfig = new AllenBradleyDataConfig { DataType = AllenBradleyDataTypeEnum.SHORT, DataAdr = "PLC_MES_COMM.AGV_Fix_Robot_IN.Material_Type" };
            m_EmptyInStateConfig = new AllenBradleyDataConfig { DataType = AllenBradleyDataTypeEnum.BOOL, DataAdr = "PLC_MES_COMM.AGV_Fix_Robot_IN.Storage_Out" };
            m_FinOutStateConfig = new AllenBradleyDataConfig { DataType = AllenBradleyDataTypeEnum.BOOL, DataAdr = "PLC_MES_COMM.AGV_Fix_Robot_IN.Storage_In" };
            m_RawInFinConfig = new AllenBradleyDataConfig { DataType = AllenBradleyDataTypeEnum.BOOL, DataAdr = "PLC_MES_COMM.AGV_Fix_Robot_IN.Act_Req" };
            m_EmptyOutFinConfig = new AllenBradleyDataConfig { DataType = AllenBradleyDataTypeEnum.BOOL, DataAdr = "PLC_MES_COMM.AGV_Fix_Robot_OUT.Act_Finish" };
            m_EmptyInFinConfig = new AllenBradleyDataConfig { DataType = AllenBradleyDataTypeEnum.BOOL, DataAdr = "PLC_MES_COMM.AGV_Fix_Robot_IN.Alarm" };
            m_FinOutFinConfig = new AllenBradleyDataConfig { DataType = AllenBradleyDataTypeEnum.BOOL, DataAdr = "PLC_MES_COMM.AGV_Fix_Robot_OUT.Reset" };

            m_RawInRequireAllowConfig = new AllenBradleyDataConfig { DataType = AllenBradleyDataTypeEnum.SHORT, DataAdr = "PLC_MES_COMM.AGV_Fix_Robot_IN.Current_Quantity_In_Box" };
            m_EmptyInAllowConfig = new AllenBradleyDataConfig { DataType = AllenBradleyDataTypeEnum.SHORT, DataAdr = "PLC_MES_COMM.AGV_Fix_Robot_IN.Current_Quantity_In_Box" };

            //m_RawInProductTypeConfig = new AllenBradleyDataConfig { DataType = AllenBradleyDataTypeEnum.SHORT, DataAdr = "PLC_MES_COMM.AGV_Fix_Robot_IN.Current_Quantity_In_Box" };
            //m_RawInMaterialTypeConfig = new AllenBradleyDataConfig { DataType = AllenBradleyDataTypeEnum.SHORT, DataAdr = "PLC_MES_COMM.AGV_Fix_Robot_IN.Current_Quantity_In_Box" };
            //m_EmptyInProductTypeConfig = new AllenBradleyDataConfig { DataType = AllenBradleyDataTypeEnum.SHORT, DataAdr = "PLC_MES_COMM.AGV_Fix_Robot_IN.Current_Quantity_In_Box" };
            //m_EmptyInMaterialTypeConfig = new AllenBradleyDataConfig { DataType = AllenBradleyDataTypeEnum.SHORT, DataAdr = "PLC_MES_COMM.AGV_Fix_Robot_IN.Current_Quantity_In_Box" };
            //m_EmptyOutProductTypeConfig = new AllenBradleyDataConfig { DataType = AllenBradleyDataTypeEnum.SHORT, DataAdr = "PLC_MES_COMM.AGV_Fix_Robot_IN.Current_Quantity_In_Box" };
            //m_EmptyOutMaterialTypeConfig = new AllenBradleyDataConfig { DataType = AllenBradleyDataTypeEnum.SHORT, DataAdr = "PLC_MES_COMM.AGV_Fix_Robot_IN.Current_Quantity_In_Box" };
            //m_FinOutProductTypeConfig = new AllenBradleyDataConfig { DataType = AllenBradleyDataTypeEnum.SHORT, DataAdr = "PLC_MES_COMM.AGV_Fix_Robot_IN.Current_Quantity_In_Box" };
            //m_FinOutMaterialTypeConfig = new AllenBradleyDataConfig { DataType = AllenBradleyDataTypeEnum.SHORT, DataAdr = "PLC_MES_COMM.AGV_Fix_Robot_IN.Current_Quantity_In_Box" };
        }

        /// <summary>
        /// 获得原料输入请求
        /// </summary>
        /// <param name="raw_in">false→true:请求生效</param>
        /// <returns>true：读取正常； false：读取异常</returns>
        public bool GetRawInRequireState(ref bool raw_in)
        {
            var ret = m_ABDevice.Read(m_RawInRequireStateConfig);
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
            var ret = m_ABDevice.Read(m_EmptyOutStateConfig);
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
            var ret = m_ABDevice.Read(m_EmptyInStateConfig);
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
            var ret = m_ABDevice.Read(m_FinOutStateConfig);
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
            var ret = m_ABDevice.Write(m_RawInFinConfig, raw_in_fin.ToString());
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
            var ret = m_ABDevice.Write(m_EmptyOutFinConfig, empty_out_fin.ToString());
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
            var ret = m_ABDevice.Write(m_EmptyInFinConfig, empty_in_fin.ToString());
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
            var ret = m_ABDevice.Write(m_FinOutFinConfig, fin_out_fin.ToString());
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
            //DEBUG:界面

            return true;
        }

        /// <summary>
        /// 获得复位
        /// </summary>
        /// <param name="reset">true:复位中；false：非复位</param>
        /// <returns>true：读取正常； false：读取异常</returns>
        public bool GetReset(ref bool reset)
        {
            //DEBUG:界面

            return true;
        }

        /// <summary>
        /// 设定复位
        /// </summary>
        /// <param name="reset">true:复位中；false：非复位</param>
        /// <returns>true：读取正常； false：读取异常</returns>
        public bool SetReset(bool reset)
        {
            //DEBUG:界面

            return true;
        }

        /// <summary>
        /// 获得原料输入允许信号（硬件IO）
        /// </summary>
        /// <param name="reset">true:允许；false：不允许</param>
        /// <returns>true：读取正常； false：读取异常</returns>
        public bool GetRawInFeedingSignal(ref bool raw_in)
        {
            var ret = m_ABDevice.Read(m_RawInRequireAllowConfig);
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
            var ret = m_ABDevice.Read(m_EmptyInAllowConfig);
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

            return true;
        }
    }
}
