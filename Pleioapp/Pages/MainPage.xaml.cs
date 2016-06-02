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

			Master = leftMenu;
			leftMenu.IsVisible = true;
			Detail = new NavigationPage(groupPage);
		}
	}
}

