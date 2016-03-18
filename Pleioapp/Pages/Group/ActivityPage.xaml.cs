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
		Group Group;

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

			CouldNotLoad.GestureRecognizers.Add (new TapGestureRecognizer {
				Command = new Command (() => {
					Reload();
				}),
				NumberOfTapsRequired = 1
			});

			MessagingCenter.Subscribe<Xamarin.Forms.Application> (App.Current, "logout", async(sender) => {
				SelectLeft.IsVisible = true;
			});
		}

		public async void Reload()
		{
			activities.Clear ();
			CouldNotLoad.IsVisible = false;
			NoItems.IsVisible = false;
			ActivityIndicator.IsVisible = true;

			try {
				var service = app.webService;
				var webActivities = await service.GetActivities (Group);
				foreach (Activity activity in webActivities) {
					activities.Add (activity);
				}

				if (webActivities.Count == 0) {
					NoItems.IsVisible = true;
				}
			} catch(Exception e) {
				System.Diagnostics.Debug.WriteLine ("Catched exception " + e);
				Xamarin.Insights.Report (e);
				CouldNotLoad.IsVisible = true;
			}
				
			ActivityIndicator.IsVisible = false;
		}

		public async void setGroup(Group group)
		{
			Group = group;
			if (group == null) {
				activities.Clear ();
				return;
			}
				
			SelectLeft.IsVisible = false;
			CouldNotLoad.IsVisible = false;
			Reload ();

			if (group.activitiesUnreadCount > 0) {
				group.MarkAsRead ();
				var service = app.webService;
				await service.MarkGroupAsRead (Group);
			}
		}
	}

}

