using System.Collections.Generic;
using Newtonsoft.Json;

namespace Pleioapp
{
	public class PaginatedList<T>
	{
		[JsonProperty]
		public int total { get; set; }

		[JsonProperty]
		public List<T> entities { get; set; }
	}
}

