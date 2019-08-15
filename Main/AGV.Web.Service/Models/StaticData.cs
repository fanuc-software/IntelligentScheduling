using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections.Concurrent;
namespace AGV.Web.Service.Models
{
    public class StaticData
    {
        public static BlockingCollection<WaitNode> WaitNodes = new BlockingCollection<WaitNode>();


        public static ConcurrentDictionary<string, bool> SignalDict = new ConcurrentDictionary<string, bool>();


        public static ConcurrentDictionary<string, List<ConfigNode>> ProductNodeDict = new ConcurrentDictionary<string, List<ConfigNode>>();

        static StaticData()
        {

            var waitArr = new string[] { "Wait_B1_D1", "Wait_B2_D2", "Wait_E1_F1", "Wait_E2_F2", "Wait_A1", "Wait_A2" , "Wait_H2", "Wait_G2", "Wait_G1_H1" };
            waitArr.ToList().ForEach(d =>
            {
                WaitNodes.Add(new WaitNode() { Station = d, State = WaitNodeState.Free });
            });
            var listNode = WaitNodes.ToList();


            ProductNodeDict.TryAdd("RX0D_IN", new List<ConfigNode>()
            {
                new ConfigNode()
                {
                    Station ="Bool_D",
                    Operation ="JackUnload",
                    IsRequiredWait =false,
                    ArrivalNotice=false,
                    Signal="AGVATPREPICK"
                },
                new ConfigNode()
                {
                    Station ="D",
                    Operation ="JackUnload",
                    IsRequiredWait =true,
                    ArrivalNotice=false,
                    Signal="AGVATPREPICK",
                    IncludeWaits=new List<WaitNode>(){ listNode[1],listNode[0] }
                },
                new ConfigNode()
                {
                    Station ="Bool_D",
                    Operation ="JackUnload",
                    IsRequiredWait =false,
                    ArrivalNotice=true,
                    Signal="AGVATPREPICK"
                },

            });
            ProductNodeDict.TryAdd("RX0D_Out", new List<ConfigNode>()
            {
                 new ConfigNode()
                {
                    Station ="Bool_D",
                    Operation ="JackUnload",
                    IsRequiredWait =false,
                    ArrivalNotice=false,
                    Signal="AGVATPREPICK"
                },
                new ConfigNode()
                {
                    Station ="D",
                    Operation ="JackUnload",
                    IsRequiredWait =false,
                    ArrivalNotice=false,
                    Signal="AGVATPREPICK",

                },
                 new ConfigNode()
                {
                    Station ="Bool_D",
                    Operation ="JackUnload",
                    IsRequiredWait =false,
                    ArrivalNotice=true,
                    Signal="AGVATPREPICK"
                },

            });

            ProductNodeDict.TryAdd("RX08_RAWIN", new List<ConfigNode>()
            {
                new ConfigNode()
                {
                    Station ="Bool_E",
                    Operation ="JackUnload",
                    IsRequiredWait =false,
                    ArrivalNotice=false,
                    Signal="AGVATPREPICK",
                },
                new ConfigNode()
                {
                    Station ="E",
                    Operation ="JackLoad",
                    IsRequiredWait =true,
                    ArrivalNotice=false,
                    Signal="AGVATPREPICK",
                    IncludeWaits=new List<WaitNode>(){ listNode[2],listNode[3] }

                },               
                new ConfigNode()
                {
                    Station ="B",
                    Operation ="JackUnload",
                    IsRequiredWait =true,
                    ArrivalNotice=false,
                    Signal="AGVATPREPICK",
                    IncludeWaits=new List<WaitNode>(){ listNode[1],listNode[0] }
                }
                

            });
            ProductNodeDict.TryAdd("RX0B_Out", new List<ConfigNode>()
            {
              new ConfigNode()
                {
                    Station ="Bool_B",
                    Operation ="JackUnload",
                    IsRequiredWait =false,
                    ArrivalNotice=false,
                    Signal="AGVATPREPICK"
                },
                new ConfigNode()
                {
                    Station ="B",
                    Operation ="JackUnload",
                    IsRequiredWait =false,
                    ArrivalNotice=false,
                    Signal="AGVATPREPICK",
                },
                  new ConfigNode()
                {
                    Station ="Bool_B",
                    Operation ="JackUnload",
                    IsRequiredWait =false,
                    ArrivalNotice=true,
                    Signal="AGVATPREPICK"
                },

            });

            ProductNodeDict.TryAdd("RX0E_IN", new List<ConfigNode>()
            {

                new ConfigNode()
                {
                    Station ="Bool_E",
                    Operation ="JackUnload",
                    IsRequiredWait =false,
                    ArrivalNotice=false,
                    Signal="AGVATPREPICK",

                },
                new ConfigNode()
                {
                    Station ="E",
                    Operation ="JackLoad",
                    IsRequiredWait =true,
                    ArrivalNotice=false,
                    Signal="AGVATPREPICK",
                    IncludeWaits=new List<WaitNode>(){ listNode[2],listNode[3] }

                },
                 new ConfigNode()
                {
                    Station ="Bool_E",
                    Operation ="JackLoad",
                    IsRequiredWait =false,
                    ArrivalNotice=true,
                    Signal="AGVATPREPICK",

                },
            });
            ProductNodeDict.TryAdd("RX0E_Out", new List<ConfigNode>()
            {
              //  new ConfigNode(){ Station="A",Operation="Wait"},
               new ConfigNode()
                {
                    Station ="Bool_E",
                    Operation ="JackUnload",
                    IsRequiredWait =false,
                    ArrivalNotice=false,
                    Signal="AGVATPREPICK",

                },
                new ConfigNode()
                {
                    Station ="E",
                    Operation ="JackLoad",
                    IsRequiredWait =false,
                    ArrivalNotice=false,
                    Signal="AGVATPREPICK",

                },
                 new ConfigNode()
                {
                    Station ="Bool_E",
                    Operation ="JackUnload",
                    IsRequiredWait =false,
                    ArrivalNotice=true,
                    Signal="AGVATPREPICK",

                },
            });



            ProductNodeDict.TryAdd("RX0F_IN", new List<ConfigNode>()
            {
                new ConfigNode()
                {
                    Station ="Bool_F",
                    Operation ="JackUnload",
                    IsRequiredWait =false,
                    ArrivalNotice=false,
                    Signal="AGVATPREPICK",

                },
                new ConfigNode()
                {
                    Station ="F",
                    Operation ="JackLoad",
                    IsRequiredWait =true,
                    ArrivalNotice=false,
                    Signal="AGVATPREPICK",
                    IncludeWaits=new List<WaitNode>(){ listNode[2],listNode[3] }

                },
                 new ConfigNode()
                {
                    Station ="Bool_F",
                    Operation ="JackLoad",
                    IsRequiredWait =false,
                    ArrivalNotice=true,
                    Signal="AGVATPREPICK",

                },

            });
            ProductNodeDict.TryAdd("RX0F_Out", new List<ConfigNode>()
            {
              //  new ConfigNode(){ Station="A",Operation="Wait"},
               new ConfigNode()
                {
                    Station ="Bool_F",
                    Operation ="JackUnload",
                    IsRequiredWait =false,
                    ArrivalNotice=false,
                    Signal="AGVATPREPICK",

                },
                new ConfigNode()
                {
                    Station ="F",
                    Operation ="JackLoad",
                    IsRequiredWait =false,
                    ArrivalNotice=false,
                    Signal="AGVATPREPICK",

                },
                new ConfigNode()
                {
                    Station ="Bool_F",
                    Operation ="JackLoad",
                    IsRequiredWait =false,
                    ArrivalNotice=true,
                    Signal="AGVATPREPICK",

                },

            });

            ProductNodeDict.TryAdd("RX0A_IN", new List<ConfigNode>()
            {
              //  new ConfigNode(){ Station="A",Operation="Wait"},
              new ConfigNode()
                {
                    Station ="Bool_A",
                    Operation ="JackUnload",
                    IsRequiredWait =false,
                    ArrivalNotice=false,
                    Signal="AGVATPREPICK",

                },
                new ConfigNode()
                {
                    Station ="A",
                    Operation ="JackLoad",
                    IsRequiredWait =true,
                    ArrivalNotice=false,
                    Signal="AGVATPREPICK",
                    IncludeWaits=new List<WaitNode>(){ listNode[4],listNode[5] }

                },
                  new ConfigNode()
                {
                    Station ="Bool_A",
                    Operation ="JackLoad",
                    IsRequiredWait =false,
                    ArrivalNotice=true,
                    Signal="AGVATPREPICK",

                },

            });
            ProductNodeDict.TryAdd("RX0A_Out", new List<ConfigNode>()
            {
              //  new ConfigNode(){ Station="A",Operation="Wait"},
               new ConfigNode()
                {
                    Station ="Bool_A",
                    Operation ="JackUnload",
                    IsRequiredWait =false,
                    ArrivalNotice=false,
                    Signal="AGVATPREPICK",

                },
                new ConfigNode()
                {
                    Station ="A",
                    Operation ="JackLoad",
                    IsRequiredWait =false,
                    ArrivalNotice=false,
                    Signal="AGVATPREPICK"

                },
                 new ConfigNode()
                {
                    Station ="Bool_A",
                    Operation ="JackLoad",
                    IsRequiredWait =false,
                    ArrivalNotice=true,
                    Signal="AGVATPREPICK",

                },

            });


            ProductNodeDict.TryAdd("RX0H_IN", new List<ConfigNode>()
            {
              //  new ConfigNode(){ Station="A",Operation="Wait"},
              new ConfigNode()
                {
                    Station ="Bool_H",
                    Operation ="JackUnload",
                    IsRequiredWait =false,
                    ArrivalNotice=false,
                    Signal="AGVATPREPICK",

                },
                new ConfigNode()
                {
                    Station ="H",
                    Operation ="JackLoad",
                    IsRequiredWait =true,
                    ArrivalNotice=false,
                    Signal="AGVATPREPICK",
                    IncludeWaits=new List<WaitNode>(){ listNode[6],listNode[7],listNode[8] }

                },
                  new ConfigNode()
                {
                    Station ="Bool_H",
                    Operation ="JackLoad",
                    IsRequiredWait =false,
                    ArrivalNotice=true,
                    Signal="AGVATPREPICK",

                },

            });
            ProductNodeDict.TryAdd("RX0H_Out", new List<ConfigNode>()
            {
               new ConfigNode()
                {
                    Station ="Bool_H",
                    Operation ="JackUnload",
                    IsRequiredWait =false,
                    ArrivalNotice=false,
                    Signal="AGVATPREPICK",

                },
                new ConfigNode()
                {
                    Station ="H",
                    Operation ="JackLoad",
                    IsRequiredWait =false,
                    ArrivalNotice=false,
                    Signal="AGVATPREPICK"

                },
                   new ConfigNode()
                {
                    Station ="Bool_H",
                    Operation ="JackLoad",
                    IsRequiredWait =false,
                    ArrivalNotice=true,
                    Signal="AGVATPREPICK",

                },

            });

            ProductNodeDict.TryAdd("RX0G_IN", new List<ConfigNode>()
            {
              //  new ConfigNode(){ Station="A",Operation="Wait"},
              new ConfigNode()
                {
                    Station ="Bool_G",
                    Operation ="JackUnload",
                    IsRequiredWait =false,
                    ArrivalNotice=false,
                    Signal="AGVATPREPICK",

                },
                new ConfigNode()
                {
                    Station ="G",
                    Operation ="JackLoad",
                    IsRequiredWait =true,
                    ArrivalNotice=false,
                    Signal="AGVATPREPICK",
                    IncludeWaits=new List<WaitNode>(){ listNode[6],listNode[7],listNode[8] }

                },
                  new ConfigNode()
                {
                    Station ="Bool_G",
                    Operation ="JackLoad",
                    IsRequiredWait =false,
                    ArrivalNotice=true,
                    Signal="AGVATPREPICK",

                },

            });
            ProductNodeDict.TryAdd("RX0G_Out", new List<ConfigNode>()
            {
               new ConfigNode()
                {
                    Station ="Bool_G",
                    Operation ="JackUnload",
                    IsRequiredWait =false,
                    ArrivalNotice=false,
                    Signal="AGVATPREPICK",

                },
                new ConfigNode()
                {
                    Station ="G",
                    Operation ="JackLoad",
                    IsRequiredWait =false,
                    ArrivalNotice=false,
                    Signal="AGVATPREPICK"

                },
                   new ConfigNode()
                {
                    Station ="Bool_G",
                    Operation ="JackLoad",
                    IsRequiredWait =false,
                    ArrivalNotice=true,
                    Signal="AGVATPREPICK",

                },

            });
        }

    }
}