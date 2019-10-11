using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RightCarryService
{
    //  [System.Runtime.Remoting.Contexts.Synchronization]
    public abstract class BaseRightCarryService<T> where T : IControlDevice
    {
        private static readonly object carrylock = new object();

        public T ControlDevice { get; }

        public event Action<RightCarryServiceState> SendRightCarryServiceStateMessageEvent;
        private static ReaderWriterLock m_readerWriterLock = new ReaderWriterLock();

        public BaseRightCarryService(T device)
        {
            ControlDevice = device;

            //try
            //{
            //    m_readerWriterLock.AcquireWriterLock(100000);

            //}
            //catch (Exception)
            //{


            //}
        }

        public void ReleaseLock()
        {
            //try
            //{
            //    m_readerWriterLock.ReleaseWriterLock();

            //}
            //catch (Exception)
            //{


            //}
        }

        public bool CarryIn(string product, string material)
        {
            product = "1";

            //   lock (carrylock)
            {
                var ret = ControlDevice.SetRHouseFin(false);
                if (ret == false)
                {
                    SendRightCarryServiceStateMessageEvent?.Invoke(new RightCarryServiceState
                    {
                        State = RightCarryServiceStateEnum.WARN,
                        ErrorCode = RightCarryServiceErrorCodeEnum.CARRYIN_INITIAL,
                        Message = "右侧搬运入库初始化失败",
                    });
                    return ret;
                }

                int temp_type = 0;
                ret = int.TryParse(product, out temp_type);
                if (ret == false)
                {
                    SendRightCarryServiceStateMessageEvent?.Invoke(new RightCarryServiceState
                    {
                        State = RightCarryServiceStateEnum.WARN,
                        ErrorCode = RightCarryServiceErrorCodeEnum.CARRYIN_PRODUCT,
                        Message = "右侧机械手搬运出错,产品种类错误",
                    });
                    return ret;
                }

                ret = ControlDevice.SetRHouseProductType(temp_type);
                if (ret == false)
                {
                    SendRightCarryServiceStateMessageEvent?.Invoke(new RightCarryServiceState
                    {
                        State = RightCarryServiceStateEnum.WARN,
                        ErrorCode = RightCarryServiceErrorCodeEnum.CARRYIN_PRODUCT,
                        Message = "右侧机械手搬运出错,产品种类错误",
                    });
                    return ret;
                }

                int temp_material = 0;
                ret = int.TryParse(material, out temp_material);
                if (ret == false)
                {
                    SendRightCarryServiceStateMessageEvent?.Invoke(new RightCarryServiceState
                    {
                        State = RightCarryServiceStateEnum.WARN,
                        ErrorCode = RightCarryServiceErrorCodeEnum.CARRYIN_MATERIAL,
                        Message = "右侧机械手搬运出错,物料种类错误",
                    });
                    return ret;
                }

                ret = ControlDevice.SetRHouseMaterialType(temp_material);
                if (ret == false)
                {
                    SendRightCarryServiceStateMessageEvent?.Invoke(new RightCarryServiceState
                    {
                        State = RightCarryServiceStateEnum.WARN,
                        ErrorCode = RightCarryServiceErrorCodeEnum.CARRYIN_MATERIAL,
                        Message = "右侧机械手搬运出错,物料种类错误",
                    });
                    return ret;
                }

                ret = ControlDevice.SetRHouseInOut(false);
                if (ret == false)
                {
                    SendRightCarryServiceStateMessageEvent?.Invoke(new RightCarryServiceState
                    {
                        State = RightCarryServiceStateEnum.ERROR,
                        ErrorCode = RightCarryServiceErrorCodeEnum.CARRYIN_INOUT,
                        Message = "右侧机械手搬运出错,入库出库方向设定错误",
                    });
                    return ret;
                }

                ret = ControlDevice.SetRHouseRequest(true);
                if (ret == false)
                {
                    SendRightCarryServiceStateMessageEvent?.Invoke(new RightCarryServiceState
                    {
                        State = RightCarryServiceStateEnum.ERROR,
                        ErrorCode = RightCarryServiceErrorCodeEnum.CARRYIN_REQ,
                        Message = "右侧机械手搬运出错,发送入库请求错误",
                    });
                    return ret;
                }

                var in_fin = false;
                int macCount = 10;
                while (macCount-- > 0 && (in_fin == false || ret == false))
                {
                    ret = ControlDevice.GetRHouseFin(ref in_fin);

                    var in_reset = false;
                    ControlDevice.GetRHouseReset(ref in_reset);
                    if (in_reset == true)
                    {
                        SendRightCarryServiceStateMessageEvent?.Invoke(new RightCarryServiceState
                        {
                            State = RightCarryServiceStateEnum.ERROR,
                            ErrorCode = RightCarryServiceErrorCodeEnum.CARRYIN_RESET,
                            Message = "右侧机械手搬运出错,等待入库完成时复位",
                        });
                        return false;
                    }
                    Thread.Sleep(1000);
                }

                ret = ControlDevice.SetRHouseRequest(false);
                if (ret == false)
                {
                    SendRightCarryServiceStateMessageEvent?.Invoke(new RightCarryServiceState
                    {
                        State = RightCarryServiceStateEnum.ERROR,
                        ErrorCode = RightCarryServiceErrorCodeEnum.CARRYIN_REREQ,
                        Message = "右侧机械手搬运出错,复位请求信号错误",
                    });
                    return ret;
                }

                ret = ControlDevice.SetRHouseFin(false);
                if (ret == false)
                {
                    SendRightCarryServiceStateMessageEvent?.Invoke(new RightCarryServiceState
                    {
                        State = RightCarryServiceStateEnum.ERROR,
                        ErrorCode = RightCarryServiceErrorCodeEnum.CARRYIN_REFIN,
                        Message = "右侧机械手搬运出错,复位请求完成信号错误",
                    });
                    return ret;
                }

                ret = ControlDevice.SetRHouseRequest(false);
                if (ret == false)
                {
                    SendRightCarryServiceStateMessageEvent?.Invoke(new RightCarryServiceState
                    {
                        State = RightCarryServiceStateEnum.ERROR,
                        ErrorCode = RightCarryServiceErrorCodeEnum.CARRYIN_REFIN,
                        Message = "右侧机械手搬运出错,复位请求信号错误",
                    });
                    return ret;
                }

                SendRightCarryServiceStateMessageEvent?.Invoke(new RightCarryServiceState
                {
                    State = RightCarryServiceStateEnum.WARN,
                    ErrorCode = RightCarryServiceErrorCodeEnum.NORMAL,
                    Message = "右侧机械手搬运入库完成",
                });

                return true;
            }
        }

        public bool CarryOut(string product, string material, ref int quantity)
        {
            product = "1";
            int maxCount = 3;
            //    lock (carrylock)
            {
                var ret = ControlDevice.SetRHouseFin(false);
                if (ret == false)
                {
                    SendRightCarryServiceStateMessageEvent?.Invoke(new RightCarryServiceState
                    {
                        State = RightCarryServiceStateEnum.ERROR,
                        ErrorCode = RightCarryServiceErrorCodeEnum.CARRYOUT_INITIAL,
                        Message = "右侧搬运出库初始化失败",
                    });
                    return ret;
                }

                int temp_type = 0;
                ret = int.TryParse(product, out temp_type);
                if (ret == false)
                {
                    SendRightCarryServiceStateMessageEvent?.Invoke(new RightCarryServiceState
                    {
                        State = RightCarryServiceStateEnum.ERROR,
                        ErrorCode = RightCarryServiceErrorCodeEnum.CARRYOUT_PRODUCT,
                        Message = "右侧机械手搬运出错,产品种类错误",
                    });
                    return ret;
                }

                ret = ControlDevice.SetRHouseProductType(temp_type);
                if (ret == false)
                {
                    SendRightCarryServiceStateMessageEvent?.Invoke(new RightCarryServiceState
                    {
                        State = RightCarryServiceStateEnum.ERROR,
                        ErrorCode = RightCarryServiceErrorCodeEnum.CARRYOUT_PRODUCT,
                        Message = "右侧机械手搬运出错,产品种类错误",
                    });
                    return ret;
                }

                int temp_material = 0;
                ret = int.TryParse(material, out temp_material);
                if (ret == false)
                {
                    SendRightCarryServiceStateMessageEvent?.Invoke(new RightCarryServiceState
                    {
                        State = RightCarryServiceStateEnum.ERROR,
                        ErrorCode = RightCarryServiceErrorCodeEnum.CARRYOUT_MATERIAL,
                        Message = "右侧机械手搬运出错,物料种类错误",
                    });
                    return ret;
                }

                ret = ControlDevice.SetRHouseMaterialType(temp_material);
                if (ret == false)
                {
                    SendRightCarryServiceStateMessageEvent?.Invoke(new RightCarryServiceState
                    {
                        State = RightCarryServiceStateEnum.ERROR,
                        ErrorCode = RightCarryServiceErrorCodeEnum.CARRYOUT_MATERIAL,
                        Message = "右侧机械手搬运出错,物料种类错误",
                    });
                    return ret;
                }

                ret = ControlDevice.SetRHouseInOut(true);
                if (ret == false)
                {
                    SendRightCarryServiceStateMessageEvent?.Invoke(new RightCarryServiceState
                    {
                        State = RightCarryServiceStateEnum.ERROR,
                        ErrorCode = RightCarryServiceErrorCodeEnum.CARRYOUT_INOUT,
                        Message = "右侧机械手搬运出错,出库入库方向错误",
                    });
                    return ret;
                }

                ret = ControlDevice.SetRHouseRequest(true);
                if (ret == false)
                {
                    SendRightCarryServiceStateMessageEvent?.Invoke(new RightCarryServiceState
                    {
                        State = RightCarryServiceStateEnum.ERROR,
                        ErrorCode = RightCarryServiceErrorCodeEnum.CARRYOUT_REQ,
                        Message = "右侧机械手搬运出错,出库请求错误",
                    });
                    return ret;
                }

                var in_fin = false;
                maxCount = 10;
                while (maxCount-- > 0 && (in_fin == false || ret == false))
                {
                    ret = ControlDevice.GetRHouseFin(ref in_fin);

                    var in_reset = false;
                    ControlDevice.GetRHouseReset(ref in_reset);
                    if (in_reset == true)
                    {
                        SendRightCarryServiceStateMessageEvent?.Invoke(new RightCarryServiceState
                        {
                            State = RightCarryServiceStateEnum.ERROR,
                            ErrorCode = RightCarryServiceErrorCodeEnum.CARRYOUT_RESET,
                            Message = "右侧机械手搬运出错,等待出库完成时复位",
                        });
                        return false;
                    }
                    Thread.Sleep(1000);
                }

                ret = ControlDevice.SetRHouseRequest(false);
                if (ret == false)
                {
                    SendRightCarryServiceStateMessageEvent?.Invoke(new RightCarryServiceState
                    {
                        State = RightCarryServiceStateEnum.ERROR,
                        ErrorCode = RightCarryServiceErrorCodeEnum.CARRYOUT_REREQ,
                        Message = "右侧机械手搬运出错,复位请求信号错误",
                    });
                    return ret;
                }

                ret = ControlDevice.SetRHouseFin(false);
                if (ret == false)
                {
                    SendRightCarryServiceStateMessageEvent?.Invoke(new RightCarryServiceState
                    {
                        State = RightCarryServiceStateEnum.ERROR,
                        ErrorCode = RightCarryServiceErrorCodeEnum.CARRYIN_REFIN,
                        Message = "右侧机械手搬运出错,复位请求完成信号错误",
                    });
                    return ret;
                }

                ret = ControlDevice.GetRHouseQuantity(ref quantity);
                if (ret == false)
                {
                    SendRightCarryServiceStateMessageEvent?.Invoke(new RightCarryServiceState
                    {
                        State = RightCarryServiceStateEnum.ERROR,
                        ErrorCode = RightCarryServiceErrorCodeEnum.CARRYIN_REFIN,
                        Message = "右侧机械手搬运出错,获得出库数量失败",
                    });
                    return ret;
                }

                SendRightCarryServiceStateMessageEvent?.Invoke(new RightCarryServiceState
                {
                    State = RightCarryServiceStateEnum.WARN,
                    ErrorCode = RightCarryServiceErrorCodeEnum.NORMAL,
                    Message = "右侧机械手搬运出库完成",
                });

                return true;
            }
        }
    }
}
