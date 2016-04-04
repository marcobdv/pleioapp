using System;
using Pleioapp.iOS;
using Xamarin.Forms;
using Foundation;
using System.Threading.Tasks;

[assembly: Dependency(typeof(SSOService))]
namespace Pleioapp.iOS
{
	public class SSOService : ISSOService
	{
		WebService WebService;
		SSOToken LoginToken;
		bool Loading = false;
		double TokenExpiry = 0;
		double LoginExpiry = 0;
		App App;

		public SSOService() {
			App = (App) App.Current;
			WebService = App.webService;
		}
			
		public void Expire() {
			LoginToken = null;
			TokenExpiry = 0;
		}

		public async Task<bool> LoadToken() {
			System.Diagnostics.Debug.WriteLine ("[SSO] requesting SSO token");
			LoginToken = await WebService.GenerateToken ();
			if (LoginToken != null) {
				TokenExpiry = UnixTimestamp () + LoginToken.expiry;
			}
			return true;
		}

		private double UnixTimestamp() {
			return DateTime.UtcNow.Subtract (new DateTime (1970, 1, 1)).TotalSeconds;
		}
			
		public async void OpenUrl(string Url) {
			string loadUrl;

			if (Loading) {
				return; // prevent double click..
			} else {
				Loading = true;
			}
				
			if (UnixTimestamp() > LoginExpiry && UnixTimestamp() > TokenExpiry) {
				await LoadToken ();
			}

			if (LoginToken != null) {
				loadUrl = App.currentSite.url + "api/users/me/login_token?user_guid=" + LoginToken.userGuid + "&token=" + LoginToken.token + "&redirect_url=" + Url;
				LoginToken = null;

				LoginExpiry = UnixTimestamp () + 60 * 60;
			} else {
				loadUrl = Url; // could not retrieve token
			}

			Loading = false;

			System.Diagnostics.Debug.WriteLine ("[SSO] opening: " + loadUrl);
			UIKit.UIApplication.SharedApplication.OpenUrl (new NSUrl (loadUrl));
		}
	}
}

