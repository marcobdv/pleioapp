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

			SitePicker.SelectedIndexChanged += async(sender, args) => {
				if (SitePicker.SelectedIndex != -1) {
					app.currentSite = indexToSite [SitePicker.SelectedIndex];
					await GetGroups();
				}
			};

			Menu.ItemSelected += (sender, e) =>  {
				app.currentGroup = e.SelectedItem as Group;
				MessagingCenter.Send<Xamarin.Forms.Application> (App.Current, "select_group");
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

			MessagingCenter.Subscribe<Xamarin.Forms.Application> (App.Current, "refresh_menu", async(sender) => {
				GetSites();
				GetGroups();
			});
		}

		public async void OnLogout() {
			await app.pushService.DeregisterToken ();

			app.currentSite = null;
			app.currentGroup = null;
			app.authToken = null;

			var store = DependencyService.Get<ITokenStore> ();
			store.clearTokens ();

			var service = app.webService;

			MessagingCenter.Send<Xamarin.Forms.Application> (App.Current, "login");
			MessagingCenter.Send<Xamarin.Forms.Application> (App.Current, "refresh_menu");
		}

		public async Task GetGroups() {
			if (app.currentSite == null) {
				groups.Clear ();
				return;
			}

			CouldNotLoad.IsVisible = false;
			ActivityIndicator.IsVisible = true;

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
			for (int i = 1; i <= (SitePicker.Items.Count - 1); i++) {
				SitePicker.Items.RemoveAt (i); 
				indexToSite.Remove (i);
			}

			if (app.authToken == null) {
				return;
			}

			int j = 1;
			var service = app.webService;
			var webSites = await service.GetSites ();
			foreach (Site site in webSites) {
				SitePicker.Items.Add (site.name);
				indexToSite.Add(j, site);
				j += 1;
			}
		}
	}
}

