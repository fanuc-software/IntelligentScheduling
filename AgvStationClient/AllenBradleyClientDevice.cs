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

#pragma warning disable CS0649 // 从未对字段“AllenBradleyClientDevice.m_RawInRequireStateConfig”赋值，字段将一直保持其默认值 null
        private AllenBradleyDataConfig m_RawInRequireStateConfig;
#pragma warning restore CS0649 // 从未对字段“AllenBradleyClientDevice.m_RawInRequireStateConfig”赋值，字段将一直保持其默认值 null
#pragma warning disable CS0649 // 从未对字段“AllenBradleyClientDevice.m_EmptyOutStateConfig”赋值，字段将一直保持其默认值 null
        private AllenBradleyDataConfig m_EmptyOutStateConfig;
#pragma warning restore CS0649 // 从未对字段“AllenBradleyClientDevice.m_EmptyOutStateConfig”赋值，字段将一直保持其默认值 null
#pragma warning disable CS0649 // 从未对字段“AllenBradleyClientDevice.m_EmptyInStateConfig”赋值，字段将一直保持其默认值 null
        private AllenBradleyDataConfig m_EmptyInStateConfig;
#pragma warning restore CS0649 // 从未对字段“AllenBradleyClientDevice.m_EmptyInStateConfig”赋值，字段将一直保持其默认值 null
#pragma warning disable CS0649 // 从未对字段“AllenBradleyClientDevice.m_FinOutStateConfig”赋值，字段将一直保持其默认值 null
        private AllenBradleyDataConfig m_FinOutStateConfig;
#pragma warning restore CS0649 // 从未对字段“AllenBradleyClientDevice.m_FinOutStateConfig”赋值，字段将一直保持其默认值 null
#pragma warning disable CS0649 // 从未对字段“AllenBradleyClientDevice.m_RawInFinConfig”赋值，字段将一直保持其默认值 null
        private AllenBradleyDataConfig m_RawInFinConfig;
#pragma warning restore CS0649 // 从未对字段“AllenBradleyClientDevice.m_RawInFinConfig”赋值，字段将一直保持其默认值 null
#pragma warning disable CS0649 // 从未对字段“AllenBradleyClientDevice.m_EmptyOutFinConfig”赋值，字段将一直保持其默认值 null
        private AllenBradleyDataConfig m_EmptyOutFinConfig;
#pragma warning restore CS0649 // 从未对字段“AllenBradleyClientDevice.m_EmptyOutFinConfig”赋值，字段将一直保持其默认值 null
#pragma warning disable CS0649 // 从未对字段“AllenBradleyClientDevice.m_EmptyInFinConfig”赋值，字段将一直保持其默认值 null
        private AllenBradleyDataConfig m_EmptyInFinConfig;
#pragma warning restore CS0649 // 从未对字段“AllenBradleyClientDevice.m_EmptyInFinConfig”赋值，字段将一直保持其默认值 null
#pragma warning disable CS0649 // 从未对字段“AllenBradleyClientDevice.m_FinOutFinConfig”赋值，字段将一直保持其默认值 null
        private AllenBradleyDataConfig m_FinOutFinConfig;
#pragma warning restore CS0649 // 从未对字段“AllenBradleyClientDevice.m_FinOutFinConfig”赋值，字段将一直保持其默认值 null
#pragma warning disable CS0649 // 从未对字段“AllenBradleyClientDevice.m_AlarmConfig”赋值，字段将一直保持其默认值 null
        private AllenBradleyDataConfig m_AlarmConfig;
#pragma warning restore CS0649 // 从未对字段“AllenBradleyClientDevice.m_AlarmConfig”赋值，字段将一直保持其默认值 null
#pragma warning disable CS0649 // 从未对字段“AllenBradleyClientDevice.m_ResetConfig”赋值，字段将一直保持其默认值 null
        private AllenBradleyDataConfig m_ResetConfig;
#pragma warning restore CS0649 // 从未对字段“AllenBradleyClientDevice.m_ResetConfig”赋值，字段将一直保持其默认值 null
#pragma warning disable CS0649 // 从未对字段“AllenBradleyClientDevice.m_RawInRequireAllowConfig”赋值，字段将一直保持其默认值 null
        private AllenBradleyDataConfig m_RawInRequireAllowConfig;
#pragma warning restore CS0649 // 从未对字段“AllenBradleyClientDevice.m_RawInRequireAllowConfig”赋值，字段将一直保持其默认值 null
#pragma warning disable CS0649 // 从未对字段“AllenBradleyClientDevice.m_EmptyInAllowConfig”赋值，字段将一直保持其默认值 null
        private AllenBradleyDataConfig m_EmptyInAllowConfig;
