using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RightCarryService
{
    public class TestRightCarryService<T> : BaseRightCarryService<T> where T:IControlDevice
    {
        //public override IControlDevice ControlDevice => new AllenBradleyControlDevice();
        public TestRightCarryService(T testDevice) : base(testDevice)
        {

        }
    }
}
