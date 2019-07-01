using DeviceAsset;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AGVConsoleTest
{
    class Program
    {
        static void Main(string[] args)
        {
            List<AGVNode> gVNodes = new List<AGVNode>()
            {
            new AGVNode(){ StationName="Wait_D",Action=AGVAction.Wait,OrderName="Wait_Order"}
            };

            Client client = new Client();
            Init();

            var transportOrder = new TransportOrder()
            {

                Deadline = DateTime.UtcNow.AddMinutes(20),
                Destinations = new List<DestinationOrder>()
                {
                     new DestinationOrder(){ LocationName="Wait_D",Operation="Wait",
                        Properties =new List<Property>()
                         {
                            }
                    },
                    new DestinationOrder(){ LocationName="Wait_D",Operation="Wait",
                        Properties =new List<Property>()
                         {
                        new Property(){ Key="device:queryAtExecuted",Value="SIG:wait"}
                            }
                    },

                    new DestinationOrder(){ LocationName="D",Operation="JackLoad",Properties=new List<Property>(){
                    }},

                },
                Properties = new List<Property>(),
                Dependencies = new List<string>()
            };

            var transportOrder2 = new TransportOrder()
            {

                Deadline = DateTime.UtcNow.AddMinutes(20),
                Destinations = new List<DestinationOrder>()
                {
                    new DestinationOrder()
                    {
                        LocationName ="D",
                        Operation ="JackLoad",
                        Properties =new List<Property>()

                    },

                    new DestinationOrder(){ LocationName="D1",Operation="JackUnload",Properties=new List<Property>()
                    {
                         new Property(){ Key="device:requestAtSend",Value="SIG:arrived"}

                    }}

                },
                Properties = new List<Property>(),
                Dependencies = new List<string>()
            };

            string name = $"Wait_Order_{DateTime.Now.ToShortTimeString()}";
            Console.WriteLine(name);
            client.TransportOrders2(name, transportOrder);
            client.TransportOrders2(name + "_2", transportOrder2);

            while (true)
            {
                Console.ReadLine();
            }
        }

        static void Init()
        {
            System.Net.Http.HttpClient httpClient = new System.Net.Http.HttpClient();
            var res = httpClient.GetAsync("http://192.168.1.111:6540/Device/Init").Result;
            Console.WriteLine(res);
        }
    }
}
