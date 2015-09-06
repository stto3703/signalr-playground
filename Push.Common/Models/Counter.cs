using System;
using Newtonsoft.Json;

namespace Push.Common.Models
{
	[Serializable]
	public class Counter
	{
		[JsonProperty("id")]
		public int Id { get; set; }

		public int PeriodId { get; set; }

		public int Value { get; set; }
	}
}
