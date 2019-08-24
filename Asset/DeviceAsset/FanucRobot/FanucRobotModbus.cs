using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HslCommunication;
using Modbus.Net;
using Modbus.Net.Modbus;

namespace DeviceAsset
{
    public class FanucRobotModbus
    {
        public string IP { get; set; }

        private ModbusMachine _clinet;

        private const int MaxRecon = 3;

        public FanucRobotModbus(string ip)
        {
            IP = ip;
            _clinet = new ModbusMachine(ModbusType.Tcp, IP, null, true, 2, 0);
        }
        
        public OperateResult Write(FanucRobotDataConfig dataConfig, string dataValue)
        {


            try
            {
                var ret = false;
                switch(dataConfig.DataType)
                {
                    case FanucRobotDataTypeEnum.DI:
                        bool bTemp;
                        ret = bool.TryParse(dataValue, out bTemp);
                        if(ret==true)
                        {
                            ret = WriteDI(dataConfig.DataAdr, bTemp);
                        }
                        break;
                    case FanucRobotDataTypeEnum.GI:
                        ushort iTemp;
                        ret = ushort.TryParse(dataValue, out iTemp);
                        if (ret == true)
                        {
                            ret = WriteGI(dataConfig.DataAdr, iTemp);
                        }
                        break;
                    default:
                        break;
                }

                if(ret==false)
                {
                    return new OperateResult<string>("机器人通讯写入信号失败");
                }

                return OperateResult.CreateSuccessResult();
            }
            catch
            {
                return new OperateResult<string>("机器人通讯写入信号发生致命错误");
            }
        }

        public OperateResult<string> Read(FanucRobotDataConfig dataConfig)
        {
            OperateResult<string> isRead = new OperateResult<string> { IsSuccess = false };

            try
            {
                bool ret = false;
                switch (dataConfig.DataType)
                {
                    case FanucRobotDataTypeEnum.DI:
                        bool bTemp = false;
                        ret = ReadDI(dataConfig.DataAdr, ref bTemp);
                        if(ret==true)
                        {
                            isRead.IsSuccess = true;
                            isRead.Content = bTemp.ToString();
                        }
                        break;
                    case FanucRobotDataTypeEnum.DO:
                        bTemp = false;
                        ret = ReadDO(dataConfig.DataAdr, ref bTemp);
                        if (ret == true)
                        {
                            isRead.IsSuccess = true;
                            isRead.Content = bTemp.ToString();
                        }
                        break;
                    case FanucRobotDataTypeEnum.GI:
                        ushort iTemp = 0;
                        ret = ReadGI(dataConfig.DataAdr, ref iTemp);
                        if (ret == true)
                        {
                            isRead.IsSuccess = true;
                            isRead.Content = iTemp.ToString();
                        }
                        break;
                    case FanucRobotDataTypeEnum.GO:
                        iTemp = 0;
                        ret = ReadGO(dataConfig.DataAdr, ref iTemp);
                        if (ret == true)
                        {
                            isRead.IsSuccess = true;
                            isRead.Content = iTemp.ToString();
                        }
                        break;
                    default:
                        break;

                }

                return isRead;
            }
#pragma warning disable CS0168 // 声明了变量“ex”，但从未使用过
            catch(Exception ex)
#pragma warning restore CS0168 // 声明了变量“ex”，但从未使用过
            {
                return new OperateResult<string>("机器人读取信号发生致命错误");
            }
        }

        private bool ReadDI(string adr, ref bool data)
        {
            var clinet = new ModbusMachine(ModbusType.Tcp, IP, null, false, 2, 0);
            clinet.Connect();

            int iAdr;
            var ret = int.TryParse(adr, out iAdr);
            if (ret != true) return ret;

            var addresses = new List<AddressUnit>()
            {
                new AddressUnit
                {
                    Id = adr,
                    Area = "0X",
                    Address = iAdr,
                    SubAddress = 0,
                    CommunicationTag = "DI" + adr,
                    DataType = typeof(bool)
                }
            };

            for (int j = 0; j < MaxRecon; j++)
            {
                clinet.GetAddresses = addresses;
                var ans = clinet.GetDatas(MachineGetDataType.CommunicationTag);

                if (ans != null)
                {
                    data = ans["DI" + adr].PlcValue > 0 ? true : false;
                    clinet.Disconnect();
                    return true;
                }
            }

            clinet.Disconnect();
            return false;


        }

