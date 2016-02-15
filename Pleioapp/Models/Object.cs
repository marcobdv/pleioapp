using System;
using Newtonsoft.Json;

namespace Pleioapp
{
	public class Object
	{
		[JsonProperty]
		public string guid;

		[JsonProperty(PropertyName="time_created")]
		public string timeCreated;

		[JsonProperty]
		public string name;

		[JsonProperty]
		public string description;

		[JsonProperty(PropertyName="icon_url")]
		public string iconUrl;
	}
}

