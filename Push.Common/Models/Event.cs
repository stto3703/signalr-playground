using System;

namespace Push.Common.Models
{
	[Serializable]
	public class Event
	{
		public Event()
		{
			Scoreboard = new Scoreboard();
		}

		public int Id { get; set; }

		public string Name { get; set; }

		public DateTime OpenDate { get; set; }

		public Sport Sport { get; set; }

		public Scoreboard Scoreboard { get; set; }
	}
}
