using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WareHouseService
{
    public class ModulaElevator
    {
        private static ModulaElevator _instance = null;
        private static readonly Mutex mutex = new Mutex(true,"MODULA_ELEVATOR");

        public static ModulaElevator CreateInstance()
        {
            if (_instance == null)

            {
                _instance = new ModulaElevator();
            }
            return _instance;
        }

        public void GetElevator()
        {
            //mutex.WaitOne();
        }

        public void ReleaseElevator()
        {
            //mutex.ReleaseMutex();
        }
    }
}
