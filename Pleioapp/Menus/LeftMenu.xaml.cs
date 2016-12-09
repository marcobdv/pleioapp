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
		ObservableCollection<Site> Sites = new ObservableCollection<Site> ();
		ObservableCollection<Group> Groups = new ObservableCollection<Group>();
		public Site CurrentSite;

		App app = (App) App.Current;

		public ListView Menu;
		public LeftMenu ()
		{
			InitializeComponent ();

			Menu = GroupsListView;
			BindingContext = app.CurrentSite;

			GroupsListView.ItemsSource = Groups;
			SitesListView.ItemsSource = Sites;
			Sites.Add (app.MainSite);

			SiteName.GestureRecognizers.Add (new TapGestureRecognizer {
				Command = new Command(() => ToggleSubsiteMenu())
			});
					
			SitesListView.ItemSelected += async(sender, e) => {
				app.CurrentSite = e.SelectedItem as Site;
				BindingContext = app.CurrentSite;
				Groups.Clear();
				ToggleSubsiteMenu ();
				await GetGroups ();
			};

			Menu.ItemSelected += (sender, e) =>  {
				app.CurrentGroup = e.SelectedItem as Group;
				MessagingCenter.Send<Xamarin.Forms.Application> (App.Current, "select_group");
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
			
		public void ToggleSubsiteMenu() {
			SitesListView.IsVisible = !SitesListView.IsVisible;
			GroupsListView.IsVisible = !GroupsListView.IsVisible;

			if (SitesListView.IsVisible) {
				SiteNameCaretDown.RotateXTo(180, 250);
			} else {
				SiteNameCaretDown.RotateXTo(0, 250);
			}
		}
			
		public async void OnLogout() {
			MessagingCenter.Send<Xamarin.Forms.Application> (App.Current, "logout");

			app.PushService.DeregisterToken ();	

			app.CurrentSite = null;
			app.CurrentGroup = null;
			app.AuthToken = null;

			var store = DependencyService.Get<ITokenStore> ();
			store.clearTokens ();

			MessagingCenter.Send<Xamarin.Forms.Application> (App.Current, "login");
			MessagingCenter.Send<Xamarin.Forms.Application> (App.Current, "refresh_menu");
			MessagingCenter.Send<Xamarin.Forms.Application> (App.Current, "select_group");
		}

		public async Task GetGroups() {
			if (app.CurrentSite == null) {
				Groups.Clear ();
				return;
			}

			CouldNotLoad.IsVisible = false;
			ActivityIndicator.IsVisible = true;

			try {
				var GroupsAtService = await app.WebService.GetGroups ();

				foreach (Group group in GroupsAtService) {
					if (!Groups.Contains(group)) {
						Groups.Add (group);
					} else {
						Groups.First(g => g.guid == group.guid).activitiesUnreadCount = group.activitiesUnreadCount;
					}
				}

				foreach (Group group in Groups) {
					if (!GroupsAtService.Contains(group)) {
						Groups.Remove(group);
					}
				}

			} catch (Exception e) {
				CouldNotLoad.IsVisible = true;
				System.Diagnostics.Debug.WriteLine ("Catched exception " + e);
			}

			ActivityIndicator.IsVisible = false;
		}

		public async Task GetSites() {
			try {
				var SitesAtService = await app.WebService.GetSites ();

				foreach (Site site in SitesAtService) {
					if (!Sites.Contains(site)) {
						Sites.Add(site);
					} else {
						Sites.First(s => s.guid == site.guid).groupsUnreadCount = site.groupsUnreadCount;
					}
				}

				foreach (Site site in Sites) {
					if (!SitesAtService.Contains(site) && site != app.MainSite) {
						Sites.Remove(site);
					}
				}
			} catch (Exception e) {
				System.Diagnostics.Debug.WriteLine ("Catched exception " + e);
			}
		}
	}
}

