using Microsoft.AspNet.SignalR;

namespace DesktopApp
{
    public class DesktopHub:Hub
    {
        public void Send(string name, object message)
        {
            Clients.All.addMessage(name, message);
            Clients.All.sendMessage(message);
        }
    }
}
