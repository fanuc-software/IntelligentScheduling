using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeftMaterialService
{
    public interface IWareHouseClient
    {
        bool GetPositionInfo(int product_type, int material_type, out int product_position, out int tray_position);

        bool MoveOutTray(int product_type, int material_type);

        bool MoveInTray(int product_type, int material_type);

        bool WriteBackData(int product_type, int material_type, bool in_out);

        bool ResetTray();

        bool ReleaseWriterLock();

    }
}
