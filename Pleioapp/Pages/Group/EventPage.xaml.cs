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
		}

		public async void setGroup(Group group)
		{
			events.Clear ();

			var app = (App)App.Current;
			var service = app.webService;

			foreach (Event e in await service.GetEvents (group)) {
				events.Add (e);
			}
		}

	}
}

