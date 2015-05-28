using System;
using System.Collections.Generic;

namespace Push.Common.Models
{
	[Serializable]
	public class Scoreboard
	{
		public Scoreboard()
		{
			Messages = new List<Message>();
		}

		public IList<Message> Messages { get; set; }
	}
}
