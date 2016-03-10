using System;
using Newtonsoft.Json;

namespace Pleioapp
{
	public class User
	{
		[JsonProperty]
		public string guid { get; set; }

		[JsonProperty]
		public string name { get; set; }

		[JsonProperty]
		public string url { get; set; }

		[JsonProperty(PropertyName="icon_url")]
		public string iconUrl { get; set; }
	}
}

