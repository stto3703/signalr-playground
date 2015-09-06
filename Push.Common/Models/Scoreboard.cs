using System;
using System.Collections.Generic;

namespace Push.Common.Models
{
	[Serializable]
	public class Scoreboard
	{
		public List<Message> Messages { get; set; }

		public GameStatistics Stats { get; set; }
	}
}
