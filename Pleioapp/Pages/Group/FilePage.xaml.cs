using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace Pleioapp
{
	public partial class FilePage : ContentPage
	{
		ObservableCollection<FSObject> files = new ObservableCollection<FSObject>();
		App app = (App)App.Current;
		Group Group;

		public FilePage()
		{
			InitializeComponent();
			FileListView.ItemsSource = files;

			FileListView.ItemSelected += (sender, e) =>
			{
				var selectedEvent = e.SelectedItem as FSObject;
				if (selectedEvent.url != null)
				{
					app.ssoService.OpenUrl(selectedEvent.url);
				}
			};

			CouldNotLoad.GestureRecognizers.Add(new TapGestureRecognizer
			{
				Command = new Command(() =>
				{
					Reload();
				}),
				NumberOfTapsRequired = 1
			});

			MessagingCenter.Subscribe<Xamarin.Forms.Application>(App.Current, "select_group", async (sender) =>
			{
				var app = (App)App.Current;
				setGroup(app.currentGroup);
			});
		}

		public async void Reload()
		{
			CouldNotLoad.IsVisible = false;
			NoItems.IsVisible = false;
			files.Clear();

			var app = (App)App.Current;
			var service = app.webService;

			try
			{
				var webFiles = await service.GetFiles(Group);

				foreach (FSObject e in webFiles)
				{
					files.Add(e);
				}

				if (files.Count == 0)
				{
					NoItems.IsVisible = true;
				}
			}
			catch (Exception e)
			{
				CouldNotLoad.IsVisible = true;
				System.Diagnostics.Debug.WriteLine("Catched exception " + e);
			}
		}

		public async void setGroup(Group group)
		{
			Group = group;
			if (group == null)
			{
				files.Clear();
				return;
			}

			Reload();
		}
	}
}