#pragma warning restore CS0649 // 从未对字段“AllenBradleyClientDevice.m_EmptyInAllowConfig”赋值，字段将一直保持其默认值 null
#pragma warning disable CS0649 // 从未对字段“AllenBradleyClientDevice.m_RawInProductTypeConfig”赋值，字段将一直保持其默认值 null
        private AllenBradleyDataConfig m_RawInProductTypeConfig;
#pragma warning restore CS0649 // 从未对字段“AllenBradleyClientDevice.m_RawInProductTypeConfig”赋值，字段将一直保持其默认值 null
#pragma warning disable CS0649 // 从未对字段“AllenBradleyClientDevice.m_RawInMaterialTypeConfig”赋值，字段将一直保持其默认值 null
        private AllenBradleyDataConfig m_RawInMaterialTypeConfig;
#pragma warning restore CS0649 // 从未对字段“AllenBradleyClientDevice.m_RawInMaterialTypeConfig”赋值，字段将一直保持其默认值 null
#pragma warning disable CS0649 // 从未对字段“AllenBradleyClientDevice.m_EmptyInProductTypeConfig”赋值，字段将一直保持其默认值 null
        private AllenBradleyDataConfig m_EmptyInProductTypeConfig;
#pragma warning restore CS0649 // 从未对字段“AllenBradleyClientDevice.m_EmptyInProductTypeConfig”赋值，字段将一直保持其默认值 null
#pragma warning disable CS0649 // 从未对字段“AllenBradleyClientDevice.m_EmptyInMaterialTypeConfig”赋值，字段将一直保持其默认值 null
        private AllenBradleyDataConfig m_EmptyInMaterialTypeConfig;
#pragma warning restore CS0649 // 从未对字段“AllenBradleyClientDevice.m_EmptyInMaterialTypeConfig”赋值，字段将一直保持其默认值 null
#pragma warning disable CS0649 // 从未对字段“AllenBradleyClientDevice.m_EmptyOutProductTypeConfig”赋值，字段将一直保持其默认值 null
        private AllenBradleyDataConfig m_EmptyOutProductTypeConfig;
#pragma warning restore CS0649 // 从未对字段“AllenBradleyClientDevice.m_EmptyOutProductTypeConfig”赋值，字段将一直保持其默认值 null
#pragma warning disable CS0649 // 从未对字段“AllenBradleyClientDevice.m_EmptyOutMaterialTypeConfig”赋值，字段将一直保持其默认值 null
        private AllenBradleyDataConfig m_EmptyOutMaterialTypeConfig;
#pragma warning restore CS0649 // 从未对字段“AllenBradleyClientDevice.m_EmptyOutMaterialTypeConfig”赋值，字段将一直保持其默认值 null
#pragma warning disable CS0649 // 从未对字段“AllenBradleyClientDevice.m_FinOutProductTypeConfig”赋值，字段将一直保持其默认值 null
        private AllenBradleyDataConfig m_FinOutProductTypeConfig;
#pragma warning restore CS0649 // 从未对字段“AllenBradleyClientDevice.m_FinOutProductTypeConfig”赋值，字段将一直保持其默认值 null
#pragma warning disable CS0649 // 从未对字段“AllenBradleyClientDevice.m_FinOutMaterialTypeConfig”赋值，字段将一直保持其默认值 null
        private AllenBradleyDataConfig m_FinOutMaterialTypeConfig;
#pragma warning restore CS0649 // 从未对字段“AllenBradleyClientDevice.m_FinOutMaterialTypeConfig”赋值，字段将一直保持其默认值 null

#pragma warning disable CS0649 // 从未对字段“AllenBradleyClientDevice.m_ABDevice”赋值，字段将一直保持其默认值 null
        private AllenBradley m_ABDevice;
