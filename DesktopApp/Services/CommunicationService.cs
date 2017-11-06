using Microsoft.AspNet.SignalR.Client;
using Microsoft.AspNet.SignalR.Client.Transports;
using Microsoft.Owin.Hosting;
using System;
using System.Threading.Tasks;
using System.Windows;

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
            await this.Connection.Start(new WebSocketTransport());
        }

        public async Task SendBroadcastMessageAsync(string msg)
        {
            await this.hubProxy.Invoke("send", msg);
        }

        public bool StartServer(string serverUrl)
        {
            try
            {
                var signalR = WebApp.Start<Startup>(serverUrl);
                if (signalR == null)
                {
                    MessageBox.Show("web app started failed !");
                    return false;
                }
                else
                {
                    MessageBox.Show("web app started! Try to visit " + serverUrl + "signalr/hubs");
                    return true;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }
    }
    }
