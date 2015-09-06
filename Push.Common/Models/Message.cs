using System;
using Newtonsoft.Json;


namespace Push.Common.Models
{
	[Serializable]
	public class Message
	{
		[JsonProperty("id")]
		public int Id { get; set; }

		public string Text { get; set; }
	}
}
