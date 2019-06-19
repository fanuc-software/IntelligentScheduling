using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DeviceAsset;

namespace LeftMaterialService
{
    public class ModulaWareHouseClient : IWareHouseClient
    {
        private ModulaClient m_Modula;

        public ModulaWareHouseClient()
        {
            m_Modula = new ModulaClient("192.168.1.22", 102);
        }

        public bool GetHousePosition(int product_type, int material_type, out int product_position, out int tray_position)
        {
            product_position = 0;
            tray_position = 0;

            var message = "?DPICK";

            var ret = m_Modula.Send(message);
            if (ret.Item1 == false)
            {
                return false;
            }

            if (ret.Item2.Count() < 34)
            {
                return false;
            }

            string toolCode = (ret.Item2.Substring(0, 25)).Trim();//刀具编号
            string trayCode = (ret.Item2.Substring(25, 4)).Trim();//托盘编号
            string count = (ret.Item2.Substring(29, 4)).Trim();//数量
            string mode = (ret.Item2.Substring(33, 1)).Trim();//模式P:拿 V：取
            string position = (ret.Item2.Substring(34, 2)).Trim();//

            var prod_ret = int.TryParse(toolCode, out product_position);
            var tray_ret = int.TryParse(trayCode, out tray_position);

            if(prod_ret==false || tray_ret==false)
            {
                return false;
            }

            return true;

        }

        public bool MoveOutHouseTray(int product_type, int material_type)
        {
            var message = "PART=" + material_type.ToString().PadLeft(25, ' ') + "11";
            
            var ret = m_Modula.Send(message);
            if(ret.Item1 == false )
            {
                return false;
            }

            if(ret.Item2.Count()<34)
            {
                return false;
            }

            var ok_back = ret.Item2.ElementAt(33);
            if(ok_back=='0')
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        
        public bool MoveInHouseTray(int product_type, int material_type)
        {
            var message = "RVART=" + material_type.ToString().PadLeft(25, ' ') + "11";

            var ret = m_Modula.Send(message);
            if (ret.Item1 == false)
            {
                return false;
            }

            if (ret.Item2.Count() < 35)
            {
                return false;
            }
            
            var ok_back = ret.Item2.ElementAt(34);
            if (ok_back == '0')
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool HouseDataInputRequest(int product_type, int material_type, bool in_out)
        {
            var message = "VART=" + material_type.ToString().PadLeft(25, ' ') + (in_out ? "V1" : "P1");

            var ret = m_Modula.Send(message);

            return true;
            
        }

        public bool ResetHouseTray()
        {
            var message = "FPICK=1";

            var ret = m_Modula.Send(message);
            if (ret.Item1 == false)
            {
                return false;
            }

            if (ret.Item2.Count() < 10)
            {
                return false;
            }

            var ok_back = ret.Item2.Substring(8,2);
            if (ok_back == "10")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
