using System;
using Newtonsoft.Json;
using System.ComponentModel;

namespace Pleioapp
{
	public class FSObject
	{
		[JsonProperty]
		public string guid { get; set; }

		[JsonProperty]
		public string title { get; set; }

		[JsonProperty]
		public string url { get; set; }

		[JsonProperty]
		public string subtype { get; set; }

		[JsonProperty(PropertyName = "time_created")]
		public DateTime timeCreated { get; set; }


	}
}

