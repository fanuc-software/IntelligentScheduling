using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RightCarryService
{
    [System.Runtime.Remoting.Contexts.Synchronization]
    public class TestRightCarryService : BaseRightCarryService
    {
        public override IControlDevice ControlDevice => new AllenBradleyControlDevice();
    }
}
