using System;
using Newtonsoft.Json;

namespace Pleioapp
{
	public class Group : Object
	{
		[JsonProperty]
		public string membership;

		[JsonProperty(PropertyName="activities_unread_count")]
		public int activitiesUnreadCount;
	}
}

