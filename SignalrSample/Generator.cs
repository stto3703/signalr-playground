using Microsoft.AspNet.SignalR;
using Push.Common;
using Push.Common.Notifications;
using Push.Core;
using Push.Core.SimpleHelpers;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;

namespace SignalrSample
{
	public class Generator
	{
		public readonly static Generator Instance = new Generator();

		private readonly OutboundBufferedQueue buffer;
		private readonly Random rand;
		private readonly Timer timer;

		private Generator()
		{
			buffer = new OutboundBufferedQueue();
			rand = new Random();
			timer = new Timer
			{
				Interval = 1,
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
				//buffer.EnqueueNotification(new EventNotification());
			}
		}

		public void Start()
		{
			timer.Start();
		}

		public void Stop()
		{
			timer.Stop();
		}
	}
}