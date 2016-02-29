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

			indexToSite.Add (0, app.currentSite);
			SitePicker.Items.Add (app.currentSite.name);

			SitePicker.SelectedIndex = 0;
			SitePicker.SelectedIndexChanged += (sender, args) => {
				int selectedIndex;

				if (SitePicker.SelectedIndex == -1) {
					selectedIndex = 0;
				} else {
					selectedIndex = SitePicker.SelectedIndex;
				}

				app.currentSite = indexToSite [selectedIndex];
				System.Diagnostics.Debug.WriteLine ("Selected site " + app.currentSite.name);
				MessagingCenter.Send<Xamarin.Forms.Application> (App.Current, "refresh_groups");
			};
		}

		public async Task GetGroups() {
			var app = (App)App.Current;
			var service = app.webService;

			var webGroups = await service.GetGroups ();

			groups.Clear ();
			foreach (Group group in webGroups) {
				groups.Add (group);
			}
		}

		public async Task GetSites() {
			var app = (App)App.Current;
			var service = app.webService;

			var webSites = await service.GetSites ();

			int i = 1;
			foreach (Site site in webSites) {
				SitePicker.Items.Add (site.name);
				indexToSite.Add(i, site);
				i += 1;
			}
		}
	}
}

