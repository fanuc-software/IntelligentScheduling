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
            var waitArr = new string[] { "Wait_B1_D1", "Wait_B2_D2", "Wait_E1_F1", "Wait_E2_F2" ,"Wait_A1","Wait_A2"};
            waitArr.ToList().ForEach(d =>
            {
                WaitNodes.Add(new WaitNode() { Station = d, State = WaitNodeState.Free });
            });
            var listNode = WaitNodes.ToList();
            //ProductNodeDict.TryAdd("RX08_RAWIN", new List<ConfigNode>()
            //{
            //  //  new ConfigNode(){ Station="A",Operation="Wait"},
            //    new ConfigNode()
            //    {
            //        Station ="A1",
            //        Operation ="Wait",
            //        IsRequiredWait =true,
            //        ArrivalNotice=true,
            //        Signal="AGVATPREPICK",
            //        IncludeWaits=new List<WaitNode>(){ listNode[0],listNode[1] }
            //    },
            //    new ConfigNode(){ Station="A",Operation="Wait",ArrivalNotice=true,Signal="AGVATPICK"},
            //    new ConfigNode(){ Station="A1",Operation="Wait",ArrivalNotice=true,Signal="AGVPICKEDANDLEAVE"},
            //    new ConfigNode(){ Station="B",Operation="Wait",ArrivalNotice=true,Signal="FINISHED"}

            //});
            //ProductNodeDict.TryAdd("RX09_RAWIN", new List<ConfigNode>()
            //{
            //   //  new ConfigNode(){ Station="A",Operation="Wait"},
            //    new ConfigNode()
            //    {
            //        Station ="A1_aa",
            //        Operation ="Wait",
            //        IsRequiredWait =true,
            //        ArrivalNotice=true,
            //        Signal="AGVATPREPICK",
            //        IncludeWaits=new List<WaitNode>(){ listNode[2],listNode[3] }
            //    },
            //    new ConfigNode(){ Station="A",Operation="Wait", ArrivalNotice=true,  Signal="AGVATPICK"},
            //    new ConfigNode(){ Station="A1",Operation="Wait", ArrivalNotice=true,Signal="AGVPICKEDANDLEAVE"},
            //    new ConfigNode(){ Station="B",Operation="Wait", ArrivalNotice=true,Signal="FINISHED"}
            //});

            ProductNodeDict.TryAdd("RX0D_IN", new List<ConfigNode>()
            {
              //  new ConfigNode(){ Station="A",Operation="Wait"},
                new ConfigNode()
                {
                    Station ="D",
                    Operation ="JackUnload",
                    IsRequiredWait =true,
                    ArrivalNotice=false,
                    Signal="AGVATPREPICK",
                    IncludeWaits=new List<WaitNode>(){ listNode[0],listNode[1] }
                },


            });
            ProductNodeDict.TryAdd("RX0D_Out", new List<ConfigNode>()
            {
              //  new ConfigNode(){ Station="A",Operation="Wait"},
                new ConfigNode()
                {
                    Station ="D",
                    Operation ="JackUnload",
                    IsRequiredWait =false,
                    ArrivalNotice=false,
                    Signal="AGVATPREPICK",

                },


            });
            ProductNodeDict.TryAdd("RX0B_IN", new List<ConfigNode>()
            {
              //  new ConfigNode(){ Station="A",Operation="Wait"},
                new ConfigNode()
                {
                    Station ="B",
                    Operation ="JackUnload",
                    IsRequiredWait =true,
                    ArrivalNotice=false,
                    Signal="AGVATPREPICK",
                    IncludeWaits=new List<WaitNode>(){ listNode[0],listNode[1] }
                },


            });
            ProductNodeDict.TryAdd("RX0B_Out", new List<ConfigNode>()
            {
              //  new ConfigNode(){ Station="A",Operation="Wait"},
                new ConfigNode()
                {
                    Station ="B",
                    Operation ="JackUnload",
                    IsRequiredWait =false,
                    ArrivalNotice=false,
                    Signal="AGVATPREPICK",
                },


            });
            ProductNodeDict.TryAdd("RX0E_Out", new List<ConfigNode>()
            {
              //  new ConfigNode(){ Station="A",Operation="Wait"},
                new ConfigNode()
                {
                    Station ="E",
                    Operation ="JackUnload",
                    IsRequiredWait =false,
                    ArrivalNotice=false,
                    Signal="AGVATPREPICK",

                },
            });
            ProductNodeDict.TryAdd("RX0F_Out", new List<ConfigNode>()
            {
              //  new ConfigNode(){ Station="A",Operation="Wait"},
                new ConfigNode()
                {
                    Station ="F",
                    Operation ="JackUnload",
                    IsRequiredWait =false,
                    ArrivalNotice=false,
                    Signal="AGVATPREPICK",

                },


            });

            ProductNodeDict.TryAdd("RX0E_IN", new List<ConfigNode>()
            {
              //  new ConfigNode(){ Station="A",Operation="Wait"},
                new ConfigNode()
                {
                    Station ="E",
                    Operation ="JackUnload",
                    IsRequiredWait =true,
                    ArrivalNotice=false,
                    Signal="AGVATPREPICK",
                    IncludeWaits=new List<WaitNode>(){ listNode[2],listNode[3] }

                },
            });
            ProductNodeDict.TryAdd("RX0F_IN", new List<ConfigNode>()
            {
              //  new ConfigNode(){ Station="A",Operation="Wait"},
                new ConfigNode()
                {
                    Station ="F",
                    Operation ="JackUnload",
                    IsRequiredWait =true,
                    ArrivalNotice=false,
                    Signal="AGVATPREPICK",
                    IncludeWaits=new List<WaitNode>(){ listNode[2],listNode[3] }

                },


            });
            ProductNodeDict.TryAdd("RX0A_IN", new List<ConfigNode>()
            {
              //  new ConfigNode(){ Station="A",Operation="Wait"},
                new ConfigNode()
                {
                    Station ="A",
                    Operation ="JackUnload",
                    IsRequiredWait =true,
                    ArrivalNotice=false,
                    Signal="AGVATPREPICK",
                    IncludeWaits=new List<WaitNode>(){ listNode[4],listNode[5] }

                },


            });
            ProductNodeDict.TryAdd("RX0A_Out", new List<ConfigNode>()
            {
              //  new ConfigNode(){ Station="A",Operation="Wait"},
                new ConfigNode()
                {
                    Station ="A",
                    Operation ="JackUnload",
                    IsRequiredWait =false,
                    ArrivalNotice=true,
                    Signal="AGVATPREPICK"

                },


            });

            ProductNodeDict.TryAdd("RX0H_Out", new List<ConfigNode>()
            {
              //  new ConfigNode(){ Station="A",Operation="Wait"},
                new ConfigNode()
                {
                    Station ="H",
                    Operation ="JackUnload",
                    IsRequiredWait =false,
                    ArrivalNotice=true,
                    Signal="AGVATPREPICK"

                },


            });

        }

    }
}