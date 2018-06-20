﻿using System;
using Xamarin.Forms;
using System.Collections.ObjectModel;

namespace Pleioapp
{
	public partial class ActivityPage : ContentPage
	{
		ObservableCollection<Activity> activities = new ObservableCollection<Activity>();
		App app = (App) Application.Current;
		Group Group;

		public ActivityPage ()
		{
			InitializeComponent ();
			ActivityListView.ItemsSource = activities;

			ActivityListView.ItemSelected += (sender, e) => {
				var activity = e.SelectedItem as Activity;
				if (activity.targetObject.url != null) {
					app.SsoService.OpenUrl(activity.targetObject.url);
				}
			};

			CouldNotLoad.GestureRecognizers.Add (new TapGestureRecognizer {
				Command = new Command (() => {
					Reload();
				}),
				NumberOfTapsRequired = 1
			});

			MessagingCenter.Subscribe<Application> (Application.Current, "select_group", async(sender) => {
				var app = (App)Application.Current;
				setGroup(app.CurrentGroup);
			});

			MessagingCenter.Subscribe<Application> (Application.Current, "logout", async(sender) => {
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
				var service = app.WebService;
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
				app.CurrentGroup.MarkAsRead ();
				app.CurrentSite.groupsUnreadCount -= 1;

				var service = app.WebService;
				await service.MarkGroupAsRead (Group);
			}
		}
	}

}

