using System;
using Xamarin.Forms;

namespace Pleioapp
{
	public partial class LoginPage : ContentPage
	{
		public LoginPage ()
		{
			InitializeComponent ();
			var browserService = DependencyService.Get<IBrowserService> ();

			SetupForgotPasswordGesture(browserService);

			SetupAccountRegistrationGesture(browserService);
		    var app = (App) Application.Current;

            LoadMainSites(app);
		}

	    private void SetupForgotPasswordGesture(IBrowserService browserService)
	    {
	        ForgotPassword.GestureRecognizers.Add(new TapGestureRecognizer
	        {
	            Command = new Command(() => { browserService.OpenUrl(Constants.RootUrl+"forgotpassword"); }),
	            NumberOfTapsRequired = 1
	        });
	    }

	    private void SetupAccountRegistrationGesture(IBrowserService browserService)
	    {
	        RegisterAccount.GestureRecognizers.Add(new TapGestureRecognizer
	        {
	            Command = new Command(() => { browserService.OpenUrl(Constants.RootUrl+"register"); }),
	            NumberOfTapsRequired = 1
	        });
	    }

	    private void LoadMainSites(App app)
	    {
	        foreach (var site in app.AvailbleSites)
	        {
	            SiteSelection.Items.Add(site.name);
	        }
	        SiteSelection.SelectedIndex = 0;
	    }

	    private async void OnLogin(object sender, EventArgs e)
		{
			var service = new LoginService ();
			var token = await service.Login (username.Text, password.Text);

			if (token != null)
            {
				var store = DependencyService.Get<ITokenStore> ();
				var app = (App) Application.Current;
			    token.mainSiteName = app.MainSite.name;
				store.saveToken (token);

				app.AuthToken = token;
				app.CurrentSite = app.MainSite;
				app.WebService = new WebService ();

				MessagingCenter.Send (Application.Current, "login_succesful");
				MessagingCenter.Send (Application.Current, "refresh_menu");

				await app.MainPage.Navigation.PopModalAsync ();
			}
            else
            {
				await DisplayAlert ("Login", "Kon niet inloggen, controleer je gebruikersnaam en wachtwoord.", "Ok");
			}
		}

	    private void SiteSelection_OnSelectedIndexChanged(object sender, EventArgs e)
	    {
	        var app = (App)Application.Current;
	        app.SwitchMainSite(SiteSelection.Items[SiteSelection.SelectedIndex]);
	    }
	}
}

