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

				MessagingCenter.Send<Xamarin.Forms.Application> (App.Current, "login_succesful");
			} else {
				await DisplayAlert ("Login", "Kon niet inloggen, controleer gebruikersnaam en wachtwoord", "Ok");
			}
		}
	}
}

