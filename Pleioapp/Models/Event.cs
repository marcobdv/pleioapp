using System;
using Newtonsoft.Json;
using System.ComponentModel;

namespace Pleioapp
{
	public class Event
	{
		[JsonProperty]
		public string guid { get; set; }

		[JsonProperty]
		public string title { get; set; }

		[JsonProperty]
		public string description { get; set; }

		[JsonProperty]
		public string url { get; set; }

		[JsonProperty(PropertyName="start_time")]
		public DateTime startTime { get; set; }

		[JsonProperty(PropertyName="end_time", NullValueHandling=NullValueHandling.Ignore)]
		public DateTime endTime { get; set; }

		[JsonProperty(PropertyName="time_created")]
		public DateTime timeCreated { get; set; }

		public bool hasEndTime {
			get {
				if (endTime != default(DateTime)) {
					return true;
				}

				return false;
			}
		}
	}
}

