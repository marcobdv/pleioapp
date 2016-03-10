using System;
using System.Collections.Generic;
using Xamarin.Forms;
using System.Collections.ObjectModel;

namespace Pleioapp
{
	public partial class ActivityPage : ContentPage
	{
		ObservableCollection<Activity> activities = new ObservableCollection<Activity>();
		App app = (App) App.Current;
		Activity SelectedItem = null;

		public ActivityPage ()
		{
			InitializeComponent ();
			ActivityListView.ItemsSource = activities;

			ActivityListView.ItemTapped += (sender, e) => {
				if (ActivityListView.SelectedItem == SelectedItem) {
					if (SelectedItem.targetObject.url != null) {
						app.ssoService.OpenUrl(SelectedItem.targetObject.url);
					}
				}

				SelectedItem = (Activity) ActivityListView.SelectedItem;
			};
		}

		public async void setGroup(Group group)
		{
			activities.Clear ();
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

