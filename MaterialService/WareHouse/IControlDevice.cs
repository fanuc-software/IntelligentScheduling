using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeftMaterialService
{
    public interface IControlDevice
    {
       // bool Temp_S_House_RequestFCS_Last { get; set; }

        bool GetHouseRequestFCS(ref bool req);

        bool GetHouseRequestFCSFin(ref bool req_fin);

        bool GetHouseRequestInfoFCS(ref bool req_info);

        bool GetHouseRequestInfoFCSFin(ref bool req_info_fin);

        bool GetHouseFCSAlarm(ref bool alarm);

        bool GetHouseFCSReset(ref bool reset);

        bool GetHouseProductType(ref int product);

        bool GetHouseMaterialType(ref int material);

        bool GetHouseInOut(ref bool in_out);

        bool SetHouseProductPostion(int product_pos);

        bool SetHouseTrayPostion(int tray_pos);

        bool SetHouseQuantity(int quantity);

        bool SetHouseConfirmMaterialType(int type);

        bool SetHouseRequestFCSFin(bool req_fin);

        bool SetHouseFCSAlarm(bool alarm);

        bool SetHouseFCSReset(bool alarm);

        bool SetHouseRequestInfoFCSFin(bool req_info_fin);

        bool GetHouseTrayInposition(ref bool inposition);
    }
}
