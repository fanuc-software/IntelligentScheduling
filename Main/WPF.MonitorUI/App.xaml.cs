using GalaSoft.MvvmLight.Threading;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WPF.MonitorUI
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            string password = "Password01!";
            using (var cryptoProvider = System.Security.Cryptography.SHA1.Create())
            {
                byte[] passwordHash = cryptoProvider.ComputeHash(Encoding.UTF8.GetBytes(password));
                string result = "new byte[] { " +
                    String.Join(",", passwordHash.Select(x => "0x" + x.ToString("x2")).ToArray())
                    + " } ";
            }

            base.OnStartup(e);
        }
    }
}
