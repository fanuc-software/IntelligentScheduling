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
        private static ReaderWriterLock m_elevatorLock = new ReaderWriterLock();

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
            m_elevatorLock.AcquireWriterLock(100000);
        }

        public void ReleaseElevator()
        {
            m_elevatorLock.ReleaseWriterLock();
        }
    }
}
