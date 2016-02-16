using System;
using Newtonsoft.Json;

namespace Pleioapp
{
	public class Group
	{
		[JsonProperty]
		public string guid { get; set; }

		[JsonProperty(PropertyName="time_created")]
		public DateTime timeCreated { get; set; }

		[JsonProperty]
		public string name { get; set; }

		[JsonProperty]
		public string description { get; set; }

		[JsonProperty]
		public string membership { get; set; }

		[JsonProperty(PropertyName="activities_unread_count")]
		public int activitiesUnreadCount { get; set; }
	}
}

