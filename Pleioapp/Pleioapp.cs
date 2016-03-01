using System;

using Xamarin.Forms;
using System.Threading.Tasks;

namespace Pleioapp
{
	public class App : Application
	{
		static MainPage RootPage;
		public Site currentSite;
		public Site mainSite;
		public AuthToken authToken;

		public WebService webService;
		public IPushService pushService;
		public LoginService loginService;

		public App ()
		{
			mainSite = new Site ();
			mainSite.name = "Pleio hoofdniveau";
			mainSite.url = Constants.Url;
			currentSite = mainSite;

			webService = new WebService ();
			pushService = DependencyService.Get<IPushService> ();
			loginService = new LoginService ();

			RootPage = new MainPage ();
			MainPage = RootPage;

			MessagingCenter.Subscribe<Xamarin.Forms.Application> (App.Current, "refresh_sites", async(sender) => {
				await RootPage.leftMenu.GetSites();
			});

			MessagingCenter.Subscribe<Xamarin.Forms.Application> (App.Current, "refresh_groups", async(sender) => {
				await RootPage.leftMenu.GetGroups();
			});

			MessagingCenter.Subscribe<Xamarin.Forms.Application> (App.Current, "refresh_push", async(sender) => {
				if (pushService.GetToken() == null) {
					pushService.RequestToken();
				} else {
					await pushService.RegisterToken();
				}
			});

			MessagingCenter.Subscribe<Xamarin.Forms.Application> (App.Current, "trigger_login", async(sender) => {
				await RootPage.Navigation.PushModalAsync (new LoginPage ());
			});

			MessagingCenter.Subscribe<Xamarin.Forms.Application> (App.Current, "login_succesful", async(sender) => {
				await RootPage.Navigation.PopModalAsync();

				MessagingCenter.Send<Xamarin.Forms.Application> (App.Current, "refresh_push");
				MessagingCenter.Send<Xamarin.Forms.Application> (App.Current, "refresh_sites");
				MessagingCenter.Send<Xamarin.Forms.Application> (App.Current, "refresh_groups");
			});	

			MessagingCenter.Subscribe<Xamarin.Forms.Application> (App.Current, "refresh_token", async(sender) => {
				await RefreshToken();
			});

			var token = DependencyService.Get<ITokenStore> ().getToken();

			if (token == null) {
				MessagingCenter.Send<Xamarin.Forms.Application> (App.Current, "trigger_login");
			} else {
				authToken = token;
				MessagingCenter.Send<Xamarin.Forms.Application> (App.Current, "refresh_push");
				MessagingCenter.Send<Xamarin.Forms.Application> (App.Current, "refresh_sites");
				MessagingCenter.Send<Xamarin.Forms.Application> (App.Current, "refresh_groups");
			}

			pushService.SetBadgeNumber (0);
		}

		public async Task<bool> RefreshToken()
		{
			var store = DependencyService.Get<ITokenStore> ();
			var new_token = await loginService.RefreshToken (store.getToken());

			if (new_token != null) {
				authToken = new_token;
				store.saveToken (new_token);
				return true;
			} else {
				MessagingCenter.Send<Xamarin.Forms.Application> (App.Current, "trigger_login");
			}

			return false;
		}
			
		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			pushService.SetBadgeNumber (0);
			MessagingCenter.Send<Xamarin.Forms.Application> (App.Current, "refresh_groups");
		}
	}
}

