using System;

using Xamarin.Forms;

namespace Pleioapp
{
	public class App : Application
	{
		static MainPage RootPage;

		WebService webService;
		IPushService pushService;
		AuthToken token;

		public App ()
		{
			webService = new WebService ();
			Properties.Add ("WebService", webService);

			pushService = DependencyService.Get<IPushService> ();
			Properties.Add ("PushService", pushService);

			RootPage = new MainPage ();
			MainPage = RootPage;

			MessagingCenter.Subscribe<Xamarin.Forms.Application> (App.Current, "refresh_content", async(sender) => {
				await RootPage.leftMenu.GetGroups();
			});

			MessagingCenter.Subscribe<Xamarin.Forms.Application> (App.Current, "login_succesful", async(sender) => {
				if (pushService.GetToken() == null) {
					pushService.RequestToken();
				} else {
					await pushService.RegisterToken();
				}
					
				await RootPage.leftMenu.GetGroups();
			});				
		}
			
		public static Action SuccesfulLoginAction
		{
			get {
				return new Action (() => {
					RootPage.Navigation.PopModalAsync();
				});
			}
		}

		protected async override void OnStart ()
		{
			token = DependencyService.Get<ITokenStore> ().getToken();

			if (token == null) {
				await RootPage.Navigation.PushModalAsync (new LoginPage ());
			} else {
				Properties.Add ("AuthToken", token);
				MessagingCenter.Send<Xamarin.Forms.Application> (App.Current, "login_succesful");
			}

			pushService.SetBadgeNumber (0);
		}

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			pushService.SetBadgeNumber (0);
		}
	}
}

