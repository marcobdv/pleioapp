using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Pleioapp
{
	public class PaginatedActivityList
	{
		[JsonProperty]
		public int total { get; set; }

		[JsonProperty]
		public List<Activity> entities { get; set; }
	}
}

