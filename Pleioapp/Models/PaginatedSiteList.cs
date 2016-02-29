using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Pleioapp
{
	public class PaginatedSiteList
	{
		[JsonProperty]
		public int total { get; set; }

		[JsonProperty]
		public List<Site> entities { get; set; }
	}
}

