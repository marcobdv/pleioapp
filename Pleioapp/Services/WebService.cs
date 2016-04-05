using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Pleioapp
{
	public class WebService
	{
		private AuthToken _token;
		private HttpClient _client;

		private Site currentSite { 
			get {
				App app = (App)App.Current;
				return app.currentSite;
			}
		}

		private HttpClient getClient()
		{
			if (_client == null) {
				var app = (App)App.Current;
				if (app.authToken == null) {
					throw new Exception ("Tried to perform a protected API call but there is no OAuth2 authentication token initialized.");
				}

				_token = app.authToken;
				_client = new HttpClient ();
				_client.DefaultRequestHeaders.Add ("Authorization", string.Format ("Bearer {0}", _token.accessToken));
			}

			return _client;
		}

		public async Task<List<Site>> GetSites()
		{
			var uri = new Uri (currentSite.url + "api/sites/mine");

			System.Diagnostics.Debug.WriteLine ("[Webservice] Retrieving list of sites...");
			var response = await getClient().GetAsync (uri);
			var content = await response.Content.ReadAsStringAsync ();

			if (response.IsSuccessStatusCode) {
				System.Diagnostics.Debug.WriteLine ("[Webservice] Retrieved list of sites");

				var list = JsonConvert.DeserializeObject <PaginatedSiteList> (content);
				return list.entities;
			}

			return new List<Site>();
		}

		public async Task<List<Group>> GetGroups ()
		{
			var uri = new Uri (currentSite.url + "api/groups/mine?limit=100");

			System.Diagnostics.Debug.WriteLine ("[Webservice] Retrieving list of groups...");

			var response = await getClient().GetAsync (uri);
			var content = await response.Content.ReadAsStringAsync ();

			if (response.IsSuccessStatusCode) {
				System.Diagnostics.Debug.WriteLine ("[Webservice] Retrieved list of groups");

				var list = JsonConvert.DeserializeObject <PaginatedGroupList> (content);
				if (list != null) {
					return list.entities;
				}
			} else {
				if (response.StatusCode == System.Net.HttpStatusCode.Forbidden) {
					MessagingCenter.Send<Xamarin.Forms.Application> (App.Current, "refresh_access_token");
				}
			}

			return new List<Group>();
		}
			
		public async Task<bool> MarkGroupAsRead(Group group)
		{
			var uri = new Uri (currentSite.url + "api/groups/" + group.guid + "/activities/mark_read");

			try {
				System.Diagnostics.Debug.WriteLine ("[Webservice] Marking group as read... ");

				var response = await getClient().PostAsync(uri, null);
				var content = await response.Content.ReadAsStringAsync();

				System.Diagnostics.Debug.WriteLine ("[Webservice] Marked group as read " + content);

				if (response.IsSuccessStatusCode) {
					return true;
				}
			} catch (Exception e) {
				System.Diagnostics.Debug.WriteLine ("Catched exception " + e);
				Xamarin.Insights.Report (e);
			}

			return false;
		}

		public async Task<List<Activity>> GetActivities (Group group)
		{
			var uri = new Uri (currentSite.url + "api/groups/" + group.guid + "/activities");

			System.Diagnostics.Debug.WriteLine ("[Webservice] Retrieving activities... ");

			var response = await getClient().GetAsync (uri);
			var content = await response.Content.ReadAsStringAsync ();

			if (response.IsSuccessStatusCode) {
				System.Diagnostics.Debug.WriteLine ("[Webservice] Retrieved activities");

				var list = JsonConvert.DeserializeObject <PaginatedActivityList> (content);
				return list.entities;
			} else {
				if (response.StatusCode == System.Net.HttpStatusCode.Forbidden) {
					MessagingCenter.Send<Xamarin.Forms.Application> (App.Current, "refresh_access_token");
				}
			}

			return new List<Activity>();
		}

		public async Task<List<Event>> GetEvents (Group group)
		{
			var uri = new Uri (currentSite.url + "api/groups/" + group.guid + "/events");

			System.Diagnostics.Debug.WriteLine ("[Webservice] Retrieving events... ");

			var response = await getClient().GetAsync (uri);
			var content = await response.Content.ReadAsStringAsync ();

			if (response.IsSuccessStatusCode) {
				System.Diagnostics.Debug.WriteLine ("[Webservice] Retrieved events");

				var list = JsonConvert.DeserializeObject <PaginatedEventList> (content);
				return list.entities;
			}

			return new List<Event>();
		}

		public async Task<List<User>> GetMembers (Group group)
		{
			var uri = new Uri (currentSite.url + "api/groups/" + group.guid + "/members?limit=100");

			System.Diagnostics.Debug.WriteLine ("[Webservice] Retrieving members... ");

			var response = await getClient().GetAsync (uri);
			var content = await response.Content.ReadAsStringAsync ();

			if (response.IsSuccessStatusCode) {
				System.Diagnostics.Debug.WriteLine ("[Webservice] Retrieved members");

				var list = JsonConvert.DeserializeObject <PaginatedMemberList> (content);
				return list.entities;
			}

			return new List<User>();
		}

		public async Task<bool> RegisterPush(string deviceId, string token, string service)
		{
			var uri = new Uri (currentSite.url + "api/users/me/register_push");

			if (service != "apns" && service != "gcm") {
				throw new Exception ("Invalid service type specified during push token registration.");
			}

			try {
				var formContent = new FormUrlEncodedContent (new [] {
					new KeyValuePair<string,string>("device_id", deviceId),
					new KeyValuePair<string,string>("token", token),
					new KeyValuePair<string,string>("service", service),
				});

				var response = await getClient().PostAsync(uri, formContent);
				var content = await response.Content.ReadAsStringAsync();

				if (response.IsSuccessStatusCode) {
					System.Diagnostics.Debug.WriteLine ("[Webservice] Succefully registered push token (" + token + ") at webservice " + content);
					return true;
				} else {
					System.Diagnostics.Debug.WriteLine ("[Webservice] Could not register push token at webservice " + content);

					if (response.StatusCode == System.Net.HttpStatusCode.Forbidden) {
						MessagingCenter.Send<Xamarin.Forms.Application> (App.Current, "refresh_access_token");
					}
				}
			} catch (Exception e) {
				System.Diagnostics.Debug.WriteLine ("Catched exception " + e);
				Xamarin.Insights.Report (e);
			}

			return false;
		}

		public async Task<bool>DeregisterPush(string deviceId, string service)
		{
			var uri = new Uri (currentSite.url + "api/users/me/deregister_push");

			if (service != "apns" && service != "gcm") {
				throw new Exception ("Invalid service type specified during push token registration.");
			}

			try {
				var formContent = new FormUrlEncodedContent (new [] {
					new KeyValuePair<string,string>("device_id", deviceId),
					new KeyValuePair<string,string>("service", service)
				});

				var response = await getClient().PostAsync(uri, formContent);
				var content = await response.Content.ReadAsStringAsync();

				if (response.IsSuccessStatusCode) {
					System.Diagnostics.Debug.WriteLine ("[Webservice] Succefully unregistered push token at webservice " + content);
					return true;
				} else {
					System.Diagnostics.Debug.WriteLine ("[Webservice] Could not unregister push token at webservice " + content);
				}
			} catch (Exception e) {
				System.Diagnostics.Debug.WriteLine ("Catched exception " + e);
				Xamarin.Insights.Report (e);
			}

			return false;
		}

		public async Task<SSOToken> GenerateToken()
		{
			try {
				var uri = new Uri (currentSite.url + "api/users/me/generate_token");
				var response = await getClient ().PostAsync (uri, null);
				var content = await response.Content.ReadAsStringAsync();

				if (response.IsSuccessStatusCode) {
					return JsonConvert.DeserializeObject <SSOToken> (content);
				}
			} catch (Exception e) {
				System.Diagnostics.Debug.WriteLine ("Catched exception " + e);
				Xamarin.Insights.Report (e);
			}

			return null;
		}
	}
}

