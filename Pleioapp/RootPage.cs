using System;

using Xamarin.Forms;

namespace Pleioapp
{
	public class RootPage : MasterDetailPage
	{
		public RootPage ()
		{
			var mainPage = new MainPage ();
			mainPage.Menu.ItemSelected += (sender, e) => NavigateTo (e.SelectedItem as MenuItem);

			Master = mainPage;
			Detail = new NavigationPage (new GroupPage ());
		}

		void NavigateTo (MenuItem menu)
		{
			Page displayPage = (Page)Activator.CreateInstance (menu.TargetType);

			Detail = new NavigationPage (displayPage);

			IsPresented = false;
		}
	}
}


