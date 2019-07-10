using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Threading;
using RightCarryService;

namespace WPF.RightCarryUI
{
    public class MessageItem : ViewModelBase
    {
        private RightCarryServiceStateEnum _State;
        public RightCarryServiceStateEnum State
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

    public class MainViewModel : ViewModelBase
    {
        private BackgroundWorker m_static_BackgroundWorker = new BackgroundWorker();

        public AllenBradleyControlDevice carryDevice;

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

        private string _Prod_Type;
        public string Prod_Type
        {
            get { return _Prod_Type; }
            set
            {
                if (_Prod_Type != value)
                {
                    _Prod_Type = value;
                    RaisePropertyChanged(() => Prod_Type);
                }
            }
        }

        private string _Mate_Type;
        public string Mate_Type
        {
            get { return _Mate_Type; }
            set
            {
                if (_Mate_Type != value)
                {
                    _Mate_Type = value;
                    RaisePropertyChanged(() => Mate_Type);
                }
            }
        }

        private bool _In_Out;
        public bool In_Out
        {
            get { return _In_Out; }
            set
            {
                if (_In_Out != value)
                {
                    _In_Out = value;
                    RaisePropertyChanged(() => In_Out);
                }
            }
        }

        private int _Req_Fin;
        public int Req_Fin
        {
            get { return _Req_Fin; }
            set
            {
                if (_Req_Fin != value)
                {
                    _Req_Fin = value;
                    RaisePropertyChanged(() => Req_Fin);
                }
            }
        }


        public ICommand InOut_Command { get; set; }
        public ICommand Req_Command { get; set; }

        public MainViewModel()
        {
            InOut_Command = new RelayCommand(OnInOut_Command);
            Req_Command = new RelayCommand(OnReq_Command);

            carryDevice = new AllenBradleyControlDevice();
        }

        private void OnInOut_Command()
        {
            In_Out = In_Out == false ? true : false;
        }

        private void OnReq_Command()
        {
            Task.Factory.StartNew(() =>
            {
                if (Req_Fin == 1) return;

                Req_Fin = 1;

                TestRightCarryService<AllenBradleyControlDevice> carry = new TestRightCarryService<AllenBradleyControlDevice>(carryDevice);
                carry.SendRightCarryServiceStateMessageEvent += (s) => {

                    DispatcherHelper.CheckBeginInvokeOnUI(() =>
                    {
                        Messages.Add(new MessageItem { State = s.State, Message = s.Message, CreateDateTime = DateTime.Now });
                    });

                };

                if (In_Out == false)
                {
                    var ret = carry.CarryIn(Prod_Type.ToString(), Mate_Type.ToString());
                    Req_Fin = ret == true ? 2 : 3;
                }
                else
                {
                    int qty = 0;
                    var ret = carry.CarryOut(Prod_Type.ToString(), Mate_Type.ToString(), ref qty);
                    Req_Fin = ret == true ? 2 : 3;
                }

                carry.ReleaseLock();
            });
            


        }
    }

}
