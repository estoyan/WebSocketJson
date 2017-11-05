using Microsoft.AspNet.SignalR;

namespace DesktopApp
{
    public class DesktopHub:Hub
    {
        public void Send(string message)
        {
            Clients.All.sendMessage(message);
        }
    }
}
