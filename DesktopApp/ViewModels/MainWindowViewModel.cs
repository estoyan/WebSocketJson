using DesktopApp.Commands;
using System.ComponentModel;
using System;
using System.Threading.Tasks;
using Microsoft.Owin.Hosting;
using System.Windows;
using System.Windows.Input;
using DesktopApp.Services;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace DesktopApp.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private string host = "localhost";
        private int port = 80;
        private string chat;
        private ICommand sendMessageCommand;
        private ICommand startServerCommand;
        private ICommunicationService communicationService;
        private bool isServerStarted;
        private bool isConnected;
        private List<JToken> jsonToken;


        public MainWindowViewModel(ICommunicationService service)
        {
            this.communicationService = service;
            service.NewMessage += this.NewMessage;
            this.jsonToken = new List<JToken>();
        }

        public IDisposable MySignalR { get; set; }
        public bool IsConnected
        {
            get { return isConnected; }
            set
            {
                isConnected = value;
                base.OnPropertyChanged();
            }
        }

        public bool IsServerStarted
        {
            get { return isServerStarted; }
            set
            {
                isServerStarted = value;
                base.OnPropertyChanged();
            }
        }

        public string Host
        {
            get
            {
                return this.host;
            }
            set
            {
                if (value == null || value == string.Empty)
                {
                    this.host = "localhost";
                }
                else
                {
                    this.host = value;
                    base.OnPropertyChanged();

                }

            }
        }
        public int Port
        {
            get
            {
                return this.port;
            }
            set
            {
                this.port = value;
                base.OnPropertyChanged();
            }
        }

        public string Chat
        {
            get
            {
                return this.chat;
            }
            set
            {
                this.chat = value;
            }
        }
        
        public List<JToken> JsonToken
        {
            get
            {
                return this.jsonToken;
            }
            private set
            {
                this.jsonToken = value;
                base.OnPropertyChanged();
            }
        }

        public ICommand SendMessageCommand
        {
            get
            {
                if (this.sendMessageCommand == null)
                {
                    this.sendMessageCommand =
                        new RelayCommandAsync(() => SendMessage(), (o) => CanSendMessage());
                }
                return this.sendMessageCommand;
            }
        }

        public ICommand StartServerCommand
        {
            get
            {
                if (this.startServerCommand == null)
                {
                    this.startServerCommand = new RelayCommand((o) => { StartServer(o); ConnectToServer(o); }, (o) => CanStartServer());

                }
                return this.startServerCommand;
            }
        }

        private void StartServer(object o)
        {
            //TODO extract this to Service
            try
            {
                string url = $"http://{this.host}:{this.port}/";
                this.MySignalR = WebApp.Start<Startup>(url);

                if (this.MySignalR == null)
                {
                    MessageBox.Show("web app started failed !");
                }
                else
                {
                    MessageBox.Show("web app started! Try to visit " + url + "signalr/hubs");
                    this.IsServerStarted = true;

                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async Task<bool> ConnectToServer(object o)
        {
            try
            {
                if (this.IsServerStarted)
                {
                    string url = $"http://{this.host}:{this.port}/signalr";
                    await this.communicationService.ConnectAsync(url);
                    IsConnected = true;
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception) { return false; }
        }


        private bool CanStartServer()
        {
            //TODO add validation
            return true;
        }

        private bool CanSendMessage()
        {

            //TODO add validation!
            return true;
        }

        private async Task<bool> SendMessage()
        {
            try
            {
                await this.communicationService.SendBroadcastMessageAsync(this.chat);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
            finally
            {
                try
                {
                var jsonToken = JToken.Parse(this.chat);
                if (jsonToken != null)
                {
                    this.JsonToken.Add(jsonToken);
                }
                }
                catch (JsonReaderException ex)
                {
                    MessageBox.Show("Could not open the JSON string:\r\n" + ex.Message);
                }
            }
        }

        private void NewMessage(string msg)
        {
            try
            {
                var jsonToken = JToken.Parse(msg);
                if (jsonToken != null)
                {
                    this.JsonToken.Add(jsonToken);
                }
            }
            catch (JsonReaderException ex)
            {
                MessageBox.Show("Could not open the JSON string:\r\n" + ex.Message);
            }
        }
    }
}
