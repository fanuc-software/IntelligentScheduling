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
       
            // 3C单元 RX09
            ProductNodeDict.TryAdd("RX09_RAWIN", new List<ConfigNode>()
            {
               
                 new ConfigNode()
                {
                    Station ="Bool_E",
                    Operation ="JackUnload",
                    IsRequiredWait =true,
                    WaitArrivalNotice=true,
                    WaitSinal="AGVATPREPICK",
                    ArrivalNotice=true,
                    Signal="AGVATPICK",
                    IncludeWaits=new List<WaitNode>(){ listNode[2],listNode[3] }

                },
                new ConfigNode()
                {
                    Station ="E",
                    Operation ="JackLoad",
                    IsRequiredWait =false,                  
                    ArrivalNotice=true,
                    Signal="AGVATPICK"

                },
                 new ConfigNode()
                {
                    Station ="Bool_E",
                    Operation ="JackLoad",
                    IsRequiredWait =false,
                    ArrivalNotice=true,
                    Signal="AGVPICKEDANDLEAVE",
                },
                new ConfigNode()
                {
                    Station ="B",
                    Operation ="JackUnload",
                    IsRequiredWait =true,
                    WaitArrivalNotice=true,
                    WaitSinal="AGVATPREPLACE",
                    ArrivalNotice=true,
                    Signal="AGVATPLACE",
                    IncludeWaits=new List<WaitNode>(){ listNode[1],listNode[0] }
                },
                  new ConfigNode()
                {
                    Station ="Bool_B",
                    Operation ="JackUnload",
                    IsRequiredWait =false,
                    ArrivalNotice=true,
                    Signal="FINISHED",
                },


            });           
            ProductNodeDict.TryAdd("RX09_EMPTYOUT", new List<ConfigNode>()
            {

                 new ConfigNode()
                {
                    Station ="Bool_B",
                    Operation ="JackUnload",
                    IsRequiredWait =true,
                    WaitArrivalNotice=true,
                    WaitSinal="AGVATPREPICK",
                    ArrivalNotice=true,
                    Signal="AGVATPICK",
                    IncludeWaits=new List<WaitNode>(){ listNode[1], listNode[0] }

                },
                new ConfigNode()
                {
                    Station ="B",
                    Operation ="JackLoad",
                    IsRequiredWait =false,
                    ArrivalNotice=true,
                    Signal="AGVATPICK"

                },
                 new ConfigNode()
                {
                    Station ="Bool_B",
                    Operation ="JackLoad",
                    IsRequiredWait =false,
                    ArrivalNotice=true,
                    Signal="AGVPICKEDANDLEAVE",
                },
                new ConfigNode()
                {
                    Station ="F",
                    Operation ="JackUnload",
                    IsRequiredWait =true,
                    WaitArrivalNotice=true,
                    WaitSinal="AGVATPREPLACE",
                    ArrivalNotice=true,
                    Signal="AGVATPLACE",
                    IncludeWaits=new List<WaitNode>(){ listNode[2],listNode[3] }
                },
                  new ConfigNode()
                {
                    Station ="Bool_F",
                    Operation ="JackUnload",
                    IsRequiredWait =false,
                    ArrivalNotice=true,
                    Signal="FINISHED",
                }


            });

            ProductNodeDict.TryAdd("RX09_EMPTYIN", new List<ConfigNode>()
            {

                 new ConfigNode()
                {
                    Station ="Bool_E",
                    Operation ="JackUnload",
                    IsRequiredWait =true,
                    WaitArrivalNotice=true,
                    WaitSinal="AGVATPREPICK",
                    ArrivalNotice=true,
                    Signal="AGVATPICK",
                    IncludeWaits=new List<WaitNode>(){ listNode[2],listNode[3] }

                },
                new ConfigNode()
                {
                    Station ="E",
                    Operation ="JackLoad",
                    IsRequiredWait =false,
                    ArrivalNotice=true,
                    Signal="AGVATPICK"

                },
                 new ConfigNode()
                {
                    Station ="Bool_E",
                    Operation ="JackLoad",
                    IsRequiredWait =false,
                    ArrivalNotice=true,
                    Signal="AGVPICKEDANDLEAVE",
                },
                new ConfigNode()
                {
                    Station ="D",
                    Operation ="JackUnload",
                    IsRequiredWait =true,
                    WaitArrivalNotice=true,
                    WaitSinal="AGVATPREPLACE",
                    ArrivalNotice=true,
                    Signal="AGVATPLACE",
                    IncludeWaits=new List<WaitNode>(){ listNode[0],listNode[1] }
                },
                 new ConfigNode()
                {
                    Station ="Bool_D",
                    Operation ="JackUnload",
                    IsRequiredWait =false,
                    ArrivalNotice=true,
                    Signal="FINISHED",
                },

            });
            ProductNodeDict.TryAdd("RX09_PRODUCTOUT", new List<ConfigNode>()
            {
                 new ConfigNode()
                {
                    Station ="Bool_D",
                    Operation ="JackUnload",
                    IsRequiredWait =true,
                    WaitArrivalNotice=true,
                    WaitSinal="AGVATPREPICK",
                    ArrivalNotice=true,
                    Signal="AGVATPICK",
                    IncludeWaits=new List<WaitNode>(){ listNode[0], listNode[1] }

                },
                new ConfigNode()
                {
                    Station ="D",
                    Operation ="JackLoad",
                    IsRequiredWait =false,
                    ArrivalNotice=true,
                    Signal="AGVATPICK"

                },
                 new ConfigNode()
                {
                    Station ="Bool_D",
                    Operation ="JackLoad",
                    IsRequiredWait =false,
                    ArrivalNotice=true,
                    Signal="AGVPICKEDANDLEAVE",
                },
                new ConfigNode()
                {
                    Station ="F",
                    Operation ="JackUnload",
                    IsRequiredWait =true,
                    WaitArrivalNotice=true,
                    WaitSinal="AGVATPREPLACE",
                    ArrivalNotice=true,
                    Signal="AGVATPLACE",
                    IncludeWaits=new List<WaitNode>(){ listNode[2],listNode[3] }
                },
                  new ConfigNode()
                {
                    Station ="Bool_F",
                    Operation ="JackUnload",
                    IsRequiredWait =false,
                    ArrivalNotice=true,
                    Signal="FINISHED",
                },

            });


            //RX07
            ProductNodeDict.TryAdd("RX07_RAWIN", new List<ConfigNode>()
            {
                 new ConfigNode()
                {
                    Station ="Bool_E",
                    Operation ="JackUnload",
                    IsRequiredWait =true,
                    WaitArrivalNotice=true,
                    WaitSinal="AGVATPREPICK",
                    ArrivalNotice=true,
                    Signal="AGVATPICK",
                    IncludeWaits=new List<WaitNode>(){ listNode[2],listNode[3] }

                },
                new ConfigNode()
                {
                    Station ="E",
                    Operation ="JackLoad",
                    IsRequiredWait =false,
                    ArrivalNotice=true,
                    Signal="AGVATPICK"

                },
                 new ConfigNode()
                {
                    Station ="Bool_E",
                    Operation ="JackLoad",
                    IsRequiredWait =false,
                    ArrivalNotice=true,
                    Signal="AGVPICKEDANDLEAVE",
                },
                new ConfigNode()
                {
                    Station ="A",
                    Operation ="JackUnload",
                    IsRequiredWait =true,
                    WaitArrivalNotice=true,
                    WaitSinal="AGVATPREPLACE",
                    ArrivalNotice=true,
                    Signal="AGVATPLACE",
                    IncludeWaits=new List<WaitNode>(){ listNode[4],listNode[5] }
                },
                  new ConfigNode()
                {
                    Station ="Bool_A",
                    Operation ="JackUnload",
                    IsRequiredWait =false,
                    ArrivalNotice=true,
                    Signal="FINISHED",
                }
            });
            ProductNodeDict.TryAdd("RX07_EMPTYOUT", new List<ConfigNode>()
            {
                 new ConfigNode()
                {
                    Station ="Bool_A",
                    Operation ="JackUnload",
                    IsRequiredWait =true,
                    WaitArrivalNotice=true,
                    WaitSinal="AGVATPREPICK",
                    ArrivalNotice=true,
                    Signal="AGVATPICK",
                    IncludeWaits=new List<WaitNode>(){ listNode[4], listNode[5] }

                },
                new ConfigNode()
                {
                    Station ="A",
                    Operation ="JackLoad",
                    IsRequiredWait =false,
                    ArrivalNotice=true,
                    Signal="AGVATPICK"

                },
                 new ConfigNode()
                {
                    Station ="Bool_A",
                    Operation ="JackLoad",
                    IsRequiredWait =false,
                    ArrivalNotice=true,
                    Signal="AGVPICKEDANDLEAVE",
                },
                new ConfigNode()
                {
                    Station ="F",
                    Operation ="JackUnload",
                    IsRequiredWait =true,
                    WaitArrivalNotice=true,
                    WaitSinal="AGVATPREPLACE",
                    ArrivalNotice=true,
                    Signal="AGVATPLACE",
                    IncludeWaits=new List<WaitNode>(){ listNode[2],listNode[3] }
                },
                  new ConfigNode()
                {
                    Station ="Bool_F",
                    Operation ="JackUnload",
                    IsRequiredWait =false,
                    ArrivalNotice=true,
                    Signal="FINISHED",
                }
            });

            //RX08
            ProductNodeDict.TryAdd("RX08_RAWIN", new List<ConfigNode>()
            {
                 new ConfigNode()
                {
                    Station ="Bool_E",
                    Operation ="JackUnload",
                    IsRequiredWait =true,
                    WaitArrivalNotice=true,
                    WaitSinal="AGVATPREPICK",
                    ArrivalNotice=true,
                    Signal="AGVATPICK",
                    IncludeWaits=new List<WaitNode>(){ listNode[2],listNode[3] }

                },
                new ConfigNode()
                {
                    Station ="E",
                    Operation ="JackLoad",
                    IsRequiredWait =false,
                    ArrivalNotice=true,
                    Signal="AGVATPICK"

                },
                 new ConfigNode()
                {
                    Station ="Bool_E",
                    Operation ="JackLoad",
                    IsRequiredWait =false,
                    ArrivalNotice=true,
                    Signal="AGVPICKEDANDLEAVE",
                },
                new ConfigNode()
                {
                    Station ="H",
                    Operation ="JackUnload",
                    IsRequiredWait =true,
                    WaitArrivalNotice=true,
                    WaitSinal="AGVATPREPLACE",
                    ArrivalNotice=true,
                    Signal="AGVATPLACE",
                    IncludeWaits=new List<WaitNode>(){ listNode[6],listNode[7] }
                },
                  new ConfigNode()
                {
                    Station ="Bool_H",
                    Operation ="JackUnload",
                    IsRequiredWait =false,
                    ArrivalNotice=true,
                    Signal="FINISHED",
                },


            });
            ProductNodeDict.TryAdd("RX08_EMPTYOUT", new List<ConfigNode>()
            {
                 new ConfigNode()
                {
                    Station ="Bool_H",
                    Operation ="JackUnload",
                    IsRequiredWait =true,
                    WaitArrivalNotice=true,
                    WaitSinal="AGVATPREPICK",
                    ArrivalNotice=true,
                    Signal="AGVATPICK",
                    IncludeWaits=new List<WaitNode>(){ listNode[6], listNode[7] }

                },
                new ConfigNode()
                {
                    Station ="H",
                    Operation ="JackLoad",
                    IsRequiredWait =false,
                    ArrivalNotice=true,
                    Signal="AGVATPICK"

                },
                 new ConfigNode()
                {
                    Station ="Bool_H",
                    Operation ="JackLoad",
                    IsRequiredWait =false,
                    ArrivalNotice=true,
                    Signal="AGVPICKEDANDLEAVE",
                },
                new ConfigNode()
                {
                    Station ="F",
                    Operation ="JackUnload",
                    IsRequiredWait =true,
                    WaitArrivalNotice=true,
                    WaitSinal="AGVATPREPLACE",
                    ArrivalNotice=true,
                    Signal="AGVATPLACE",
                    IncludeWaits=new List<WaitNode>(){ listNode[2],listNode[3] }
                },
                  new ConfigNode()
                {
                    Station ="Bool_F",
                    Operation ="JackUnload",
                    IsRequiredWait =false,
                    ArrivalNotice=true,
                    Signal="FINISHED",
                },

            });

            ProductNodeDict.TryAdd("RX08_EMPTYIN", new List<ConfigNode>()
            {
                 new ConfigNode()
                {
                    Station ="Bool_E",
                    Operation ="JackUnload",
                    IsRequiredWait =true,
                    WaitArrivalNotice=true,
                    WaitSinal="AGVATPREPICK",
                    ArrivalNotice=true,
                    Signal="AGVATPICK",
                    IncludeWaits=new List<WaitNode>(){ listNode[2],listNode[3] }

                },
                new ConfigNode()
                {
                    Station ="E",
                    Operation ="JackLoad",
                    IsRequiredWait =false,
                    ArrivalNotice=true,
                    Signal="AGVATPICK"

                },
                 new ConfigNode()
                {
                    Station ="Bool_E",
                    Operation ="JackLoad",
                    IsRequiredWait =false,
                    ArrivalNotice=true,
                    Signal="AGVPICKEDANDLEAVE",
                },
                new ConfigNode()
                {
                    Station ="G",
                    Operation ="JackUnload",
                    IsRequiredWait =true,
                    WaitArrivalNotice=true,
                    WaitSinal="AGVATPREPLACE",
                    ArrivalNotice=true,
                    Signal="AGVATPLACE",
                    IncludeWaits=new List<WaitNode>(){ listNode[7],listNode[6] }
                },
                  new ConfigNode()
                {
                    Station ="Bool_G",
                    Operation ="JackUnload",
                    IsRequiredWait =false,
                    ArrivalNotice=true,
                    Signal="FINISHED",
                },


            });
            ProductNodeDict.TryAdd("RX08_PRODUCTOUT", new List<ConfigNode>()
            {
                 new ConfigNode()
                {
                    Station ="Bool_G",
                    Operation ="JackUnload",
                    IsRequiredWait =true,
                    WaitArrivalNotice=true,
                    WaitSinal="AGVATPREPICK",
                    ArrivalNotice=true,
                    Signal="AGVATPICK",
                    IncludeWaits=new List<WaitNode>(){ listNode[7], listNode[6] }

                },
                new ConfigNode()
                {
                    Station ="G",
                    Operation ="JackLoad",
                    IsRequiredWait =false,
                    ArrivalNotice=true,
                    Signal="AGVATPICK"

                },
                 new ConfigNode()
                {
                    Station ="Bool_G",
                    Operation ="JackLoad",
                    IsRequiredWait =false,
                    ArrivalNotice=true,
                    Signal="AGVPICKEDANDLEAVE",
                },
                new ConfigNode()
                {
                    Station ="F",
                    Operation ="JackUnload",
                    IsRequiredWait =true,
                    WaitArrivalNotice=true,
                    WaitSinal="AGVATPREPLACE",
                    ArrivalNotice=true,
                    Signal="AGVATPLACE",
                    IncludeWaits=new List<WaitNode>(){ listNode[2],listNode[3] }
                },
                  new ConfigNode()
                {
                    Station ="Bool_F",
                    Operation ="JackUnload",
                    IsRequiredWait =false,
                    ArrivalNotice=true,
                    Signal="FINISHED",
                },

            });
        }

    }
}