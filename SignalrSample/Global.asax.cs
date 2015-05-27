using System.Linq;
using System.Runtime.Remoting.Channels;
using Microsoft.AspNet.SignalR;
using System;
using System.Web.Hosting;
using Push.Common;
using Push.Common.Notifications;
using Timer = System.Timers.Timer;
using Push.Core.SimpleHelpers;

namespace SignalrSample
{
	public class Global : System.Web.HttpApplication
	{
		protected void Application_Start(object sender, EventArgs e)
		{
			HostingEnvironment.QueueBackgroundWorkItem(cancelToken => StartRandomEventUpdate());
			//HostingEnvironment.QueueBackgroundWorkItem(cancelToken => StartOddEventNotificationTimer());
		}


		private static void StartRandomEventUpdate()
		{
			var timer = new Timer
			{
				Interval = 1000,
				AutoReset = true
			};

			var rand = new Random();

			timer.Elapsed += (sender, args) =>
			{
				var oldEvent = EventsRepository.Instance.GetEvents().PickRandom();
				var newEvent = oldEvent.DeepClone();
				var deltaMinutes = rand.Next(-1, 1);
				newEvent.OpenDate = newEvent.OpenDate.AddMinutes(deltaMinutes);

				var delta = ObjectDiffPatch.GenerateDiff(oldEvent, newEvent);
				var clients = GlobalHost.ConnectionManager.GetHubContext<AdminHub>().Clients.All;
				var notification = new EventNotification
				{
					Id = newEvent.Id,
					Delta = delta.NewValues
				};

				clients.updateEvent(notification);
			};

			timer.Start();
		}



		private static void StartOddEventNotificationTimer()
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
				await clients.Group("even").evenNotification(msg, HostingEnvironment.SiteName);
			};

			timer.Start();
		}
	}
}