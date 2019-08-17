using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Agv.Common;
using AgvStationClient;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Threading;

namespace WPF.MonitorUI
{
    public class TestStationCient<T> : BaseStationClient<T> where T:IStationDevice
    {


        public TestStationCient(AgvStationEnum id, T testStationDevice) : base(id, testStationDevice)
        {

        }

    }

    public class MessageItem : ViewModelBase
    {
        private StationClientStateEnum _State;
        public StationClientStateEnum State
        {
            get { return _State; }
            set
            {
                if (_State != value)
                {
                    _State = value;
                    RaisePropertyChanged(() => State);
                }
            }
        }

        private string _Message;
        public string Message
        {
            get { return _Message; }
            set
            {
                if (_Message != value)
                {
                    _Message = value;
                    RaisePropertyChanged(() => Message);
                }
            }
        }

        private DateTime _CreateDateTime;
        public DateTime CreateDateTime
        {
            get { return _CreateDateTime; }
            set
            {
                if (_CreateDateTime != value)
                {
                    _CreateDateTime = value;
                    RaisePropertyChanged(() => CreateDateTime);
                }
            }
        }
    }

    public class StationIdItem : ViewModelBase
    {
        private AgvStationEnum _Station_Id;
        public AgvStationEnum Station_Id
        {
            get { return _Station_Id; }
            set
            {
                if (_Station_Id != value)
                {
                    _Station_Id = value;
                    RaisePropertyChanged(() => Station_Id);
                }
            }
        }

        private string _Station_Name;
        public string Station_Name
        {
            get { return _Station_Name; }
            set
            {
                if (_Station_Name != value)
                {
                    _Station_Name = value;
                    RaisePropertyChanged(() => Station_Name);
                }
            }
        }
    }

    public class MainViewModel : ViewModelBase
    {

        private BackgroundWorker m_static_BackgroundWorker = new BackgroundWorker();

        private ObservableCollection<StationIdItem> _Stations = new ObservableCollection<StationIdItem>();
        public ObservableCollection<StationIdItem> Stations
        {
            get { return _Stations; }
            set
            {
                if (_Stations != value)
                {
                    _Stations = value;
                    RaisePropertyChanged(() => Stations);
                }
            }
        }

        private StationIdItem _SelStation;
        public StationIdItem SelStation
        {
            get { return _SelStation; }
            set
            {
                if (_SelStation != value)
                {
                    _SelStation = value;
                    _Client.Station_Id = SelStation.Station_Id;
                    RaisePropertyChanged(() => SelStation);
                }
            }
        }

        private TestStationCient<TestStationDevice> _Client;

        private ObservableCollection<MessageItem> _Messages = new ObservableCollection<MessageItem>();
        public ObservableCollection<MessageItem> Messages
        {
            get { return _Messages; }
            set
            {
                if (_Messages != value)
                {
                    _Messages = value;
                    RaisePropertyChanged(() => Messages);
                }
            }
        }

        #region 属性
        private string _RawIn_Prod;
        public string RawIn_Prod
        {
            get { return _RawIn_Prod; }
            set
            {
                if (_RawIn_Prod != value)
                {
                    _RawIn_Prod = value;
                    (_Client.StationDevice as TestStationDevice).RawIn_Prod = value;
                    RaisePropertyChanged(() => RawIn_Prod);
                }
            }
        }

        private string _RawIn_Mate;
        public string RawIn_Mate
        {
            get { return _RawIn_Mate; }
            set
            {
                if (_RawIn_Mate != value)
                {
                    _RawIn_Mate = value;
                    (_Client.StationDevice as TestStationDevice).RawIn_Mate = value;
                    RaisePropertyChanged(() => RawIn_Mate);
                }
            }
        }

        private bool _RawIn_Req;
        public bool RawIn_Req
        {
            get { return _RawIn_Req; }
            set
            {
                if (_RawIn_Req != value)
                {
                    _RawIn_Req = value;
                    (_Client.StationDevice as TestStationDevice).RawIn_Req = value;
                    RaisePropertyChanged(() => RawIn_Req);
                }
            }
        }

        private bool _RawIn_Fin;
        public bool RawIn_Fin
        {
            get { return _RawIn_Fin; }
            set
            {
                if (_RawIn_Fin != value)
                {
                    _RawIn_Fin = value;
                    RaisePropertyChanged(() => RawIn_Fin);
                }
            }
        }

