using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR.Client;
namespace Agv.Common
{
    public class SignalrService
    {
        string url = "";
        string hubName = "";
        private HubConnection hubConnect;
        private IHubProxy hubProxy;

        public SignalrService(string url, string name)
        {
            this.url = url;
            hubName = name;
            hubConnect = new HubConnection(url);
            hubProxy = hubConnect.CreateHubProxy(hubName);

            hubConnect.StateChanged += HubConnect_StateChanged;
        }

        public async Task Start()
        {
            try
            {
                await hubConnect.Start();
            }
            catch (Exception ex)
            {


            }

        }
        public void OnMessage<T>(string actionName, Action<T> doAction)
        {
            hubProxy.On<T>(actionName, doAction);
        }
        public async Task<T> Send<T>(string actionName, T obj)
        {
            if (hubConnect != null && hubConnect.State == ConnectionState.Connected)
            {

              return  await hubProxy.Invoke<T>(actionName, obj);

            }
            return default(T);
        }

        private async void HubConnect_StateChanged(StateChange obj)
        {
            if (obj.NewState == ConnectionState.Disconnected)
            {
                await Start();
            }
        }
    }
}
