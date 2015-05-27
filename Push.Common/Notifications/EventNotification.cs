using Newtonsoft.Json.Linq;

namespace Push.Common.Notifications
{
	public class EventNotification
	{
		public int Id { get; set; }

		public JObject Delta { get; set; }
	}
}
