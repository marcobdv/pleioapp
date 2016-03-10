using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Pleioapp
{
	public class PaginatedEventList
	{
		[JsonProperty]
		public int total { get; set; }

		[JsonProperty]
		public List<Event> entities { get; set; }
	}
}

