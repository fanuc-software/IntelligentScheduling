using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WareHouse
{
    public interface IWareHouse
    {
        bool GetHousePosition(int product_type, int material_type, ref int product_position, ref int tray_position);

        bool MoveOutHouseTray();

        bool MoveInHouseTray();

        bool HouseDataInputRequest();

    }
}
