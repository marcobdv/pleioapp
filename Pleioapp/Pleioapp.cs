using System;

using Xamarin.Forms;

namespace Pleioapp
{
	public class App : Application
	{
		static MainPage RootPage;

		public App ()
		{
			RootPage = new MainPage ();
			MainPage = RootPage;

			Properties.Add ("WebService", new WebService ());

			var token = DependencyService.Get<ITokenStore> ().getToken();
			if (token != null) {
				Properties.Add ("AuthToken", token);
			} else {
				RootPage.Navigation.PushModalAsync (new LoginPage ());
			}
		}
			
		public static Action SuccesfulLoginAction
		{
			get {
				return new Action (() => {
					RootPage.leftMenu.GetGroups();
					RootPage.Navigation.PopModalAsync();
				});
			}
		}

		protected override void OnStart ()
		{
			// Handle when your app starts
		}

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}
	}
}