        private bool ReadDO(string adr, ref bool data)
        {
            var clinet = new ModbusMachine(ModbusType.Tcp, IP, null, false, 2, 0);
            clinet.Connect();

            int iAdr;
            var ret = int.TryParse(adr, out iAdr);
            if (ret != true) return ret;

            var addresses = new List<AddressUnit>()
            {
                new AddressUnit
                {
                    Id = adr,
                    Area = "1X",
                    Address = iAdr,
                    SubAddress = 0,
                    CommunicationTag = "DO" + adr,
                    DataType = typeof(bool)
                }
            };

            for (int j = 0; j < MaxRecon; j++)
            {
                clinet.GetAddresses = addresses;
                var ans = clinet.GetDatas(MachineGetDataType.CommunicationTag);

                if (ans != null)
                {
                    data = ans["DO" + adr].PlcValue > 0 ? true : false;
                    clinet.Disconnect();
                    return true;
                }
            }

            clinet.Disconnect();
            return false;
        }

        private bool WriteDI(string adr, bool data)
        {
            var clinet = new ModbusMachine(ModbusType.Tcp, IP, null, false, 2, 0);
            clinet.Connect();

            int iAdr;
            var ret = int.TryParse(adr, out iAdr);
            if (ret != true) return ret;

            var div_adr = "0X " + iAdr.ToString();

            double div_data = data == false ? 0 : 1;

            var dic = new Dictionary<string, double>()
            {
                {
                    div_adr, div_data
                }
            };

            for (int j = 0; j < MaxRecon; j++)
            {
                ret = AsyncHelper.RunSync(() =>
                    clinet.BaseUtility.GetUtilityMethods<IUtilityMethodWriteSingle>().SetSingleDataAsync(div_adr, dic[div_adr] >= 1));

                if (ret == true) break;
            }

            clinet.Disconnect();
            return ret;
        }

        private bool ReadGO(string adr, ref ushort data)
        {
            var clinet = new ModbusMachine(ModbusType.Tcp, IP, null, false, 2, 0);
            clinet.Connect();

            int iAdr;
            var ret = int.TryParse(adr, out iAdr);
            if (ret != true) return ret;

            var addresses = new List<AddressUnit>()
            {
                new AddressUnit
                {
                    Id = "LOWPART",
                    Area = "1X",
                    Address = iAdr,
                    SubAddress = 0,
                    CommunicationTag = "LOWPART",
                    DataType = typeof(byte)
                },
                new AddressUnit
                {
                    Id = "HIGHPART",
                    Area = "1X",
                    Address = iAdr+8,
                    SubAddress = 0,
                    CommunicationTag = "HIGHPART",
                    DataType = typeof(byte)
                },
            };

            for (int j = 0; j < MaxRecon; j++)
            {
                clinet.GetAddresses = addresses;
                var ans = clinet.GetDatas(MachineGetDataType.CommunicationTag);

                if (ans != null)
                {
                    data = (ushort)(ans["LOWPART"].PlcValue + ans["HIGHPART"].PlcValue * 256);
                    return true;
                }
            }

            clinet.Disconnect();
            return false;
        }

        private bool ReadGI(string adr, ref ushort data)
        {
            int iAdr;
            var ret = int.TryParse(adr, out iAdr);
            if (ret != true) return ret;

            var addresses = new List<AddressUnit>()
            {
                new AddressUnit
                {
                    Id = "LOWPART",
                    Area = "0X",
                    Address = iAdr,
                    SubAddress = 0,
                    CommunicationTag = "LOWPART",
                    DataType = typeof(byte)
                },
                new AddressUnit
                {
                    Id = "HIGHPART",
                    Area = "0X",
                    Address = iAdr+8,
                    SubAddress = 0,
                    CommunicationTag = "HIGHPART",
                    DataType = typeof(byte)
                },
            };
            
            for (int j = 0; j < MaxRecon; j++)
            {
                _clinet.GetAddresses = addresses;
                var ans = _clinet.GetDatas(MachineGetDataType.CommunicationTag);

                if (ans != null)
                {
                    data = (ushort)(ans["LOWPART"].PlcValue + ans["HIGHPART"].PlcValue * 256);
                    return true;
                } 
            }

            return false;

            
        }
        
        private bool WriteGI(string adr, ushort data)
        {
            int iAdr;
            var ret = int.TryParse(adr, out iAdr);
            if (ret != true) return ret;

            for(int i=0;i<16;i++)
            {
                var div_adr = "0X " + (iAdr + i).ToString();
                double div_data = data & (0x01 << i);

                var dic = new Dictionary<string, double>()
                {
                    {
                        div_adr, div_data
                    }
                };

                for (int j = 0; j < MaxRecon; j++)
                {
                    ret = AsyncHelper.RunSync(() =>
                        _clinet.BaseUtility.GetUtilityMethods<IUtilityMethodWriteSingle>().SetSingleDataAsync(div_adr, dic[div_adr] >= 1));

                    if (ret == true) break; 
                }
                if(ret!=true)
                {
                    return ret;
                }

            }
            
            return true;
        }
    }
}
