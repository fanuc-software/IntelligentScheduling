using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DeviceAsset;

namespace WareHouseService
{
    [System.Runtime.Remoting.Contexts.Synchronization]
    public class RightModulaService : System.ContextBoundObject, IWareHouseService
    {
        private string m_client;
        static object obj = new object();
        private static ReaderWriterLock m_readerWriterLock = new ReaderWriterLock();

        private ModulaClient m_Modula;

        public event Action<string> ServiceInfoEvent;

        public RightModulaService(string client, string ip, ushort port)
        {
            m_client = client;

            m_Modula = new ModulaClient(ip, port);
            m_readerWriterLock.AcquireWriterLock(100000);
        }

        public WareHouseResult GetPositionInfo(WareHousePara para)
        {
            var elevator = ModulaElevator.CreateInstance();
            elevator.ReleaseElevator();

            ServiceInfoEvent?.Invoke($"【{Thread.CurrentThread.Name}】【{m_client}】: 【GetPositionInfo】 {DateTime.Now}");

            var result = new WareHouseResult() { IsSuccessed = false };

            var message = "?DPICK";

            var ret = m_Modula.Send(message);
            if (ret.Item1 == false)
            {
                return new WareHouseResult() { IsSuccessed = false };
            }

            if (ret.Item2.Count() < 34)
            {
                return new WareHouseResult() { IsSuccessed = false };
            }

            var removestartstr = ret.Item2.Replace("DPICK=", "");

            //string toolCode = (removestartstr.Substring(0, 25)).Trim();//刀具编号
            //string trayCode = (removestartstr.Substring(25, 4)).Trim();//托盘编号
            //string count = (removestartstr.Substring(29, 4)).Trim();//数量
            //string mode = (removestartstr.Substring(33, 1)).Trim();//模式P:拿 V：取
            //string position = (removestartstr.Substring(34, 2)).Trim();//

            //int material_position;
            //int tray_position;
            //int quantity;
            //var prod_ret = int.TryParse(position, out material_position);
            //var tray_ret = int.TryParse(trayCode, out tray_position);
            //var qty_ret = int.TryParse(count, out quantity);

            string toolCode = (removestartstr.Substring(0, 25)).Trim();//刀具编号
            string trayCode = (removestartstr.Substring(25, 4)).Trim();//托盘编号
            string count = (removestartstr.Substring(29, 4)).Trim();//数量
            string mode = (removestartstr.Substring(33, 1)).Trim();//模式P:拿 V：取
            string position = (removestartstr.Substring(34, 2)).Trim();//

            int material_position;
            int tray_position;
            int quantity;
            var prod_ret = int.TryParse(position, out material_position);
            var tray_ret = int.TryParse(trayCode, out tray_position);
            var qty_ret = int.TryParse(count, out quantity);

            if (prod_ret == false || tray_ret == false || qty_ret == false)
            {
                return new WareHouseResult() { IsSuccessed = false };
            }

            return new WareHouseResult() { IsSuccessed = true, Result1 = tray_position, Result2 = material_position, Result3 = quantity };
        }

        public WareHouseResult MoveOutTray(WareHousePara para)
        {
            var elevator = ModulaElevator.CreateInstance();
            elevator.GetElevator();

            ServiceInfoEvent?.Invoke($"【{Thread.CurrentThread.Name}】【{m_client}】: 【MoveOutTray】 {DateTime.Now}");

            var message = "PART=" + para.Material_Type.ToString().PadLeft(25, ' ') + "11";

            var ret = m_Modula.Send(message);
            if (ret.Item1 == false)
            {
                elevator.ReleaseElevator();
                return new WareHouseResult() { IsSuccessed = false };
            }

            if (ret.Item2.Count() < 34)
            {
                elevator.ReleaseElevator();
                return new WareHouseResult() { IsSuccessed = false };
            }

            var ok_back = ret.Item2.ElementAt(33);
            if (ok_back == '0')
            {
                return new WareHouseResult() { IsSuccessed = true };
            }
            else
            {
                elevator.ReleaseElevator();
                return new WareHouseResult() { IsSuccessed = false };
            }
        }

        public WareHouseResult MoveInTray(WareHousePara para)
        {
            var elevator = ModulaElevator.CreateInstance();
            elevator.GetElevator();
            
            ServiceInfoEvent?.Invoke($"【{Thread.CurrentThread.Name}】【{m_client}】: 【MoveInTray】 {DateTime.Now}");

            var message = "RVART=" + para.Material_Type.ToString().PadLeft(25, ' ') + "11";

            var ret = m_Modula.Send(message);
            if (ret.Item1 == false)
            {
                elevator.ReleaseElevator();
                return new WareHouseResult() { IsSuccessed = false };
            }

            if (ret.Item2.Count() < 35)
            {
                elevator.ReleaseElevator();
                return new WareHouseResult() { IsSuccessed = false };
            }

            var ok_back = ret.Item2.ElementAt(34);
            if (ok_back == '0')
            {
                return new WareHouseResult() { IsSuccessed = true };
            }
            else
            {
                elevator.ReleaseElevator();
                return new WareHouseResult() { IsSuccessed = false };
            }
        }

        public WareHouseResult WriteBackData(WareHousePara para)
        {

            ServiceInfoEvent?.Invoke($"【{Thread.CurrentThread.Name}】【{m_client}】: 【WriteBackData】 {DateTime.Now}");

            var message = "VART=" + para.Material_Type.ToString().PadLeft(25, ' ') + (para.In_Out ? "P1" : "V1");

            var ret = m_Modula.Send(message);

            return new WareHouseResult() { IsSuccessed = true };

        }

        public WareHouseResult ResetTray(WareHousePara para)
        {
            var elevator = ModulaElevator.CreateInstance();
            elevator.GetElevator();

            ServiceInfoEvent?.Invoke($"【{Thread.CurrentThread.Name}】【{m_client}】: 【ResetTray】 {DateTime.Now}");

            var message = "FPICK=1";

            var ret = m_Modula.Send(message);
            if (ret.Item1 == false)
            {
                elevator.ReleaseElevator();
                return new WareHouseResult() { IsSuccessed = false };
            }

            if (ret.Item2.Count() < 10)
            {
                elevator.ReleaseElevator();
                return new WareHouseResult() { IsSuccessed = false };
            }

            var ok_back = ret.Item2.Substring(8, 2);
            if (ok_back == "10")
            {
                Task.Factory.StartNew(() =>
                {
                    System.Threading.Thread.Sleep(30);

                    var elevator2 = ModulaElevator.CreateInstance();
                    elevator2.ReleaseElevator();
                });

                return new WareHouseResult() { IsSuccessed = true };
            }
            else
            {
                elevator.ReleaseElevator();
                return new WareHouseResult() { IsSuccessed = false };
            }
        }

        public WareHouseResult ReleaseWriterLock(WareHousePara para)
        {
            m_readerWriterLock.ReleaseWriterLock();
           
            return new WareHouseResult { IsSuccessed = true };
        }

    }
}
