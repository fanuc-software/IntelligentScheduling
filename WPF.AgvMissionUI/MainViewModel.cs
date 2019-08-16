using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Agv.Common;
using AgvMissionManager;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Threading;

namespace WPF.AgvMissionUI
{

    public class MessageItem : ViewModelBase
    {
        private AgvMissionServiceStateEnum _State;
        public AgvMissionServiceStateEnum State
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

        private AgvMissionServiceErrorCodeEnum _ErrorCode;
        public AgvMissionServiceErrorCodeEnum ErrorCode
        {
            get { return _ErrorCode; }
            set
            {
                if (_ErrorCode != value)
                {
                    _ErrorCode = value;
                    RaisePropertyChanged(() => ErrorCode);
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

    public class InMissionItem : ViewModelBase
    {
        private string _Id;
        public string Id
        {
            get { return _Id; }
            set
            {
                if (_Id != value)
                {
                    _Id = value;
                    RaisePropertyChanged(() => _Id);
                }
            }
        }

        private string _TimeId;
        public string TimeId
        {
            get { return _TimeId; }
            set
            {
                if (_TimeId != value)
                {
                    _TimeId = value;
                    RaisePropertyChanged(() => TimeId);
                }
            }
        }

        private AgvStationEnum _ClientId;
        public AgvStationEnum ClientId
        {
            get { return _ClientId; }
            set
            {
                if (_ClientId != value)
                {
                    _ClientId = value;
                    RaisePropertyChanged(() => ClientId);
                }
            }
        }

        private AgvMissionTypeEnum _Type;
        public AgvMissionTypeEnum Type
        {
            get { return _Type; }
            set
            {
                if (_Type != value)
                {
                    _Type = value;
                    RaisePropertyChanged(() => Type);
                }
            }
        }

        private AgvInMissonProcessEnum _Process;
        public AgvInMissonProcessEnum Process
        {
            get { return _Process; }
            set
            {
                if (_Process != value)
                {
                    _Process = value;
                    RaisePropertyChanged(() => Process);
                }
            }
        }

        private CarryInMissonProcessEnum _CarryProcess;
        public CarryInMissonProcessEnum CarryProcess
        {
            get { return _CarryProcess; }
            set
            {
                if (_CarryProcess != value)
                {
                    _CarryProcess = value;
                    RaisePropertyChanged(() => CarryProcess);
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

    public class OutMissionItem : ViewModelBase
    {
        private string _Id;
        public string Id
        {
            get { return _Id; }
            set
            {
                if (_Id != value)
                {
                    _Id = value;
                    RaisePropertyChanged(() => _Id);
                }
            }
        }

        private AgvStationEnum _ClientId;
        public AgvStationEnum ClientId
        {
            get { return _ClientId; }
            set
            {
                if (_ClientId != value)
                {
                    _ClientId = value;
                    RaisePropertyChanged(() => ClientId);
                }
            }
        }

        private AgvMissionTypeEnum _Type;
        public AgvMissionTypeEnum Type
        {
            get { return _Type; }
            set
            {
                if (_Type != value)
                {
                    _Type = value;
                    RaisePropertyChanged(() => Type);
                }
            }
        }

        private AgvOutMissonProcessEnum _Process;
        public AgvOutMissonProcessEnum Process
        {
            get { return _Process; }
            set
            {
                if (_Process != value)
                {
                    _Process = value;
                    RaisePropertyChanged(() => Process);
                }
            }
        }

        private CarryOutMissonProcessEnum _CarryProcess;
        public CarryOutMissonProcessEnum CarryProcess
        {
            get { return _CarryProcess; }
            set
            {
                if (_CarryProcess != value)
                {
                    _CarryProcess = value;
                    RaisePropertyChanged(() => CarryProcess);
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

    public class MainViewModel : ViewModelBase
    {
        #region 属性
        private bool _InOut;
        public bool InOut
        {
            get { return _InOut; }
            set
            {
                if (_InOut != value)
                {
                    _InOut = value;
                    RaisePropertyChanged(() => InOut);
                }
            }
        }



        private int _ProdType;
        public int ProdType
        {
            get { return _ProdType; }
            set
            {
                if (_ProdType != value)
                {
                    _ProdType = value;
                    RaisePropertyChanged(() => ProdType);
                }
            }
        }



        private int _MateType;
        public int MateType
        {
            get { return _MateType; }
            set
            {
                if (_MateType != value)
                {
                    _MateType = value;
                    RaisePropertyChanged(() => MateType);
                }
            }
        }



        private bool _Req;
        public bool Req
        {
            get { return _Req; }
            set
            {
                if (_Req != value)
                {
                    _Req = value;
                    RaisePropertyChanged(() => Req);
                }
            }
        }

        public ICommand Fin_Command { get; set; }


        #endregion

        private BackgroundWorker m_static_BackgroundWorker = new BackgroundWorker();

        BaseAgvMissionService agvMissionSrv = new BaseAgvMissionService();

        private ObservableCollection<InMissionItem> _InMissions = new ObservableCollection<InMissionItem>();
        public ObservableCollection<InMissionItem> InMissions
        {
            get { return _InMissions; }
            set
            {
                if (_InMissions != value)
                {
                    _InMissions = value;
                    //RaisePropertyChanged(() => InMissions);
                }
            }
        }

        private ObservableCollection<OutMissionItem> _OutMissions = new ObservableCollection<OutMissionItem>();
        public ObservableCollection<OutMissionItem> OutMissions
        {
            get { return _OutMissions; }
            set
            {
                if (_OutMissions != value)
                {
                    _OutMissions = value;
                    //RaisePropertyChanged(() => OutMissions);
                }
            }
        }

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

        public event Action<ObservableCollection<InMissionItem>, InMissionItem> InMissionItemAddEvent;

        public event Action<ObservableCollection<OutMissionItem>, OutMissionItem> OutMissionItemAddEvent;

        public ICommand Start_Command { get; set; }

        public MainViewModel()
        {
            Start_Command = new RelayCommand(OnStart_Command);

            Fin_Command = new RelayCommand(OnFin_Command);

            agvMissionSrv.AgvInMissChangeEvent += AgvMissionSrv_AgvInMissChangeEvent;
            agvMissionSrv.AgvOutMissChangeEvent += AgvMissionSrv_AgvOutMissChangeEvent;

            agvMissionSrv.SendAgvMissionServiceStateMessageEvent += (s) => {

                DispatcherHelper.CheckBeginInvokeOnUI(() =>
                {
                    Messages.Add(new MessageItem { State = s.State, Message = s.Message, ErrorCode=s.ErrorCode, CreateDateTime = DateTime.Now });
                });

            };

            m_static_BackgroundWorker.WorkerReportsProgress = false;
            m_static_BackgroundWorker.WorkerSupportsCancellation = true;
            m_static_BackgroundWorker.DoWork += ScannerStaticFunc;
            m_static_BackgroundWorker.RunWorkerAsync();
        }

        private void OnFin_Command()
        {
            Task.Factory.StartNew(()=>{

                //TEST:CARRY DEVICE
                agvMissionSrv.carryDevice.Fin = true;

                System.Threading.Thread.Sleep(1000);

                agvMissionSrv.carryDevice.Fin = false;
            });
        }

        private void AgvMissionSrv_AgvOutMissChangeEvent(AgvOutMisson mission, bool arg2)
        {
            if (arg2)
            {
                OutMissionItemAddEvent?.Invoke(OutMissions, new OutMissionItem
                {
                    Id = mission.Id,
                    ClientId = mission.ClientId,
                    Type = mission.Type,
                    Process = mission.Process,
                    CarryProcess=mission.CarryProcess,
                    CreateDateTime = mission.CreateDateTime,
                });
                return;
            }

            var item = OutMissions.Where(x => x.Id == mission.Id && x.Process != AgvOutMissonProcessEnum.CLOSE).FirstOrDefault();
            if (item != null)
            {
                item.Process = mission.Process;
                item.CarryProcess = mission.CarryProcess;
                item.CreateDateTime = mission.CreateDateTime;
            }
        }

        private void AgvMissionSrv_AgvInMissChangeEvent(AgvInMisson mission, bool arg2)
        {
            if (arg2)
            {
                InMissionItemAddEvent?.Invoke(InMissions, new InMissionItem
                {
                    Id = mission.Id,
                    ClientId = mission.ClientId,
                    Type = mission.Type,
                    Process = mission.Process,
                    CarryProcess = mission.CarryProcess,
                    CreateDateTime = mission.CreateDateTime,
                });
                return;
            }

            var item = InMissions.Where(x => x.Id == mission.Id && x.Process != AgvInMissonProcessEnum.CLOSE).FirstOrDefault();
            if (item != null)
            {
                item.Process = mission.Process;
                item.CarryProcess = mission.CarryProcess;
                item.CreateDateTime = mission.CreateDateTime;
            }
        }

        private void OnStart_Command()
        {
            agvMissionSrv.Start(true);
        }

        private void ScannerStaticFunc(object sender, DoWorkEventArgs e)
        {
            while (m_static_BackgroundWorker.CancellationPending == false)
            {
                DispatcherHelper.CheckBeginInvokeOnUI(() =>
                {
                    //TEST:CARRY DEVICE
                    ProdType = agvMissionSrv.carryDevice.Product_Type;
                    MateType = agvMissionSrv.carryDevice.Material_Type;
                    InOut = agvMissionSrv.carryDevice.InOut;
                    Req = agvMissionSrv.carryDevice.Req;
                });

                System.Threading.Thread.Sleep(1000);
            }
        }
    }
}
