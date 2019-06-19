using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RightMaterialService
{
    public class BaseRightMaterialService
    {
        public Queue<RightMaterialOutMisson> OutMissions { get; set; }

        public Queue<RightMaterialInMisson> InMissions { get; set; }

        public BaseRightMaterialService()
        {
            OutMissions = new Queue<RightMaterialOutMisson>();

            InMissions = new Queue<RightMaterialInMisson>();
        }

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