#pragma warning restore CS0649 // 从未对字段“AllenBradleyClientDevice.m_ABDevice”赋值，字段将一直保持其默认值 null

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
            var ret = m_ABDevice.Write(m_AlarmConfig, alarm.ToString());
            if (ret.IsSuccess == false) return false;

            return true;
        }

        /// <summary>
        /// 获得复位
        /// </summary>
        /// <param name="reset">true:复位中；false：非复位</param>
        /// <returns>true：读取正常； false：读取异常</returns>
        public bool GetReset(ref bool reset)
        {
            var ret = m_ABDevice.Read(m_ResetConfig);
            if (ret.IsSuccess == false) return false;

            bool temp = false;
            var pret = bool.TryParse(ret.Content, out temp);
            if (pret == false) return false;

            reset = temp;
            return true;
        }

        /// <summary>
        /// 设定复位
        /// </summary>
        /// <param name="reset">true:复位中；false：非复位</param>
        /// <returns>true：读取正常； false：读取异常</returns>
        public bool SetReset(bool reset)
        {
            var ret = m_ABDevice.Write(m_ResetConfig, reset.ToString());
            if (ret.IsSuccess == false) return false;

            return true;
        }

        /// <summary>
        /// 获得原料输入允许信号（硬件IO）
        /// </summary>
        /// <param name="reset">true:允许；false：不允许</param>
        /// <returns>true：读取正常； false：读取异常</returns>
        public bool CheckRawInRequireAllow(ref bool raw_in)
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
        public bool CheckEmptyInAllow(ref bool empty_in)
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
            var ret = m_ABDevice.Read(m_RawInProductTypeConfig);
            if (ret.IsSuccess == false) return false;

            int temp = 0;
            var pret = int.TryParse(ret.Content, out temp);
            if (pret == false) return false;

            type = temp.ToString();
            return true;
        }

        /// <summary>
        /// 获得毛坯物料类型
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns>true：读取正常； false：读取异常</returns>
        public bool GetRawInMaterialType(ref string type)
        {
            var ret = m_ABDevice.Read(m_RawInMaterialTypeConfig);
            if (ret.IsSuccess == false) return false;

            int temp = 0;
            var pret = int.TryParse(ret.Content, out temp);
            if (pret == false) return false;

            type = temp.ToString();
            return true;
        }

        /// <summary>
        /// 获得成品空箱种类
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns>true：读取正常； false：读取异常</returns>
        public bool GetEmptyInProductType(ref string type)
        {
            var ret = m_ABDevice.Read(m_EmptyInProductTypeConfig);
            if (ret.IsSuccess == false) return false;

            int temp = 0;
            var pret = int.TryParse(ret.Content, out temp);
            if (pret == false) return false;

            type = temp.ToString();
            return true;
        }

        /// <summary>
        /// 获得成品空箱物料类型
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns>true：读取正常； false：读取异常</returns>
        public bool GetEmptyInMaterialType(ref string type)
        {
            var ret = m_ABDevice.Read(m_EmptyInMaterialTypeConfig);
            if (ret.IsSuccess == false) return false;

            int temp = 0;
            var pret = int.TryParse(ret.Content, out temp);
            if (pret == false) return false;

            type = temp.ToString();
            return true;
        }

        /// <summary>
        /// 获得毛坯空箱回库种类
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns>true：读取正常； false：读取异常</returns>
        public bool GetEmptyOutProductType(ref string type)
        {
            var ret = m_ABDevice.Read(m_EmptyOutProductTypeConfig);
            if (ret.IsSuccess == false) return false;

            int temp = 0;
            var pret = int.TryParse(ret.Content, out temp);
            if (pret == false) return false;

            type = temp.ToString();
            return true;
        }

        /// <summary>
        /// 获得毛坯空箱回库物料类型
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns>true：读取正常； false：读取异常</returns>
        public bool GetEmptyOutMaterialType(ref string type)
        {
            var ret = m_ABDevice.Read(m_EmptyOutMaterialTypeConfig);
            if (ret.IsSuccess == false) return false;

            int temp = 0;
            var pret = int.TryParse(ret.Content, out temp);
            if (pret == false) return false;

            type = temp.ToString();
            return true;
        }

        /// <summary>
        /// 获得成品回库种类
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns>true：读取正常； false：读取异常</returns>
        public bool GetFinOutProductType(ref string type)
        {
            var ret = m_ABDevice.Read(m_FinOutProductTypeConfig);
            if (ret.IsSuccess == false) return false;

            int temp = 0;
            var pret = int.TryParse(ret.Content, out temp);
            if (pret == false) return false;

            type = temp.ToString();
            return true;
        }

        /// <summary>
        /// 获得成品回库物料类型
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns>true：读取正常； false：读取异常</returns>
        public bool GetFinOutMaterialType(ref string type)
        {
            var ret = m_ABDevice.Read(m_FinOutMaterialTypeConfig);
            if (ret.IsSuccess == false) return false;

            int temp = 0;
            var pret = int.TryParse(ret.Content, out temp);
            if (pret == false) return false;

            type = temp.ToString();
            return true;
        }
    }
}