        private string _EmptyOut_Prod;
        public string EmptyOut_Prod
        {
            get { return _EmptyOut_Prod; }
            set
            {
                if (_EmptyOut_Prod != value)
                {
                    _EmptyOut_Prod = value;
                    (_Client.StationDevice as TestStationDevice).EmptyOut_Prod = value;
                    RaisePropertyChanged(() => EmptyOut_Prod);
                }
            }
        }

        private string _EmptyOut_Mate;
        public string EmptyOut_Mate
        {
            get { return _EmptyOut_Mate; }
            set
            {
                if (_EmptyOut_Mate != value)
                {
                    _EmptyOut_Mate = value;
                    (_Client.StationDevice as TestStationDevice).EmptyOut_Mate = value;
                    RaisePropertyChanged(() => EmptyOut_Mate);
                }
            }
        }

        private bool _EmptyOut_Req;
        public bool EmptyOut_Req
        {
            get { return _EmptyOut_Req; }
            set
            {
                if (_EmptyOut_Req != value)
                {
                    _EmptyOut_Req = value;
                    (_Client.StationDevice as TestStationDevice).EmptyOut_Req = value;
                    RaisePropertyChanged(() => EmptyOut_Req);
                }
            }
        }

        private bool _EmptyOut_Fin;
        public bool EmptyOut_Fin
        {
            get { return _EmptyOut_Fin; }
            set
            {
                if (_EmptyOut_Fin != value)
                {
                    _EmptyOut_Fin = value;
                    RaisePropertyChanged(() => EmptyOut_Fin);
                }
            }
        }

        private string _EmptyIn_Prod;
        public string EmptyIn_Prod
        {
            get { return _EmptyIn_Prod; }
            set
            {
                if (_EmptyIn_Prod != value)
                {
                    _EmptyIn_Prod = value;
                    (_Client.StationDevice as TestStationDevice).EmptyIn_Prod = value;
                    RaisePropertyChanged(() => EmptyIn_Prod);
                }
            }
        }

        private string _EmptyIn_Mate;
        public string EmptyIn_Mate
        {
            get { return _EmptyIn_Mate; }
            set
            {
                if (_EmptyIn_Mate != value)
                {
                    _EmptyIn_Mate = value;
                    (_Client.StationDevice as TestStationDevice).EmptyIn_Mate = value;
                    RaisePropertyChanged(() => EmptyIn_Mate);
                }
            }
        }

        private bool _EmptyIn_Req;
        public bool EmptyIn_Req
        {
            get { return _EmptyIn_Req; }
            set
            {
                if (_EmptyIn_Req != value)
                {
                    _EmptyIn_Req = value;
                    (_Client.StationDevice as TestStationDevice).EmptyIn_Req = value;
                    RaisePropertyChanged(() => EmptyIn_Req);
                }
            }
        }

        private bool _EmptyIn_Fin;
        public bool EmptyIn_Fin
        {
            get { return _EmptyIn_Fin; }
            set
            {
                if (_EmptyIn_Fin != value)
                {
                    _EmptyIn_Fin = value;
                    RaisePropertyChanged(() => EmptyIn_Fin);
                }
            }
        }

        private string _FinOut_Prod;
        public string FinOut_Prod
        {
            get { return _FinOut_Prod; }
            set
            {
                if (_FinOut_Prod != value)
                {
                    _FinOut_Prod = value;
                    (_Client.StationDevice as TestStationDevice).FinOut_Prod = value;
                    RaisePropertyChanged(() => FinOut_Prod);
                }
            }
        }

        private string _FinOut_Mate;
        public string FinOut_Mate
        {
            get { return _FinOut_Mate; }
            set
            {
                if (_FinOut_Mate != value)
                {
                    _FinOut_Mate = value;
                    (_Client.StationDevice as TestStationDevice).FinOut_Mate = value;
                    RaisePropertyChanged(() => FinOut_Mate);
                }
            }
        }

        private bool _FinOut_Req;
        public bool FinOut_Req
        {
            get { return _FinOut_Req; }
            set
            {
                if (_FinOut_Req != value)
                {
                    _FinOut_Req = value;
                    (_Client.StationDevice as TestStationDevice).FinOut_Req = value;
                    RaisePropertyChanged(() => FinOut_Req);
                }
            }
        }

        private bool _FinOut_Fin;
        public bool FinOut_Fin
        {
            get { return _FinOut_Fin; }
            set
            {
                if (_FinOut_Fin != value)
                {
                    _FinOut_Fin = value;

                    RaisePropertyChanged(() => FinOut_Fin);
                }
            }
        }

