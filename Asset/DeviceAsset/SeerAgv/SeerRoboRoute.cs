using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeviceAsset
{
    public class SeerRoboRoute
    {
        private static SeerRoboRoute _instance = null;
        //public event Action<RightMaterialInMisson> UpdateRightMaterialInMissonEvent;

        #region ctor
        public static SeerRoboRoute CreateInstance()
        {
            if (_instance == null)

            {
                _instance = new SeerRoboRoute();
            }
            return _instance;
        }


        public SeerRoboRoute()
        {

        }

        #endregion

        public void OnInitial()
        {

        }

        private void PollAgvOrderState()
        {

        }
    }
}
