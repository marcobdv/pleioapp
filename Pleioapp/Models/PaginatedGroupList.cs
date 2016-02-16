using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Pleioapp
{
	public class PaginatedGroupList
	{
		[JsonProperty]
		public int total { get; set; }

		[JsonProperty]
		public List<Group> entities { get; set; }
	}
}

