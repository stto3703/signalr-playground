using System.Threading;
using Push.Common.Notifications;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
//using Timer = System.Timers.Timer;

namespace Push.Core
{
	public class OutboundBufferedQueue : ConcurrentQueue<EventNotification>, IDisposable
	{
		private const int MaxPackageSize = 1000;
		private const double FlushInterval = 1;
		private readonly Timer timer;
		private long sequence;
		private long stats;

		public event EventHandler<PackageAvailableEventArgs> ThresholdReached;

		public OutboundBufferedQueue()
		{
			sequence = 0;
			stats = 0;

			timer = new Timer(FLushNext, null, TimeSpan.Zero, TimeSpan.FromMilliseconds(FlushInterval));

			//timer = new Timer
			//{
			//	AutoReset = true,
			//	Interval = FlushInterval
			//};

			//timer.Elapsed += (sender, e) => FLushNext();
			//timer.Start();
		}

		public void EnqueueNotification(EventNotification notification)
		{
			Interlocked.Increment(ref stats);
			Enqueue(notification);
		}

		public void FLushNext(object state)
		{
			var package = new List<EventNotification>();

			System.Diagnostics.Debug.WriteLine(string.Format("Queue size: {0}. Total sent: {1}", Count, stats));

			var payloadCount = 0;
			EventNotification next;
			while (payloadCount < MaxPackageSize && TryDequeue(out next))
			{
				payloadCount++;
				package.Add(next);
			}

			if (package.Any())
			{
				Interlocked.Increment(ref sequence);
				OnThreshholdReached(new PackageAvailableEventArgs { Packet = new Packet
				{
					Id = sequence,
					Payload = package
				}});
			}
		}

		private void OnThreshholdReached(PackageAvailableEventArgs e)
		{
			if (ThresholdReached != null)
			{
				ThresholdReached(this, e);
			}
		}

		public void Dispose()
		{
			timer.Dispose();
		}
	}
}
