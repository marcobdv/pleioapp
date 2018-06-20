using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using System.Threading.Tasks;

namespace Pleioapp
{
    public class App : Application
    {
        public MainPage RootPage;
        public Site MainSite;
        public Site CurrentSite;
        public Group CurrentGroup;
        public AuthToken AuthToken;

        public WebService WebService;
        public IPushService PushService;
        public ISSOService SsoService;
        public LoginService LoginService;

        private bool _isRefreshingToken;

        public App()
        {
            RootPage = new MainPage();
            MainPage = RootPage;
            RegisterServices();
            RegisterMainSite();
            LoadAccessToken();
            PushService.SetBadgeNumber(0);
            SubscribeToMessages();
        }

        private void RegisterMainSite()
        {
            AvailbleSites = Task.Run(async () => await WebService.GetMainSites()).Result; ;
            MainSite = AvailbleSites.First();
            CurrentSite = MainSite;
        }

        public List<Site> AvailbleSites { get; private set; }

        private void RegisterServices()
        {
            WebService = new WebService();
            LoginService = new LoginService();
            PushService = DependencyService.Get<IPushService>();
            SsoService = DependencyService.Get<ISSOService>();
        }

        private void SubscribeToMessages()
        {
            MessagingCenter.Subscribe<Application>(Current, "refresh_access_token",
                async (sender) => { await RefreshToken(); });

            MessagingCenter.Subscribe<Application>(Current, "login", (sender) => { ShowLogin(); });

            MessagingCenter.Subscribe<Application>(Current, "login_succesful", (sender) => { RefreshPushToken(); });
        }

        private void LoadAccessToken()
        {
            var token = DependencyService.Get<ITokenStore>().getToken();
            if (token != null)
            {
                if (token.mainSiteName != null)
                {
                    SwitchMainSite(token.mainSiteName);
                }
                AuthToken = token;
                RefreshPushToken();
                MessagingCenter.Send(Current, "refresh_menu");
            }
            else
            {
                ShowLogin();
            }
        }

        private async void RefreshPushToken()
        {
            await Task.Run(() =>
            {
                try
                {
                    if (PushService.GetToken() == null)
                    {
                        PushService.RequestToken();
                    }
                    else
                    {
                        //android will register through an intentservice
                        if (Device.OS == TargetPlatform.Android)
                            return;

                        //iOS and other platforms registration
                        PushService.RegisterToken();
                    }
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine("Catched exception " + e);
                    Xamarin.Insights.Report(e);
                }
            });
        }

        private async void ShowLogin()
		{
			await RootPage.Navigation.PushModalAsync (new LoginPage ());
		}

		public async Task<bool> RefreshToken()
		{
			if (_isRefreshingToken) {
				return true;
			} else {
				_isRefreshingToken = true;
			}

			var store = DependencyService.Get<ITokenStore> ();
			var newToken = await LoginService.RefreshToken (store.getToken());
		   
			

			if (newToken != null)
			{
			    newToken.mainSiteName = MainSite.name;
                UpdateToken(newToken, store);
			    _isRefreshingToken = false;
                return true;
			} else {
			    _isRefreshingToken = false;
                ShowLogin ();
                return false;
			}
		}

        private void UpdateToken(AuthToken newToken, ITokenStore store)
        {
            AuthToken = newToken;
            CurrentSite = MainSite;
            WebService = new WebService();

            store.saveToken(newToken);

            MessagingCenter.Send(Current, "login_succesful");
            MessagingCenter.Send(Current, "refresh_menu");
        }

        protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			MessagingCenter.Send (Current, "refresh_menu");
			PushService.SetBadgeNumber (0);
		}

        public void SwitchMainSite(string siteSelectionItem)
        {
            var selectedSite = AvailbleSites.FirstOrDefault(x => x.name.Equals(siteSelectionItem));
            if (selectedSite != null)
            {
                MainSite = selectedSite;
            }
        }
    }
}

