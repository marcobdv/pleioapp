using System;
using Newtonsoft.Json;
using System.ComponentModel;

namespace Pleioapp
{
	public class Site : IEquatable<Site>, INotifyPropertyChanged
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
		public string url { get; set; }

		[JsonProperty(PropertyName="groups_unread_count")]
		public int groupsUnreadCount { get; set; }

		public bool hasGroupsUnread { 
			get {
				return groupsUnreadCount != 0;
			}
		}

		public bool Equals(Site other) {
			return string.Equals (guid, other.guid);
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

