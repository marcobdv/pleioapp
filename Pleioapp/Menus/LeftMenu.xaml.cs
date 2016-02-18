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

		public async Task GetGroups() {
			var service = (WebService) App.Current.Properties ["WebService"];

			var webGroups = await service.GetGroups ();
			groups.Clear ();
			foreach (Group group in webGroups) {
				groups.Add (group);
			}
		}
	}
}

