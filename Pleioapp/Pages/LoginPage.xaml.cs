using System;
using System.Collections.Generic;

using Xamarin.Forms;
using System.Globalization;
using System.Collections;

namespace Pleioapp
{
	public partial class LoginPage : ContentPage
	{
		public LoginPage ()
		{
			InitializeComponent ();
			var browserService = DependencyService.Get<IBrowserService> ();

			ForgotPassword.GestureRecognizers.Add (new TapGestureRecognizer {
				Command = new Command (() => {
					browserService.OpenUrl("https://www.pleio.nl/forgotpassword");
				}),
				NumberOfTapsRequired = 1
			});

			RegisterAccount.GestureRecognizers.Add (new TapGestureRecognizer {
				Command = new Command (() => {
					browserService.OpenUrl("https://www.pleio.nl/register");
				}),
				NumberOfTapsRequired = 1
			});

		}

		async void OnLogin(object sender, EventArgs e)
		{
			var service = new LoginService ();
			AuthToken token = await service.Login (username.Text, password.Text);

			if (token != null) {
				var store = DependencyService.Get<ITokenStore> ();
				var app = (App) App.Current;

				store.saveToken (token);

				app.authToken = token;
				app.currentSite = app.mainSite;

				MessagingCenter.Send<Xamarin.Forms.Application> (App.Current, "refresh_push_token");
				MessagingCenter.Send<Xamarin.Forms.Application> (App.Current, "refresh_menu");

				await app.MainPage.Navigation.PopModalAsync ();
			} else {
				await DisplayAlert ("Login", "Kon niet inloggen, controleer je gebruikersnaam en wachtwoord.", "Ok");
			}
		}
	}
}

