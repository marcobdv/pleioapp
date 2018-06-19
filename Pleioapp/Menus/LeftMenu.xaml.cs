using System;
using Xamarin.Forms;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Linq;

namespace Pleioapp
{
	public partial class LeftMenu : ContentPage
	{
	    private readonly ObservableCollection<Site> _sites = new ObservableCollection<Site> ();
		public readonly ObservableCollection<Group> Groups = new ObservableCollection<Group>();
		public Site CurrentSite;

	    private readonly App _app = (App) Application.Current;

		public ListView Menu;
	    public ListView SiteMenu;
		public LeftMenu ()
		{
			InitializeComponent ();

			Menu = GroupsListView;
		    SiteMenu = SitesListView;
			BindingContext = _app.CurrentSite;

			GroupsListView.ItemsSource = Groups;
			SitesListView.ItemsSource = _sites;
			_sites.Add (_app.MainSite);

			SiteName.GestureRecognizers.Add (new TapGestureRecognizer {
				Command = new Command(ToggleSubsiteMenu)
			});
					
			

			

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

			MessagingCenter.Subscribe<Application> (Application.Current, "refresh_menu", async(sender) => {
				GetSites();
				GetGroups();
			});
		}




	    public void ToggleSubsiteMenu() {
			SitesListView.IsVisible = !SitesListView.IsVisible;
			GroupsListView.IsVisible = !GroupsListView.IsVisible;

	        SiteNameCaretDown.RotateXTo(SitesListView.IsVisible ? 180 : 0, 250);
	    }
			
		public async void OnLogout() {
			MessagingCenter.Send (Application.Current, "logout");

			await _app.PushService.DeregisterToken ();	

			_app.CurrentSite = null;
			_app.CurrentGroup = null;
			_app.AuthToken = null;

			var store = DependencyService.Get<ITokenStore> ();
			store.clearTokens ();

			MessagingCenter.Send (Application.Current, "login");
			MessagingCenter.Send (Application.Current, "refresh_menu");
			MessagingCenter.Send (Application.Current, "select_group");
		}

		public async Task GetGroups() {
			if (_app.CurrentSite == null) {
				Groups.Clear ();
				return;
			}

			CouldNotLoad.IsVisible = false;
			ActivityIndicator.IsVisible = true;

			try {
				var groupsAtService = await _app.WebService.GetGroups ();

				foreach (Group group in groupsAtService) {
					if (!Groups.Contains(group)) {
						Groups.Add (group);
					} else {
						Groups.First(g => g.guid == group.guid).activitiesUnreadCount = group.activitiesUnreadCount;
					}
				}

				foreach (Group group in Groups) {
					if (!groupsAtService.Contains(group)) {
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
				var sitesAtService = await _app.WebService.GetSites ();

				foreach (Site site in sitesAtService) {
					if (!_sites.Contains(site)) {
						_sites.Add(site);
					} else {
						_sites.First(s => s.guid == site.guid).groupsUnreadCount = site.groupsUnreadCount;
					}
				}

				foreach (Site site in _sites) {
					if (!sitesAtService.Contains(site) && site != _app.MainSite) {
						_sites.Remove(site);
					}
				}
			} catch (Exception e) {
				System.Diagnostics.Debug.WriteLine ("Catched exception " + e);
			}
		}
	}
}

