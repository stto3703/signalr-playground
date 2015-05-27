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
					new Event { Id = 42344, Name = "FC Barcelona - R Madrid", OpenDate = new DateTime(2015, 6, 1, 19, 0, 0, DateTimeKind.Utc), Sport = new Sport { Id = 4, Name = "Soccer" } },
					new Event { Id = 55435, Name = "Spain - Germany", OpenDate = new DateTime(2015, 8, 1, 22, 0, 0, DateTimeKind.Utc), Sport = new Sport { Id = 18, Name = "Volleyball" } }
				};
			}
		} 
	}
}