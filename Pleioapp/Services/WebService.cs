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
			var uri = new Uri (Constants.Url + "api/groups/mine");

			try {
				var response = await getClient().GetAsync (uri);
				var content = await response.Content.ReadAsStringAsync ();

				if (response.IsSuccessStatusCode) {
					//System.Diagnostics.Debug.WriteLine ("Get groups succesful: " + content);

					var list = JsonConvert.DeserializeObject <PaginatedGroupList> (content);
					return list.entities;
				}
			} catch (Exception e) {
				Xamarin.Insights.Report (e);
			}

			return null;
		}

		public async Task<bool> MarkGroupAsRead(Group group)
		{
			var uri = new Uri (Constants.Url + "api/groups/" + group.guid + "/activities/mark_read");

			try {
				var response = await getClient().PostAsync(uri, null);
				var content = await response.Content.ReadAsStringAsync();

				System.Diagnostics.Debug.WriteLine ("Marked group as read " + content);

				if (response.IsSuccessStatusCode) {
					return true;
				}
			} catch (Exception e) {
				Xamarin.Insights.Report (e);
			}

			return false;
		}

		public async Task<List<Activity>> GetActivities (Group group)
		{
			var uri = new Uri (Constants.Url + "api/groups/" + group.guid + "/activities");

			try {
				var response = await getClient().GetAsync (uri);
				var content = await response.Content.ReadAsStringAsync ();

				if (response.IsSuccessStatusCode) {
					//System.Diagnostics.Debug.WriteLine ("Get activities succesful: " + content);

					var list = JsonConvert.DeserializeObject <PaginatedActivityList> (content);
					return list.entities;
				}
			} catch (Exception e) {
				Xamarin.Insights.Report (e);
			}

			return null;
		}


		public async Task<bool> RegisterPush(string token, string service)
		{
			var uri = new Uri (Constants.Url + "api/users/me/register_push");

			if (service != "apns" && service != "gcm") {
				throw new Exception ("Invalid service type specified during push token registration.");
			}

			try {
				var formContent = new FormUrlEncodedContent (new [] {
					new KeyValuePair<string,string>("token", token),
					new KeyValuePair<string,string>("service", service),
				});

				var response = await getClient().PostAsync(uri, formContent);
				var content = await response.Content.ReadAsStringAsync();

				if (response.IsSuccessStatusCode) {
					System.Diagnostics.Debug.WriteLine ("Succefully registered push token (" + token + ") at webservice " + content);
					return true;
				} else {
					System.Diagnostics.Debug.WriteLine ("Could not register push token at webservice " + content);
				}
			} catch (Exception e) {
				Xamarin.Insights.Report (e);
			}

			return false;
		}
	}
}