        private bool _InFeedingSignal;
        public bool InFeedingSignal
        {
            get { return _InFeedingSignal; }
            set
            {
                if (_InFeedingSignal != value)
                {
                    _InFeedingSignal = value;
                    (_Client.StationDevice).InFeedingSignal = value;
                    RaisePropertyChanged(() => InFeedingSignal);
                }
            }
        }

        private bool _OutFeedingSignal;
        public bool OutFeedingSignal
        {
            get { return _OutFeedingSignal; }
            set
            {
                if (_OutFeedingSignal != value)
                {
                    _OutFeedingSignal = value;
                    (_Client.StationDevice as TestStationDevice).OutFeedingSignal = value;
                    RaisePropertyChanged(() => OutFeedingSignal);
                }
            }
        }

        private bool _Alarm;
        public bool Alarm
        {
            get { return _Alarm; }
            set
            {
                if (_Alarm != value)
                {
                    _Alarm = value;
                    RaisePropertyChanged(() => Alarm);
                }
            }
        }

        private bool _Reset_Req;
        public bool Reset_Req
        {
            get { return _Reset_Req; }
            set
            {
                if (_Reset_Req != value)
                {
                    _Reset_Req = value;
                    (_Client.StationDevice as TestStationDevice).Reset = value;
                    RaisePropertyChanged(() => Reset_Req);
                }
            }
        }


        #endregion

        public ICommand RawIn_Command { get; set; }
        public ICommand EmptyOut_Command { get; set; }
        public ICommand EmptyIn_Command { get; set; }
        public ICommand FinOut_Command { get; set; }
        public ICommand Reset_Command { get; set; }
        public ICommand Start_Command { get; set; }

        public MainViewModel()
        {
            RawIn_Command = new RelayCommand(OnRawIn_Command);
            EmptyOut_Command = new RelayCommand(OnEmptyOut_Command);
            EmptyIn_Command = new RelayCommand(OnEmptyIn_Command);
            FinOut_Command = new RelayCommand(OnFinOut_Command);
            Reset_Command = new RelayCommand(OnReset_Command);
            Start_Command = new RelayCommand(OnStart_Command);

            Stations.Add(new StationIdItem { Station_Id = AgvStationEnum.RX07, Station_Name = "工业实训单元" });
            Stations.Add(new StationIdItem { Station_Id = AgvStationEnum.RX08, Station_Name = "汽车刹车盘单元" });
            Stations.Add(new StationIdItem { Station_Id = AgvStationEnum.RX09, Station_Name = "3C加工单元单元" });


            //测试用
            //TestStationDevice 替换成 AgvStationClient\AllenBradleyClientDevice(RX07) 或者AgvStationClient\FanucRobotClientDevice（RX08/RX09）
            _Client = new TestStationCient<TestStationDevice>(AgvStationEnum.RX08,new TestStationDevice());
            _Client.SendStationClientStateMessageEvent += (s) => {

                DispatcherHelper.CheckBeginInvokeOnUI(() =>
                {
                    Messages.Add(new MessageItem { State = s.State, Message = s.Message, CreateDateTime = DateTime.Now });
                });

            };

            m_static_BackgroundWorker.WorkerReportsProgress = false;
            m_static_BackgroundWorker.WorkerSupportsCancellation = true;
            m_static_BackgroundWorker.DoWork += ScannerStaticFunc;
            m_static_BackgroundWorker.RunWorkerAsync();
        }
        private void ScannerStaticFunc(object sender, DoWorkEventArgs e)
        {
            while (m_static_BackgroundWorker.CancellationPending == false)
            {
                DispatcherHelper.CheckBeginInvokeOnUI(() =>
                {
                    RawIn_Fin = _Client.StationDevice.RawIn_Fin;
                    EmptyIn_Fin = _Client.StationDevice.EmptyIn_Fin;
                    EmptyOut_Fin = _Client.StationDevice.EmptyOut_Fin;
                    FinOut_Fin = _Client.StationDevice.FinOut_Fin;
                    Alarm = _Client.StationDevice.Alarm;
                });

                System.Threading.Thread.Sleep(1000);
            }
        }

        private void OnStart_Command()
        {
            _Client.Start();
        }

        private void OnRawIn_Command()
        {
            RawIn_Req = true;
        }

        private void OnEmptyOut_Command()
        {
            EmptyOut_Req = true;
        }


        private void OnEmptyIn_Command()
        {
            EmptyIn_Req = true;
        }


        private void OnFinOut_Command()
        {
            FinOut_Req = true;
        }

        private void OnReset_Command()
        {
            Reset_Req = true;
        }
    }
}
