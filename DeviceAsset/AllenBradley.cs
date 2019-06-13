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

        public AllenBradley(string ip, ushort port)
        {
            IP = ip;
            Port = port;
        }

        public OperateResult Write(AllenBradleyDataConfig dataConfig, string dataValue)
        {
            try
            {
                AllenBradleyNet allenBradleyNet = new AllenBradleyNet();
                allenBradleyNet.IpAddress = IP;
                allenBradleyNet.Port = Port;
                OperateResult connect = allenBradleyNet.ConnectServer();
                if (!connect.IsSuccess)
                {
                    return new OperateResult<string>("AB PLC连接失败");
                }

                OperateResult isWrite;
                switch (dataConfig.DataType)
                {
                    case AllenBradleyDataTypeEnum.BOOL:
                        isWrite = allenBradleyNet.Write(dataConfig.DataAdr, bool.Parse(dataValue));
                        break;
                    case AllenBradleyDataTypeEnum.BYTE:
                        isWrite = allenBradleyNet.Write(dataConfig.DataAdr, byte.Parse(dataValue));
                        break;
                    case AllenBradleyDataTypeEnum.SHORT:
                        isWrite = allenBradleyNet.Write(dataConfig.DataAdr, short.Parse(dataValue));
                        break;
                    case AllenBradleyDataTypeEnum.INT:
                        isWrite = allenBradleyNet.Write(dataConfig.DataAdr, int.Parse(dataValue));
                        break;
                    case AllenBradleyDataTypeEnum.LONG:
                        isWrite = allenBradleyNet.Write(dataConfig.DataAdr, long.Parse(dataValue));
                        break;
                    default:
                        isWrite = new OperateResult { IsSuccess = false };
                        break;
                }
                if (!isWrite.IsSuccess)
                {
                    return new OperateResult<string>("AB PLC写入失败");
                }
                allenBradleyNet.ConnectClose();


                return OperateResult.CreateSuccessResult();
            }
            catch
            {
                return new OperateResult<string>("AB PLC通讯发生致命错误");
            }
        }

        public OperateResult<string> Read(AllenBradleyDataConfig dataConfig)
        {
            try
            {
                AllenBradleyNet allenBradleyNet = new AllenBradleyNet();
                allenBradleyNet.IpAddress = IP;
                allenBradleyNet.Port = Port;
                OperateResult connect = allenBradleyNet.ConnectServer();
                if (!connect.IsSuccess)
                {
                    return new OperateResult<string>("PLC连接失败");
                }
     
                OperateResult<string> isRead =new OperateResult<string> { IsSuccess = false };

                switch (dataConfig.DataType)
                {
                    case AllenBradleyDataTypeEnum.BOOL:
                        var bRes = allenBradleyNet.ReadBool(dataConfig.DataAdr);
                        if (bRes.IsSuccess == true)
                        {
                            isRead.IsSuccess = true;
                            isRead.Content = bRes.Content.ToString();
                        }
                        break;
                    case AllenBradleyDataTypeEnum.BYTE:
                        var cRes = allenBradleyNet.ReadByte(dataConfig.DataAdr);
                        if (cRes.IsSuccess == true)
                        {
                            isRead.IsSuccess = true;
                            isRead.Content = cRes.Content.ToString();
                        }
                        break;
                    case AllenBradleyDataTypeEnum.SHORT:
                        var ui16Res = allenBradleyNet.ReadUInt16(dataConfig.DataAdr);
                        if (ui16Res.IsSuccess == true)
                        {
                            isRead.IsSuccess = true;
                            isRead.Content = ui16Res.Content.ToString();
                        }
                        break;
                    case AllenBradleyDataTypeEnum.INT:
                        var ui32Res = allenBradleyNet.ReadUInt32(dataConfig.DataAdr);
                        if (ui32Res.IsSuccess == true)
                        {
                            isRead.IsSuccess = true;
                            isRead.Content = ui32Res.Content.ToString();
                        }
                        break;
                    case AllenBradleyDataTypeEnum.LONG:
                        var ui64Res = allenBradleyNet.ReadUInt64(dataConfig.DataAdr);
                        if (ui64Res.IsSuccess == true)
                        {
                            isRead.IsSuccess = true;
                            isRead.Content = ui64Res.Content.ToString();
                        }
                        break;
                    default:
                        isRead.IsSuccess = false;
                        break;
                }
                
                allenBradleyNet.ConnectClose();
                
                return isRead;
            }
            catch
            {
                return new OperateResult<string>("AB PLC通讯发生致命错误");
            }
        }
        
    }
    
}
