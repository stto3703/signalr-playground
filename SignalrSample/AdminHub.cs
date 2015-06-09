using System;
using Microsoft.AspNet.SignalR;
using System.Threading.Tasks;
using Push.Core.Extensions;

namespace SignalrSample
{
	public class AdminHub : Hub
	{
		public override Task OnConnected()
		{
			var ip = Context.Request.RemoteIpAddress();
			return base.OnConnected();
		}


		public void Start()
		{
			Generator.Instance.Start();
		}

		public void Stop()
		{
			Generator.Instance.Stop();
			GC.Collect();
			GC.WaitForPendingFinalizers();
			GC.WaitForFullGCComplete();
			GC.Collect();

		}
	}
}