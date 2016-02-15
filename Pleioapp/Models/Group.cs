using System;
using Newtonsoft.Json;

namespace Pleioapp
{
	public class Group
	{
		[JsonProperty]
		public string guid;
		public string name;
		public string description;
		public string membership;

		[JsonProperty(PropertyName="icon_url")]
		public string iconUrl;

		[JsonProperty(PropertyName="activities_unread_count")]
		public int activitiesUnreadCount;

		[JsonProperty(PropertyName="time_created")]
		public string timeCreated;
	}
}

