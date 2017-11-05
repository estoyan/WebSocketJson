using Microsoft.Owin.Cors;
using Owin;

namespace DesktopApp
{
   public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseCors(CorsOptions.AllowAll);
            app.MapSignalR();
        }
    }
}
