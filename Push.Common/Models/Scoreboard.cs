using System;
using System.Collections.Generic;

namespace Push.Common.Models
{
	[Serializable]
	public class Scoreboard
	{
		public IList<Message> Messages { get; set; }
	}
}
