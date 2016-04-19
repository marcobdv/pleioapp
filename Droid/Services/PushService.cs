using System;
using Xamarin.Forms;
using System.Threading.Tasks;
using System.Collections.Generic;
using Pleioapp.Droid;

[assembly: Dependency(typeof(PushService))]
namespace Pleioapp.Droid
{
	public class PushService : IPushService
	{
		public void RequestToken() {
			return;
		}

		public void SetBadgeNumber(int number) {
			return;
		}

		public void SaveToken(string DeviceToken) {
			return;
		}

		public string GetToken() {
			return "";
		}

		public Task<bool> RegisterToken()
		{
			var app = (App)App.Current;
			var service = app.webService;
			//return service.RegisterPush ("", "", "gcm1");

			var t = Task.Factory.StartNew(() => {
				return true;
			});
			return t;
		}

		public Task<bool> DeregisterToken()
		{
			var app = (App)App.Current;
			var service = app.webService;
			//return service.DeRegisterPush ("", "", "gcm1");

			var t = Task.Factory.StartNew(() => {
				return true;
			});
			return t;

		}

		public void ProcessPushNotification(Dictionary <string, string> data)
		{
			System.Diagnostics.Debug.WriteLine ("Received a push notification " + data.ToString());
			MessagingCenter.Send<Xamarin.Forms.Application> (App.Current, "refresh_menu");
		}
	}
}

