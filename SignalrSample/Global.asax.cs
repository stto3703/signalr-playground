using Push.Core;
using System;

namespace SignalrSample
{
	public class Global : System.Web.HttpApplication
	{
		private readonly Random rand = new Random();
		private readonly OutboundBufferedQueue buffer = new OutboundBufferedQueue();

		protected void Application_Start(object sender, EventArgs e)
		{
			//HostingEnvironment.QueueBackgroundWorkItem(cancelToken => StartRandomEventUpdate());
			//HostingEnvironment.QueueBackgroundWorkItem(cancelToken => StartOddEventNotificationTimer());
		}
	}
}