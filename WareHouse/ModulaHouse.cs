using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WareHouse
{
    public class ModulaHouse : IWareHouse
    {
        public bool GetHousePosition(int product_type, int material_type, ref int product_position, ref int tray_position)
        {
            return true;
        }

        public bool MoveOutHouseTray()
        {
            return true;
        }

        public bool MoveInHouseTray()
        {
            return true;
        }

        public bool HouseDataInputRequest()
        {
            return true;
        }
    }
}
