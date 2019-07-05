using GalaSoft.MvvmLight.Threading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WPF.AgvMissionUI
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            DispatcherHelper.Initialize();

            InitializeComponent();
            var main = new MainViewModel();
            main.OutMissionItemAddEvent += Main_OutMissionItemAddEvent;
            main.InMissionItemAddEvent += Main_InMissionItemAddEvent;
            this.DataContext = main;
        }

        private void Main_InMissionItemAddEvent(System.Collections.ObjectModel.ObservableCollection<InMissionItem> arg1, InMissionItem arg2)
        {
            Dispatcher.BeginInvoke(new Action(() => arg1.Add(arg2)));
        }

        private void Main_OutMissionItemAddEvent(System.Collections.ObjectModel.ObservableCollection<OutMissionItem> arg1, OutMissionItem arg2)
        {
            Dispatcher.BeginInvoke(new Action(() => arg1.Add(arg2)));
        }
    }
}
