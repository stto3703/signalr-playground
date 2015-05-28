using System.Collections.Generic;

namespace Push.Common.Notifications
{
	public class Packet
	{
		public long Id { get; set; }

		public IEnumerable<EventNotification> Payload { get; set; }
	}
}
