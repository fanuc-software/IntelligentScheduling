using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeftMaterialService
{
    public interface IWareHouseClient
    {
        bool GetHousePosition(int product_type, int material_type, out int product_position, out int tray_position);

        bool MoveOutHouseTray(int product_type, int material_type);

        bool MoveInHouseTray(int product_type, int material_type);

        bool HouseDataInputRequest(int product_type, int material_type, bool in_out);

        bool ResetHouseTray();

    }
}
