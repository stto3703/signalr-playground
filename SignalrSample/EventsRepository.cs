using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Push.Common.Models;

namespace SignalrSample
{
	public class EventsRepository
	{
		public static EventsRepository Instance = new EventsRepository();

		private readonly ConcurrentBag<Event> events;

		private EventsRepository()
		{
			events = new ConcurrentBag<Event>(DefaultList);
		}

		public IEnumerable<Event> GetEvents()
		{
			return events;
		}

		private static IEnumerable<Event> DefaultList
		{
			get
			{
				return new[]
				{
					new Event
					{
						Id = 42344,
						Name = "FC Barcelona - R Madrid",
						OpenDate = new DateTime(2015, 6, 1, 19, 0, 0, DateTimeKind.Utc),
						Sport = new Sport {Id = 4, Name = "Soccer"},
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
					},
					//new Event
					//{
					//	Id = 55435,
					//	Name = "Spain - Germany",
					//	OpenDate = new DateTime(2015, 8, 1, 22, 0, 0, DateTimeKind.Utc),
					//	Sport = new Sport {Id = 18, Name = "Volleyball"}
					//}
				};
			}
		}
	}
}