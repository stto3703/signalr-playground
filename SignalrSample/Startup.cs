using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(SignalrSample.Startup))]

namespace SignalrSample
{
	public class Startup
	{
		public void Configuration(IAppBuilder app)
		{
			//GlobalHost.DependencyResolver.UseRedis("127.0.0.1", 6379, "", "Push.Sample.Redis");
			app.MapSignalR();
		}
	}
}
