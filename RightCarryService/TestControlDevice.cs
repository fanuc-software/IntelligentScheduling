using RightCarryService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RightCarryService
{
    public class TestControlDevice : IControlDevice
    {
        public int Product_Type{get;set;}
        public int Material_Type { get; set; }
        public bool Req { get; set; }
        public bool Fin { get; set; }
        public bool InOut { get; set; }
        public bool Alarm { get; set; }
        public bool Reset { get; set; }
        public int Qty { get; set; }

        public bool SetRHouseProductType(int prod_type)
        {
            Product_Type = prod_type;
            return true;
        }

        public bool SetRHouseMaterialType(int material_type)
        {
            Material_Type = material_type;

            return true;
        }

        public bool GetRHouseRequest(ref bool req)
        {
            req = Req;
            return true;
        }

        public bool SetRHouseRequest(bool req)
        {
            Req = req;
            return true;
        }

        public bool GetRHouseFin(ref bool fin)
        {
            fin = Fin;
            return true;
        }

        public bool SetRHouseFin(bool fin)
        {
            Fin = fin;
            return true;
        }

        public bool SetRHouseInOut(bool in_out)
        {
            InOut = in_out;
            return true;
        }

        public bool SetRHouseAlarm(bool alarm)
        {
            Alarm = alarm;
            return true;
        }

        public bool GetRHouseReset(ref bool reset)
        {
            reset = Reset;
            return true;
        }

        public bool SetRHouseReset(bool reset)
        {
            Reset = reset;
            return true;
        }

        public bool GetRHouseQuantity(ref int quantity)
        {
            quantity = Qty;
            return true;
        }
    }
}
