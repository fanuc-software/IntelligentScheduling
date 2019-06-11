using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using HslCommunication;
using HslCommunication.Profinet.AllenBradley;

namespace DeviceAsset
{
    public class AllenBradley
    {
        public string IP { get; set; }

        public ushort Port { get; set; }

        public AllenBradley()
        {
            IP = "192.168.1.1";
            Port = 80;
        }

        //public OperateResult WriteBit(string dataAddress, string dataValues)
        //{
        //    try
        //    {
        //        AllenBradleyNet allenBradleyNet = new AllenBradleyNet();
        //        allenBradleyNet.IpAddress = ServerIP.ToString();
        //        allenBradleyNet.Port = ServerPort;
        //        OperateResult connect = allenBradleyNet.ConnectServer();
        //        if (!connect.IsSuccess)
        //        {
        //            return new OperateResult<string>("PLC连接失败");
        //        }
        //        //todo:读数据
        //        OperateResult isWrite = allenBradleyNet.Write(dataAddress, Int16.Parse(dataValue));
        //        if (!isWrite.IsSuccess)
        //        {
        //            return new OperateResult<string>("PLC写入失败");
        //        }
        //        allenBradleyNet.ConnectClose();


        //        return OperateResult.CreateSuccessResult();
        //    }
        //    catch (Exception ex)
        //    {
        //        string error = $"读取 设备IP({ServerIP}) 地址({dataAddress}) 失败，错误为({ex.Message.ToString()})";
        //        Console.WriteLine(error);
        //        return new OperateResult<string>(error);
        //    }
        //}
    }
}
