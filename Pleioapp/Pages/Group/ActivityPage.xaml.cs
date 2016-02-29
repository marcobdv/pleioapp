using System;
using System.Collections.Generic;
using Xamarin.Forms;
using System.Collections.ObjectModel;

namespace Pleioapp
{
	public partial class ActivityPage : ContentPage
	{
		ObservableCollection<Activity> activities = new ObservableCollection<Activity>();

		public ActivityPage ()
		{
			InitializeComponent ();
			ActivityListView.ItemsSource = activities;
		}

		public async void setGroup(Group group)
		{
			activities.Clear ();

			var app = (App)App.Current;
			var service = app.webService;

			foreach (Activity activity in await service.GetActivities (group)) {
				activities.Add (activity);
			}

			if (group.activitiesUnreadCount > 0) {
				group.MarkAsRead ();
				await service.MarkGroupAsRead (group);
			}
		}
	}

}

