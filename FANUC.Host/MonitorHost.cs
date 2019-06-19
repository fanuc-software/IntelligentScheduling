using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FANUC.Host
{
    [System.Runtime.Remoting.Contexts.Synchronization]
    public class MonitorHost : System.ContextBoundObject
    
    {
        string Name;
        static object obj = new object();
        private static ReaderWriterLock m_readerWriterLock = new ReaderWriterLock();

        public event Action<string> ConsoleInfoEvent;
        public MonitorHost(string name)
        {
            Name = name;
            m_readerWriterLock.AcquireWriterLock(100000);
        }

        public void StepA()
        {

            Thread.Sleep(2000);
            ConsoleInfoEvent?.Invoke($"【{Thread.CurrentThread.Name}】【{Name}】: 【StepA】 {DateTime.Now}");
        }

        public void StepB()
        {

            Thread.Sleep(2000);
            ConsoleInfoEvent?.Invoke($"【{Thread.CurrentThread.Name}】【{Name}】: 【StepB】 {DateTime.Now}");
        }

        public void StepC()
        {

            Thread.Sleep(2000);
            ConsoleInfoEvent?.Invoke($"【{Thread.CurrentThread.Name}】【{Name}】: 【StepC】 {DateTime.Now}");
            m_readerWriterLock.ReleaseWriterLock();

        }


    }
}
