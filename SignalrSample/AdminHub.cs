using Microsoft.AspNet.SignalR;
using System.Threading.Tasks;

namespace SignalrSample
{
	public class AdminHub : Hub
	{
		public Task Join(string group)
		{
			return Groups.Add(Context.ConnectionId, group);
		}

		public Task Leave(string group)
		{
			return Groups.Remove(Context.ConnectionId, group);
		}
	}
}