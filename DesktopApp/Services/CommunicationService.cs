using Microsoft.AspNet.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopApp.Services
{
    public class CommunicationService : ICommunicationService
    {
        public event Action<string> NewMessage;
        private IHubProxy hubProxy;
        public HubConnection Connection { get; set; }

        public async Task ConnectAsync(string hostUrl)
        {
            this.Connection = new HubConnection(hostUrl);
            this.hubProxy = Connection.CreateHubProxy("DesktopHub");
            this.hubProxy.On<string>("sendMessage", (m) => NewMessage?.Invoke(m));
            //hubProxy.On<string, string>("sendMessage", (n, m) => NewMessage?.Invoke(n, m);
            await this.Connection.Start();
        }

        public Task LogoutAsync()
        {
            throw new NotImplementedException();
        }

        public async Task SendBroadcastMessageAsync(string msg)
        {
            await this.hubProxy.Invoke("send", msg);
        }

        public bool StartServer(string serverUrl)
        {
            throw new NotImplementedException();
        }
    }
}
