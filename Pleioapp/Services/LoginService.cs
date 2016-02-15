using System;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

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
	}
}

