using System;
using Newtonsoft.Json;
using NUnit.Framework;
using Push.Common;
using Push.Common.Models;
using Push.Core.SimpleHelpers;

namespace Tests
{
	[TestFixture]
	public class DeltaTests
	{
		[Test]
		public void GenerateSimpleDelta()
		{
			var e1 = new Event
			{
				Id = 312332,
				Name = "Real Madrid - FC Barcelona",
				OpenDate = DateTime.UtcNow,
				Sport = new Sport {Id = 4, Name = "Soccer"},
				Scoreboard = new Scoreboard
				{
					Messages = new[]
					{
						new Message
						{
							Id = 123,
							Text = "Goal for Real Madrid"
						},
						new Message
						{
							Id = 124,
							Text = "Penalty for Barcelona"
						}
					}
				}
			};

			var e2 = e1.DeepClone();

			e2.OpenDate = DateTime.UtcNow.AddMinutes(10);
			e2.Scoreboard = new Scoreboard
			{
				Messages = new[]
				{
					new Message
					{
						Id = 4324,
						Text = "Red card for Real Madrid"
					},
					new Message
					{
						Id = 124,
						Text = "Penalty for Barcelona"
					},
					new Message
					{
						Id = 645,
						Text = "No clear indication"
					},
				}
			};

			var diff = ObjectDiffPatch.GenerateDiff(e1, e2);

			var payload = diff.NewValues.ToString(Formatting.Indented);

			Assert.IsNotNull(payload);
		}
	}
}