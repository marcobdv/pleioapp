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

	    protected override void OnAppearing()
	    {
	        base.OnAppearing();

	        LoadAccessToken();
	    }

        private void LoadAccessToken()
        {
            var token = DependencyService.Get<ITokenStore>().getToken();

            if (token != null)
            {
                MessagingCenter.Send<Xamarin.Forms.Application>(App.Current, "refresh_menu");
            }
            else {
                MessagingCenter.Send<Xamarin.Forms.Application>(App.Current, "login");
            }
        }
    }
}

