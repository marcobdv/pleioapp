using System;
using Newtonsoft.Json;
using System.ComponentModel;

namespace Pleioapp
{
	public class Site : IEquatable<Site>, INotifyPropertyChanged
	{

		public event PropertyChangedEventHandler PropertyChanged;
		private int _groupsUnreadCount;

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

		public bool Equals(Site other) {
			return string.Equals (guid, other.guid);
		}
			
		public bool hasGroupsUnread { 
			get {
				return _groupsUnreadCount != 0;
			}
		}
			
		[JsonProperty(PropertyName="groups_unread_count")]
		public int groupsUnreadCount {
			get { return _groupsUnreadCount; }
			set {
				_groupsUnreadCount = value;
				OnPropertyChanged ("groupsUnreadCount");
				OnPropertyChanged ("hasGroupsUnread");
			}
		}

		public void MarkAsRead() {
			_groupsUnreadCount = 0;
			OnPropertyChanged ("activitiesUnreadCount");
			OnPropertyChanged ("hasActivitiesUnread");
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

