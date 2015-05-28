using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using Push.Common;
using Push.Common.Notifications;
using Push.Core;
using Push.Core.SimpleHelpers;
using System;
using System.Web.Hosting;
using Timer = System.Timers.Timer;

namespace SignalrSample
{
	public class Global : System.Web.HttpApplication
	{
		private readonly Random rand = new Random();
		private readonly OutboundBufferedQueue buffer = new OutboundBufferedQueue();

		protected void Application_Start(object sender, EventArgs e)
		{
			HostingEnvironment.QueueBackgroundWorkItem(cancelToken => StartRandomEventUpdate());
			//HostingEnvironment.QueueBackgroundWorkItem(cancelToken => StartOddEventNotificationTimer());
		}


		private void StartRandomEventUpdate()
		{
			var timer = new Timer
			{
				Interval = 10,
				AutoReset = true
			};

			buffer.ThresholdReached += (sender, args) =>
			{
				var clients = GlobalHost.ConnectionManager.GetHubContext<AdminHub>().Clients.All;
				clients.updateEvent(args.Packet);
			};

			timer.Elapsed += (sender, args) => Parallel.Invoke(
				GenerateUpdate,
				GenerateUpdate,
				GenerateUpdate,
				GenerateUpdate,
				GenerateUpdate,
				GenerateUpdate,
				GenerateUpdate
				);

			timer.Start();
		}

		private void GenerateUpdate()
		{
			var oldEvent = EventsRepository.Instance.GetEvents().First(); //.PickRandom();
			//oldEvent.Scoreboard.Messages.Add(new Message { Id = rand.Next(), Text = "Message content " + rand.Next() });
			var newEvent = oldEvent.DeepClone();
			var deltaMinutes = rand.Next(-10, 10);
			newEvent.OpenDate = newEvent.OpenDate.AddMinutes(deltaMinutes);
			//newEvent.Scoreboard.Messages.Add(new Message { Id = rand.Next(), Text = "Message content " + rand.Next() });

			var delta = ObjectDiffPatch.GenerateDiff(oldEvent, newEvent);

			if (!delta.AreEqual)
			{
				buffer.EnqueueNotification(new EventNotification
				{
					Id = newEvent.Id,
					Delta = delta.NewValues
				});
			}
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