using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopApp.Services
{
   public  interface  ICommunicationService
    {
        event Action<string> NewMessage;
        bool StartServer(string serverUrl);
        Task ConnectAsync(string hostUrl);
        Task LogoutAsync();
        Task SendBroadcastMessageAsync(string msg);
    }
}
