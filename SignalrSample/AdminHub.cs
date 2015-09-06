using Microsoft.AspNet.SignalR;
using Push.Common.Models;
using Push.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SignalrSample
{
	public class AdminHub : Hub
	{
		public override Task OnConnected()
		{
			var ip = Context.Request.RemoteIpAddress();
			return base.OnConnected();
		}


		public Task<IEnumerable<Event>> Start()
		{
			var events = EventsRepository.Instance.GetEvents();
			UpdateGenerator.Instance.Start();
			return Task.FromResult(events);
		}

		public void Stop()
		{
			UpdateGenerator.Instance.Stop();
			GC.Collect();
			GC.WaitForPendingFinalizers();
			GC.WaitForFullGCComplete();
			GC.Collect();

		}
	}
}