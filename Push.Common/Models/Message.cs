using System;

namespace Push.Common.Models
{
	[Serializable]
	public class Message
	{
		public int Id { get; set; }

		public string Text { get; set; }
	}
}
