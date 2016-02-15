using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace Pleioapp
{
	public class WebService
	{
		private HttpClient _client;
		private AuthToken _token;

		public WebService ()
		{
			_client = new HttpClient ();
			_token = (AuthToken) App.Current.Properties ["AuthToken"];
			_client.DefaultRequestHeaders.Add ("Authorization", string.Format ("Bearer {0}", _token.accessToken));
		}

		public async Task<List<Group>> GetGroups ()
		{
			var uri = new Uri (Constants.Url + "/api/groups/mine");
			var response = await _client.GetAsync (uri);
			var content = await response.Content.ReadAsStringAsync ();

			if (response.IsSuccessStatusCode) {
				return JsonConvert.DeserializeObject <List<Group>> (content);
			}
			System.Diagnostics.Debug.WriteLine ("Get groups succesful: " + content);
			return null;
		}
	}
}

