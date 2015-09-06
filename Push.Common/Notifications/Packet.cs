using System.Collections.Generic;
using System.Web.Hosting;

namespace Push.Common.Notifications
{
	public class Packet
	{
		public string Host
		{
			get { return HostingEnvironment.SiteName; }
		}

		public long Id { get; set; }

		public IEnumerable<EventNotification> Payload { get; set; }
	}
}
