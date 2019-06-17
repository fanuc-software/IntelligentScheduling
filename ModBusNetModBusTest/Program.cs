using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Modbus.Net;
using Modbus.Net.Modbus;

namespace ModBusNetModBusTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var _modbusTcpMachine = new ModbusMachine(ModbusType.Tcp, "192.168.0.11", null, true, 2, 0);

            #region DO READ
            if (false)
            {
                var addresses = new List<AddressUnit>();
                for (int i = 0; i < 200; i++)
                {
                    addresses.Add(
                                        new AddressUnit
                                        {
                                            Id = i.ToString(),
                                            Area = "1X",
                                            Address = 1 + i,
                                            SubAddress = 0,
                                            CommunicationTag = "A" + i.ToString(),
                                            DataType = typeof(bool)
                                        }
                        );
                }



                _modbusTcpMachine.GetAddresses = addresses;

                var ans = _modbusTcpMachine.GetDatas(MachineGetDataType.Address);

                foreach (var an in ans)
                {
                    if (an.Value.PlcValue != 0)
                        Console.WriteLine(an.Key);
                }
            }
            #endregion
            
            #region DI READ
            if (false)
            {
                var addresses = new List<AddressUnit>();
                for (int i = 0; i < 200; i++)
                {
                    addresses.Add(
                                        new AddressUnit
                                        {
                                            Id = i.ToString(),
                                            Area = "0X",
                                            Address = 1 + i,
                                            SubAddress = 0,
                                            CommunicationTag = "A" + i.ToString(),
                                            DataType = typeof(bool)
                                        }
                        );
                }



                _modbusTcpMachine.GetAddresses = addresses;

                var ans = _modbusTcpMachine.GetDatas(MachineGetDataType.Address);

                foreach (var an in ans)
                {
                    if (an.Value.PlcValue != 0)
                        Console.WriteLine(an.Key);
                }
            }
            #endregion

            #region DI WRITE
            if (false)
            {

                var dic2 = new Dictionary<string, double>()
                {
                    {
                        "0X 72", 0
                    }
                };
                
                var ret =  _modbusTcpMachine.BaseUtility.GetUtilityMethods<IUtilityMethodWriteSingle>().SetSingleDataAsync("0X 72", dic2["0X 72"] >= 1).Result;

                //var addresses = new List<AddressUnit>()
                //{
                //    new AddressUnit
                //    {
                //        Id = "2",
                //        Area = "0X",
                //        Address = 71,
                //        SubAddress = 0,
                //        CommunicationTag = "B71",
                //        DataType = typeof(bool)
                //    }
                //};

                //var dic1 = new Dictionary<string, double>()
                //{
                //    {
                //       "0X 71", 0
                //    }
                //};

                //_modbusTcpMachine.GetAddresses = addresses;
                //var ret = _modbusTcpMachine.SetDatas(MachineSetDataType.Address, dic1);
                //var ans = _modbusTcpMachine.GetDatas(MachineGetDataType.Address);
                Console.WriteLine(ret);
            }
            #endregion

            #region GO READ
            if (false)
            {
                var addresses = new List<AddressUnit>()
                {
                    new AddressUnit
                    {
                        Id = "81",
                        Area = "1X",
                        Address = 81,
                        SubAddress = 0,
                        CommunicationTag = "GO1",
                        DataType = typeof(UInt16)
                    },
                    new AddressUnit
                    {
                        Id = "97",
                        Area = "1X",
                        Address = 97,
                        SubAddress = 0,
                        CommunicationTag = "GO2",
                        DataType = typeof(UInt16)
                    },
                };



                _modbusTcpMachine.GetAddresses = addresses;

                var ans = _modbusTcpMachine.GetDatas(MachineGetDataType.Address);

                foreach (var an in ans)
                {
                    if (an.Value.PlcValue != 0)
                        Console.WriteLine(an.Key+":"+ an.Value.PlcValue);
                }
            }

            #endregion

            #region GI WRITE
            if(false)
            {
                var adr = "81";
                ushort data = 10;

                int iAdr;
                var ret = int.TryParse(adr, out iAdr);

                for (int i = 0; i < 16; i++)
                {
                    var div_adr = "0X " + (iAdr + i).ToString();
                    double div_data = data & (0x01 << i);

                    var dic = new Dictionary<string, double>()
                    {
                        {
                            div_adr, div_data
                        }
                    };

                    ret = AsyncHelper.RunSync(() =>
                        _modbusTcpMachine.BaseUtility.GetUtilityMethods<IUtilityMethodWriteSingle>().SetSingleDataAsync(div_adr, dic[div_adr] >= 1));
                    
                }

                Console.WriteLine("HERE");
            }

            #endregion

            #region 连续读取
            if (false)
            {
                CancellationTokenSource token = new CancellationTokenSource();
                Task.Factory.StartNew(() =>
                {
                    var _modbusTcpMachine2 = new ModbusMachine(ModbusType.Tcp, "192.168.0.11", null, true, 2, 0);

                    while (true)
                    {

                        if (true)
                        {
                            var addresses = new List<AddressUnit>();
                            for (int i = 0; i < 200; i++)
                            {
                                addresses.Add(
                                                    new AddressUnit
                                                    {
                                                        Id = i.ToString(),
                                                        Area = "1X",
                                                        Address = 1 + i,
                                                        SubAddress = 0,
                                                        CommunicationTag = "A" + i.ToString(),
                                                        DataType = typeof(bool)
                                                    }
                                    );
                            }

                            _modbusTcpMachine2.GetAddresses = addresses;

                            var ans = _modbusTcpMachine2.GetDatas(MachineGetDataType.Address) ?? new Dictionary<string, ReturnUnit>();

                            foreach (var an in ans)
                            {
                                if (an.Value.PlcValue != 0)
                                {
                                    Console.WriteLine(an.Key);
                                }
                            }
                            Console.WriteLine(DateTime.Now);
                        }


                    }
                    _modbusTcpMachine2.Disconnect();

                }, token.Token);
            }

            #endregion

            _modbusTcpMachine.Disconnect();
            Console.ReadKey();
        }
    }
}
