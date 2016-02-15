using System;
using System.Collections.Generic;

using Xamarin.Forms;

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
				App.Current.Properties ["AuthToken"] = token;
				App.SuccesfulLoginAction.Invoke ();
			} else {
				await DisplayAlert ("Login", "Kon niet inloggen, controleer gebruikersnaam en wachtwoord", "Ok");
			}
		}
	}
}

