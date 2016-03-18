using System;
using System.Collections.Generic;

using Xamarin.Forms;
using System.Collections.ObjectModel;

namespace Pleioapp
{
	public partial class EventPage : ContentPage
	{
		ObservableCollection<Event> events = new ObservableCollection<Event>();
		App app = (App) App.Current;
		Event SelectedItem = null;
		Group Group;

		public EventPage ()
		{
			InitializeComponent ();
			EventListView.ItemsSource = events;
			EventListView.ItemTapped += (sender, e) => {
				if (EventListView.SelectedItem == SelectedItem) {
					if (SelectedItem.url != null) {
						app.ssoService.OpenUrl(SelectedItem.url);
					}
				}

				SelectedItem = (Event) EventListView.SelectedItem;
			};

			CouldNotLoad.GestureRecognizers.Add (new TapGestureRecognizer {
				Command = new Command (() => {
					Reload();
				}),
				NumberOfTapsRequired = 1
			});
		}

		public async void Reload()
		{
			CouldNotLoad.IsVisible = false;
			NoItems.IsVisible = false;
			events.Clear ();

			var app = (App)App.Current;
			var service = app.webService;

			try {
				var webEvents = await service.GetEvents (Group);

				foreach (Event e in webEvents) {
					events.Add (e);
				}

				if (events.Count == 0) {
					NoItems.IsVisible = true;
				}
			} catch (Exception e) {
				CouldNotLoad.IsVisible = true;
				System.Diagnostics.Debug.WriteLine ("Catched exception " + e);
				Xamarin.Insights.Report (e);
			}
		}

		public void setGroup(Group group)
		{
			Group = group;
			if (group == null) {
				events.Clear ();
				return;
			}

			Reload ();
		}

	}
}

