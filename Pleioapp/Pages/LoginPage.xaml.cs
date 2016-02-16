using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace Pleioapp
{
	public partial class LoginPage : ContentPage
	{
		ITokenStore store;


		public LoginPage ()
		{
			InitializeComponent ();

			store = DependencyService.Get<ITokenStore> ();

			var token = store.getToken ();
			if (token != null) {
				App.Current.Properties ["AuthToken"] = token;
				App.SuccesfulLoginAction.Invoke ();
			}
		}

		async void OnLogin(object sender, EventArgs e)
		{
			var service = new LoginService ();
			AuthToken token = await service.Login (username.Text, password.Text);

			if (token != null) {
				var store = DependencyService.Get<ITokenStore> ();
				store.saveToken (token);
				App.Current.Properties ["AuthToken"] = token;
				App.SuccesfulLoginAction.Invoke ();
			} else {
				await DisplayAlert ("Login", "Kon niet inloggen, controleer gebruikersnaam en wachtwoord", "Ok");
			}
		}
	}
}

