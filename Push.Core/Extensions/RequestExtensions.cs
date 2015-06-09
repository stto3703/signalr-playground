using Microsoft.AspNet.SignalR;

namespace Push.Core.Extensions
{
	public static class RequestExtensions
	{
		public static string RemoteIpAddress(this IRequest request)
		{
			object ipAddress;
			if (request.Environment.TryGetValue("server.RemoteIpAddress", out ipAddress))
			{
				return ipAddress as string;
			}
			return null;
		}
	}
}
