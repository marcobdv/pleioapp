using System;
using System.Collections.Generic;

using Xamarin.Forms;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace Pleioapp
{
	public partial class LeftMenu : ContentPage
	{

		ObservableCollection<Group> groups = new ObservableCollection<Group>();
		Dictionary<int, Site> indexToSite = new Dictionary<int, Site>();
		App app = (App) App.Current;

		public ListView Menu;

		public LeftMenu ()
		{
			InitializeComponent ();

			Menu = GroupsListView;
			GroupsListView.ItemsSource = groups;

			indexToSite.Add (0, app.mainSite);
			SitePicker.Items.Add (app.mainSite.name);
			SitePicker.SelectedIndex = 0;

			SitePicker.SelectedIndexChanged += (sender, args) => {
				if (SitePicker.SelectedIndex != -1) {
					app.currentSite = indexToSite [SitePicker.SelectedIndex];
					MessagingCenter.Send<Xamarin.Forms.Application> (App.Current, "refresh_groups");
				}

			};

			CouldNotLoad.GestureRecognizers.Add (new TapGestureRecognizer {
				Command = new Command (async () => {
					await GetGroups();
				}),
				NumberOfTapsRequired = 1
			});

			LogoutButton.Clicked += (s, e) => {
				OnLogout();
			};
		}

		public void OnLogout() {
			MessagingCenter.Send<Xamarin.Forms.Application> (App.Current, "logout");

			var store = DependencyService.Get<ITokenStore> ();
			store.clearTokens ();

			MessagingCenter.Send<Xamarin.Forms.Application> (App.Current, "trigger_login");
		}

		public async Task GetGroups() {
			CouldNotLoad.IsVisible = false;
			ActivityIndicator.IsVisible = true;

			var app = (App)App.Current;
			var service = app.webService;

			try {
				var webGroups = await service.GetGroups ();
				groups.Clear ();
				foreach (Group group in webGroups) {
					groups.Add (group);
				}
			} catch (Exception e) {
				CouldNotLoad.IsVisible = true;
				System.Diagnostics.Debug.WriteLine ("Catched exception " + e);
				Xamarin.Insights.Report (e);
			}

			ActivityIndicator.IsVisible = false;
		}

		public async Task GetSites() {
			var app = (App)App.Current;
			var service = app.webService;

			var webSites = await service.GetSites ();

			for (int i = 1; i <= (SitePicker.Items.Count - 1); i++) {
				SitePicker.Items.RemoveAt (i); 
				indexToSite.Remove (i);
			}
				
			int j = 1;
			foreach (Site site in webSites) {
				SitePicker.Items.Add (site.name);
				indexToSite.Add(j, site);
				j += 1;
			}
		}
	}
}

