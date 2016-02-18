using System;
using Newtonsoft.Json;
using System.ComponentModel;

namespace Pleioapp
{
	public class Group : INotifyPropertyChanged
	{

		public event PropertyChangedEventHandler PropertyChanged;

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

		public void MarkAsRead() {
			activitiesUnreadCount = 0;
			OnPropertyChanged ("activitiesUnreadCount");
		}

		protected virtual void OnPropertyChanged(string propertyName)
		{
			if (PropertyChanged != null)
			{
				PropertyChanged(this,
					new PropertyChangedEventArgs(propertyName));
			}
		}
	}
}

