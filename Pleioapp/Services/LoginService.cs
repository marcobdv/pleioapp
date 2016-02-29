using System;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Pleioapp
{
	public class LoginService
	{
		private HttpClient client;

		public LoginService () 
		{
			client = new HttpClient ();
		}

		public async Task<AuthToken> Login (string username, string password)
		{
			var uri = new Uri (Constants.Url + "oauth/v2/token");
			var formContent = new FormUrlEncodedContent (new [] {
				new KeyValuePair<string,string>("client_id", Constants.ClientId),
				new KeyValuePair<string,string>("client_secret", Constants.ClientSecret),
				new KeyValuePair<string,string>("grant_type", "password"),
				new KeyValuePair<string,string>("username", username),
				new KeyValuePair<string,string>("password", password),
			});

			var response = await client.PostAsync (uri, formContent);
			var content = await response.Content.ReadAsStringAsync ();

			if (response.IsSuccessStatusCode) {
				System.Diagnostics.Debug.WriteLine ("Login succesful: " + content);
				return JsonConvert.DeserializeObject <AuthToken> (content);
			} else {
				System.Diagnostics.Debug.WriteLine ("A problem occured during login: " + content);
				return null;
			}
		}

		public async Task<AuthToken> RefreshToken(AuthToken currentToken)
		{
			var uri = new Uri (Constants.Url + "oauth/v2/token");
			var formContent = new FormUrlEncodedContent (new [] {
				new KeyValuePair<string,string>("client_id", Constants.ClientId),
				new KeyValuePair<string,string>("client_secret", Constants.ClientSecret),
				new KeyValuePair<string,string>("grant_type", "refresh_token"),
				new KeyValuePair<string,string>("refresh_token", currentToken.refreshToken),
			});

			var response = await client.PostAsync (uri, formContent);
			var content = await response.Content.ReadAsStringAsync ();

			if (response.IsSuccessStatusCode) {
				System.Diagnostics.Debug.WriteLine ("Refresh token succesful: " + content);
				return JsonConvert.DeserializeObject <AuthToken> (content);
			} else {
				System.Diagnostics.Debug.WriteLine ("A problem occured during token refresh, triggering relogin: " + content);
				MessagingCenter.Send<Xamarin.Forms.Application> (App.Current, "trigger_login");
				return null;
			}

		}
	}
}

