using DesktopApp.Commands;
using System.ComponentModel;

namespace DesktopApp.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private string host = "localhost";
        private int port = 80;
        private string chat;
        private SendMessageCommand sendMessage;
        private StartServerCommand startServer;


        public MainWindowViewModel()
        {
            this.sendMessage = new SendMessageCommand();
            this.startServer = new StartServerCommand();
            base.PropertyChanged += MainWindowViewModel_PropertyChanged;
        }

        private void MainWindowViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            this.startServer.CanExecute(new object[] { host, port.ToString() });
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
                    base.OnPropertyChanged(this, new PropertyChangedEventArgs("Host"));

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
                base.OnPropertyChanged(this, new PropertyChangedEventArgs("Port"));
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

        public SendMessageCommand SendMessage
        {
            get
            {
                return this.sendMessage;
            }
        }

        public StartServerCommand StartServer
        {
            get
            {
                return this.startServer;
            }
        }
    }
}
