using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Pleioapp
{
	public class PaginatedMemberList
	{
		[JsonProperty]
		public int total { get; set; }

		[JsonProperty]
		public List<User> entities { get; set; }
	}
}

