using DesktopApp.Commands;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using DesktopApp.Services;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Collections.ObjectModel;

namespace DesktopApp.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private const string defaultHost= "localhost";
        private const int defaultPort = 8080;
        private string host = defaultHost;
        private int port = defaultPort;
        private string chat;
        private ICommand sendMessageCommand;
        private ICommand startServerCommand;
        private ICommunicationService communicationService;
        private bool isServerStarted;
        private bool isConnected;
        private ObservableCollection<JToken> jsonToken;


        public MainWindowViewModel(ICommunicationService service)
        {
            this.communicationService = service;
            service.NewMessage += this.NewMessage;
            this.jsonToken = new ObservableCollection<JToken>();
        }

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
                    this.host = defaultHost;
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
                if (value <-1 || value> 65535)
                {
                    this.port = defaultPort;
                }
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
                base.OnPropertyChanged();
            }
        }

        public ObservableCollection<JToken> JsonToken
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
            string url = $"http://{this.host}:{this.port}/";
            if (this.communicationService.StartServer(url))
            {
                this.IsServerStarted = true;
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

            return this.Host != null && this.Port > -1&&this.Port<65635&&!this.isServerStarted;
        }

        private bool CanSendMessage()
        {
            return this.IsServerStarted && this.isConnected&& this.Chat!=null;
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
                this.NewMessage(this.chat);
            }
        }

        private void NewMessage(string msg)
        {
            try
            {
                var jsonToken = JToken.Parse(msg);
                if (jsonToken != null)
                {
                    App.Current.Dispatcher.Invoke((Action)delegate
                    {
                        this.JsonToken.Add(jsonToken);

                    });
                }
            }
            catch (JsonReaderException ex)
            {
                MessageBox.Show("Could not open the JSON string:\r\n" + ex.Message);
            }
        }
    }
}
