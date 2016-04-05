using System;
using System.Collections.Generic;

using Xamarin.Forms;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Linq;

namespace Pleioapp
{
	public partial class LeftMenu : ContentPage
	{

		ObservableCollection<Group> groups = new ObservableCollection<Group>();
		Dictionary<int, Site> indexToSite = new Dictionary<int, Site>();
		Dictionary<Site, int> siteToIndex = new Dictionary<Site, int>();
		App app = (App) App.Current;
		bool updatingSitePicker = false;
		bool updatingGroupPicker = false;

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
					MessagingCenter.Send<Xamarin.Forms.Application> (App.Current, "select_site");
					app.currentSite = indexToSite [SitePicker.SelectedIndex];
					await GetGroups();
				}
			};

			Menu.ItemSelected += (sender, e) =>  {
				if (updatingGroupPicker == false) {
					app.currentGroup = e.SelectedItem as Group;
					MessagingCenter.Send<Xamarin.Forms.Application> (App.Current, "select_group");
				}
			};

			CouldNotLoad.GestureRecognizers.Add (new TapGestureRecognizer {
				Command = new Command (async () => {
					await GetSites();
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
			MessagingCenter.Send<Xamarin.Forms.Application> (App.Current, "logout");

			app.pushService.DeregisterToken ();	

			app.currentSite = null;
			app.currentGroup = null;
			app.authToken = null;

			var store = DependencyService.Get<ITokenStore> ();
			store.clearTokens ();

			MessagingCenter.Send<Xamarin.Forms.Application> (App.Current, "login");
			MessagingCenter.Send<Xamarin.Forms.Application> (App.Current, "refresh_menu");
			MessagingCenter.Send<Xamarin.Forms.Application> (App.Current, "select_group");
		}

		public async Task GetGroups() {
			updatingGroupPicker = true;

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

				if (app.currentGroup != null) {
					var currentGroup = groups.FirstOrDefault(g => g.guid == app.currentGroup.guid);
					if (currentGroup != null) {
						GroupsListView.SelectedItem = currentGroup;
					}
				}
			} catch (Exception e) {
				CouldNotLoad.IsVisible = true;
				System.Diagnostics.Debug.WriteLine ("Catched exception " + e);
			}

			ActivityIndicator.IsVisible = false;
			updatingGroupPicker = false;
		}

		public async Task GetSites() {
			if (updatingSitePicker == false) {
				updatingSitePicker = true;
			} else {
				return;
			}

			int i = 1;
			while (SitePicker.Items.Count > 1) {
				SitePicker.Items.RemoveAt (1); 
				siteToIndex.Remove (indexToSite[i]);
				indexToSite.Remove (i);
				i += 1;
			}

			if (app.authToken == null) {
				updatingSitePicker = false;
				return;
			}

			int j = 1;
			var service = app.webService;

			try {
				var webSites = await service.GetSites ();

				foreach (Site site in webSites) {
					SitePicker.Items.Add (site.name);
					indexToSite.Add(j, site);
					siteToIndex.Add (site, j);
					j += 1;
				}
					
				if (app.currentSite != null) {
					var currentSite = siteToIndex.Keys.FirstOrDefault (s => s.guid == app.currentSite.guid);
					if (currentSite != null) {
						SitePicker.SelectedIndex = siteToIndex [currentSite];
					}
				}
			} catch (Exception e) {
				System.Diagnostics.Debug.WriteLine ("Catched exception " + e);
			}

			updatingSitePicker = false;
		}
	}
}

