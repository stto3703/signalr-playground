using System;
using System.Collections.Generic;

namespace Push.Common.Models
{
	[Serializable]
	public class GameStatistics
	{
		public List<LiveDoubleCounter> DoubleCounters { get; set; } 

	}
}
