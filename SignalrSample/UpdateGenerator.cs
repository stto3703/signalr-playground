using JsonDiffPatch;
using Microsoft.AspNet.SignalR;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Push.Common;
using Push.Common.Models;
using Push.Common.Notifications;
using Push.Core;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;

namespace SignalrSample
{
	public class UpdateGenerator
	{
		public readonly static UpdateGenerator Instance = new UpdateGenerator();

		private readonly OutboundBufferedQueue buffer;
		private readonly JsonDiffer jsonDiffer;
		private readonly Random rand;
		private readonly Timer timer;

		private UpdateGenerator()
		{
			buffer = new OutboundBufferedQueue();
			jsonDiffer = new JsonDiffer();
			rand = new Random();
			timer = new Timer
			{
				Interval = 1000,
				AutoReset = true
			};

			buffer.ThresholdReached += (sender, args) =>
			{
				var clients = GlobalHost.ConnectionManager.GetHubContext<AdminHub>().Clients.All;
				clients.updateEvent(args.Packet);
			};

			timer.Elapsed += (sender, args) => Parallel.Invoke(
				GenerateUpdate//,
				//GenerateUpdate,
				//GenerateUpdate,
				//GenerateUpdate,
				//GenerateUpdate,
				//GenerateUpdate,
				//GenerateUpdate
			);
		}

		private void GenerateUpdate()
		{
			try
			{
				var oldEvent = EventsRepository.Instance.GetEvents().First(e => e.Scoreboard != null); //.PickRandom();
				//oldEvent.Scoreboard.Messages.Add(new Message { Id = rand.Next(), Text = "Message content " + rand.Next() });
				var newEvent = oldEvent.DeepClone();
				var deltaMinutes = rand.Next(-10, 10);
				//newEvent.OpenDate = newEvent.OpenDate.AddMinutes(deltaMinutes);
				//newEvent.Scoreboard.Messages.Add(new Message { Id = rand.Next(), Text = "Message content " + rand.Next() });
				newEvent.Scoreboard.Messages[0].Text = "Message content " + rand.Next();
				newEvent.Scoreboard.Stats.DoubleCounters[0].Team1.Counters[0].Value = 11 + rand.Next();

				var existingJson = JObject.FromObject(oldEvent).Root;
				var updatedJson = JObject.FromObject(newEvent).Root;

				var patchDoc = jsonDiffer.Diff(existingJson, updatedJson, true);

				if (patchDoc.Operations.Any())
				{
					buffer.EnqueueNotification(new EventNotification
					{
						Id = newEvent.Id,
						Delta = patchDoc.ToString(Formatting.None)
					});
				}
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine(ex.Message);
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