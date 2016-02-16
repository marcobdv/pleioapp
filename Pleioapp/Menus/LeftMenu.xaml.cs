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
		public ListView Menu;

		public LeftMenu ()
		{
			InitializeComponent ();

			Menu = GroupsListView;
			GroupsListView.ItemsSource = groups;
		}

		protected override async void OnAppearing()
		{
			//@todo: fix this, this refreshes on every menu load!
			if (App.Current.Properties.ContainsKey("AuthToken")) {
				await GetGroups ();
			}
		}

		public async Task GetGroups() {
			var service = (WebService) App.Current.Properties ["WebService"];

			groups.Clear ();
			foreach (Group group in await service.GetGroups ()) {
				groups.Add (group);
			}
		}

	}

	public class MenuItem
	{ 
		
	}
}

