using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DeviceAsset;
using WareHouseService;

namespace LeftMaterialService
{
    public class LefModulaWareHouseClient : IWareHouseClient
    {
        private IWareHouseService m_wareHouseSrv;

        public LefModulaWareHouseClient(string id)
        {
            m_wareHouseSrv = new LeftModulaService(id, "192.168.1.22", 102);
            m_wareHouseSrv.ServiceInfoEvent+= (s) => Console.WriteLine(s);
        }

        //0813
        public bool GetPositionInfo(int product_type, int material_type, out int material_position, out int tray_position,out int quantity)
        //public bool GetPositionInfo(int product_type, int material_type, out int product_position, out int tray_position, out int quantity)
        {
            var temp = m_wareHouseSrv.GetPositionInfo(new WareHousePara { Product_Type= product_type, Material_Type=material_type});

            //0813
            tray_position = temp.Result1;
            material_position = temp.Result2;
            quantity = temp.Result3;

            //product_position = temp.Result1;
            //tray_position = temp.Result2;
            //quantity = temp.Result3;

            return temp.IsSuccessed;
        }

        public bool MoveOutTray(int product_type, int material_type)
        {
            var temp = m_wareHouseSrv.MoveOutTray(new WareHousePara { Product_Type = product_type, Material_Type = material_type });
           
            return temp.IsSuccessed;
        }

        public bool MoveInTray(int product_type, int material_type)
        {
            var temp = m_wareHouseSrv.MoveInTray(new WareHousePara { Product_Type = product_type, Material_Type = material_type });

            return temp.IsSuccessed;
        }

        public bool WriteBackData(int product_type, int material_type, bool in_out)
        {
            var temp = m_wareHouseSrv.WriteBackData(new WareHousePara { Product_Type = product_type, Material_Type = material_type ,In_Out=in_out});

            return temp.IsSuccessed;
        }

        public bool ResetTray()
        {
            var temp = m_wareHouseSrv.ResetTray(new WareHousePara());

            return temp.IsSuccessed;
        }

        public bool ReleaseWriterLock()
        {
            var temp = m_wareHouseSrv.ReleaseWriterLock(new WareHousePara());

            return temp.IsSuccessed;
        }

    }
}
