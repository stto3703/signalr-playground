using System;
using Newtonsoft.Json;

namespace Push.Common.Models
{
	[Serializable]
	public class LiveDoubleCounter
	{
		public string Name { get; set; }

		[JsonProperty("id")]
		public int Id { get; set; }

		public Team Team1 { get; set; }

		public Team Team2 { get; set; }
	}
}
