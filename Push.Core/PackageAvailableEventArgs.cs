using Push.Common.Notifications;
using System;

namespace Push.Core
{
	public class PackageAvailableEventArgs : EventArgs
	{
		public Packet Packet { get; set; } 
	}
}
