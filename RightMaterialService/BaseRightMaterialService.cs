using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DeviceAsset;

namespace RightMaterialService
{
    public class BaseRightMaterialService
    {
        private SeerRoboRoute seerRoboRoute;

        private static BaseRightMaterialService _instance = null;
        private CancellationTokenSource token = new CancellationTokenSource();

        //出库队列
        private List<RightMaterialOutMisson> OutMissions { get; set; }
        //入库队列
        private List<RightMaterialInMisson> InMissions { get; set; }
        
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
            OutMissions = new List<RightMaterialOutMisson>();
            InMissions = new List<RightMaterialInMisson>();

            seerRoboRoute = new SeerRoboRoute();
        }

        #endregion

        public void PushOutMission(RightMaterialOutMisson mission)
        {
            if (OutMissions.Where(x => x.Id == mission.Id).Count() == 0)
            {
                OutMissions.Add(mission);
            }
        }

        public void PushInMission(RightMaterialInMisson mission)
        {
            if (InMissions.Where(x => x.Id == mission.Id).Count() == 0)
            {
                InMissions.Add(mission);
            }
        }

        public void Start()
        {
            Task.Factory.StartNew(() =>
            {
                while (!token.IsCancellationRequested)
                {
                    

                }
            }, token.Token);
        }

        //TODO:
        public void Stop()
        {

        }
    }
}
