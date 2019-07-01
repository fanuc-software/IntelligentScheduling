using Agv.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    public class SignalrTest
    {
        private static SignalrService signalrService;

        public async static void MainTest()
        {
            signalrService = new SignalrService("http://localhost/Agv", "AgvMissonHub");
            signalrService.OnMessage<AgvOutMisson>(AgvReceiveActionEnum.receiveOutMissionMessage.EnumToString()
                , (s) =>
            {
                Console.WriteLine("[receive]:" + s.Id);
            });

            await signalrService.Start();

            while (true)
            {

                string data = Console.ReadLine();
                Console.WriteLine("[send]:" + data);
                await signalrService.Send<AgvOutMisson>(AgvSendActionEnum.SendOutMission.EnumToString(),
                    new AgvOutMisson()
                {
                    Id = data
                });
            }
        }



    }
}
