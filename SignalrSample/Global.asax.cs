using Microsoft.AspNet.SignalR;
using System;
using System.Web.Hosting;
using Timer = System.Timers.Timer;

namespace SignalrSample
{
	public class Global : System.Web.HttpApplication
	{
		protected void Application_Start(object sender, EventArgs e)
		{
			HostingEnvironment.QueueBackgroundWorkItem(cancelToken => StartNotificationTimer());
		}

		private static void StartNotificationTimer()
		{
			var timer = new Timer
			{
				Interval = 2000, 
				AutoReset = true
			};

			timer.Elapsed += async (sender, args) =>
			{
				var msg = DateTime.UtcNow;
				var clients = GlobalHost.ConnectionManager.GetHubContext<AdminHub>().Clients;
				await clients.Group("odd").oddNotification(msg, HostingEnvironment.SiteName);
				await clients.Group("even").evenNotification(msg);
			};

			timer.Start();
		}
	}
}