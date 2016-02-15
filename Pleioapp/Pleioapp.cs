using System;

using Xamarin.Forms;

namespace Pleioapp
{
	public class App : Application
	{
		static RootPage _RootPage;

		public App ()
		{
			_RootPage = new RootPage ();
			MainPage = _RootPage;

			// @todo: only on login
			if (!Properties.ContainsKey("AuthToken")) {
				_RootPage.Navigation.PushModalAsync (new LoginPage ());
			}
		}
			
		public static Action SuccesfulLoginAction
		{
			get {
				return new Action (() => {
					_RootPage.Navigation.PopModalAsync();
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

