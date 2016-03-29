using System;
using Newtonsoft.Json;

namespace Pleioapp
{
	public class Activity
	{
		[JsonProperty]
		public string id { get; set; }

		[JsonProperty]
		public User subject { get; set; }

		[JsonProperty(PropertyName="action_type")]
		public string actionType { get; set; }

		[JsonProperty(PropertyName="object")]
		public Object targetObject { get; set; }

		[JsonProperty(PropertyName="time_created")]
		public DateTime timeCreated { get; set; }

		public string humanReadableActionType
		{
			get 
			{
				switch (actionType) 
				{
					case "create":
						return subject.name + " heeft een " + targetObject.humanReadableType + " toegevoegd.";
					case "comment":
						return subject.name + " heeft een reactie toegevoegd.";
					case "reply":
						return subject.name + " heeft een reactie toegevoegd.";
					default:
						return subject.name + " heeft " + actionType + " gedaan.";
				}
			}
		}
	}
}

