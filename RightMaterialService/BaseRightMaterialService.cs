using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RightMaterialService
{
    public class BaseRightMaterialService
    {
        private static BaseRightMaterialService _instance = null;

        //出库队列
        private Queue<RightMaterialOutMisson> OutMissions { get; set; }
        //入库队列
        private Queue<RightMaterialInMisson> InMissions { get; set; }
        
        public event Action<RightMaterialInMisson> UpdateRightMaterialInMissonEvent;

        public event Action<RightMaterialOutMisson> UpdateRightMaterialOutMissonEvent;

        #region ctor
        public static BaseRightMaterialService CreateInstance()
        {
            if (_instance == null)

            {
                _instance = new BaseRightMaterialService();
            }
            return _instance;
        }

        public BaseRightMaterialService()
        {
            OutMissions = new Queue<RightMaterialOutMisson>();

            InMissions = new Queue<RightMaterialInMisson>();
        }

        #endregion

        public void PushOutMission()
        {

        }

        public void PushInMission()
        {

        }

        public void OnStart()
        {

        }
    }
}
