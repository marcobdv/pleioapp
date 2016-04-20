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
		Group Group;

		public ActivityPage ()
		{
			InitializeComponent ();
			ActivityListView.ItemsSource = activities;

			ActivityListView.ItemSelected += (sender, e) => {
				var activity = e.SelectedItem as Activity;
				if (activity.targetObject.url != null) {
					app.ssoService.OpenUrl(activity.targetObject.url);
				}
			};

			CouldNotLoad.GestureRecognizers.Add (new TapGestureRecognizer {
				Command = new Command (() => {
					Reload();
				}),
				NumberOfTapsRequired = 1
			});

			MessagingCenter.Subscribe<Xamarin.Forms.Application> (App.Current, "select_group", async(sender) => {
				var app = (App)App.Current;
				setGroup(app.currentGroup);
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
				CouldNotLoad.IsVisible = true;
				System.Diagnostics.Debug.WriteLine ("Catched exception " + e);
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

