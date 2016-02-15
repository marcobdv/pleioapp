using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Pleioapp
{
	public class PaginatedList
	{
		[JsonProperty]
		public int total;

		[JsonProperty]
		public List<Group> entities;
	}
}

