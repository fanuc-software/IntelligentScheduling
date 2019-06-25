using Microsoft.VisualStudio.TestTools.UnitTesting;
using DeviceAsset;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeviceAsset.Tests
{
    [TestClass()]
    public class ClientTests
    {
        [TestMethod()]
        public void TransportOrders2Test()
        {

            Client client = new Client();
            //var res = client.VehiclesAllAsync(ProcState.IDLE).Result;
            var time = DateTime.UtcNow.ToShortTimeString();
            client.TransportOrders2Async($"TestOrder_{time}", new TransportOrder()
            {
                Deadline = DateTime.UtcNow.AddHours(8).AddMinutes(10),
                Destinations = new List<DestinationOrder>()
                {
                    new DestinationOrder(){ LocationName="A",Operation="JackLoad",Properties=new List<Property>()},
                    new DestinationOrder(){ LocationName="B",Operation="JackUnload",Properties=new List<Property>()},
                    new DestinationOrder(){ LocationName="C",Operation="JackLoad",Properties=new List<Property>()},
                },
                Properties = new List<Property>(),
                Dependencies = new List<string>()
            }).Wait();


        }
    }
}