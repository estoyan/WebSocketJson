using Microsoft.Owin.Hosting;
using System;
using System.Windows;
using System.Windows.Input;

namespace DesktopApp.Commands
{
    public class StartServerCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        IDisposable MySignalR { get; set; }


        public bool CanExecute(object parameter)
        {
            //try
            //{
            //    var host = (string)((Object[])parameter)[0];
            //    var port = (string)((Object[])parameter)[1];
            //    if (!string.IsNullOrEmpty(host))
            //    {
            //        //this.CanExecuteChanged.Invoke(this, new EventArgs());
            return true;
            //    }

            //    return false;
            //}
            //catch (NullReferenceException)
            //{
            //    return false;
            //}
        }

        public void Execute(object parameter)
        {
            var host = (string)((Object[])parameter)[0];
            var port = (string)((Object[])parameter)[1];
            try
            {
                string url = $"http://{host}:{port}/";
                this.MySignalR = WebApp.Start<Startup>(url);

                if (this.MySignalR == null)
                    MessageBox.Show("web app started failed !");
                else
                    MessageBox.Show("web app started! Try to visit " + url + "signalr/hubs");

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
