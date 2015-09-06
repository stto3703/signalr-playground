using JsonDiffPatch;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using Push.Common;
using Push.Common.Models;
using System;
using System.Collections.Generic;

namespace Tests
{
	[TestFixture]
	public class DeltaTests
	{
		[Test]
		public void GeneratePatches()
		{
			var e1 = new Event
			{
				Id = 312332,
				Name = "Real Madrid - FC Barcelona",
				OpenDate = DateTime.UtcNow,
				Sport = new Sport { Id = 4, Name = "Soccer" },
				Scoreboard = new Scoreboard
				{
					Messages = new List<Message>
					{
						new Message
						{
							Id = 123,
							Text = "Goal for Real Madrid"
						}
					},
					Stats = new GameStatistics
					{
						DoubleCounters = new List<LiveDoubleCounter>
						{
							new LiveDoubleCounter
							{
								Id = 1,
								Name = "Double Counter 1",
								Team1 = new Team
								{
									Counters = new List<Counter>
									{
										new Counter
										{
											Id = 1,
											PeriodId = 254,
											Value = 3
										},
										new Counter
										{
											Id = 2,
											PeriodId = 255,
											Value = 5
										}
									}
								}
							}
						}
					}
				}
			};

			var e2 = e1.DeepClone();

			//e2.OpenDate = DateTime.UtcNow.AddMinutes(10);

			//e2.Scoreboard.Messages[0].Text = "New update message";
			e2.Scoreboard.Messages.Add(new Message
			{
				Id = 124,
				Text = "Penalty for Barcelona"
			});

			e2.Scoreboard.Stats.DoubleCounters[0].Team1.Counters[0].Value = 11;
			e2.Scoreboard.Stats.DoubleCounters[0].Name = "Updated name";

			var jo1 = JObject.FromObject(e1).Root;
			var jo2 = JObject.FromObject(e2).Root;

			var patchDoc = new JsonDiffer().Diff(jo1, jo2, true);
			System.Diagnostics.Trace.WriteLine(patchDoc);

		}
	}
}