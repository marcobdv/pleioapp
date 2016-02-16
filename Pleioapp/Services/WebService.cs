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
		private AuthToken _token;

		private HttpClient _client;
		private HttpClient getClient()
		{
			if (_client == null) {
				if (!App.Current.Properties.ContainsKey ("AuthToken")) {
					throw new Exception ("Tried to perform a protected API call but there is no OAuth2 authentication token initialized.");
				}

				_token = (AuthToken) App.Current.Properties ["AuthToken"];
				_client = new HttpClient ();
				_client.DefaultRequestHeaders.Add ("Authorization", string.Format ("Bearer {0}", _token.accessToken));
			}

			return _client;
		}

		public async Task<List<Group>> GetGroups ()
		{
			var uri = new Uri (Constants.Url + "/api/groups/mine");
			var response = await getClient().GetAsync (uri);
			var content = await response.Content.ReadAsStringAsync ();

			if (response.IsSuccessStatusCode) {
				System.Diagnostics.Debug.WriteLine ("Get groups succesful: " + content);

				var list = JsonConvert.DeserializeObject <PaginatedGroupList> (content);
				return list.entities;
			} else {
				return null;
			}
		}

		public async Task<List<Activity>> GetActivities (Group group)
		{
			var uri = new Uri (Constants.Url + "/api/groups/" + group.guid + "/activities");
			var response = await getClient().GetAsync (uri);
			var content = await response.Content.ReadAsStringAsync ();

			if (response.IsSuccessStatusCode) {
				System.Diagnostics.Debug.WriteLine ("Get activities succesful: " + content);

				var list = JsonConvert.DeserializeObject <PaginatedActivityList> (content);
				return list.entities;
			} else {
				return null;
			}
		}

	}
}

