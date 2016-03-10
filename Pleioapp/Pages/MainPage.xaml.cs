using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace Pleioapp
{
	public partial class MainPage : MasterDetailPage
	{
		public GroupPage groupPage = new GroupPage ();
		public LeftMenu leftMenu = new LeftMenu();

		public MainPage ()
		{
			InitializeComponent ();

			leftMenu.Menu.ItemSelected += (sender, e) =>  {
				ViewGroup(e.SelectedItem as Group);
			};

			Master = leftMenu;

			var navigation = new NavigationPage (groupPage);
			Detail = new NavigationPage(groupPage);
		}

		void ViewGroup (Group group)
		{
			groupPage.setGroup (group);
		}
	}
}

