using System;
using UIKit;
using Foundation;
using Xamarin.Forms;
using Pleioapp.iOS;
using System.Threading.Tasks;
using System.Collections.Generic;

[assembly: Dependency(typeof(PushService))]
namespace Pleioapp.iOS
{
	public class PushService : IPushService
	{
		public void RequestToken() {
			if (UIDevice.CurrentDevice.CheckSystemVersion (8, 0)) {
				var pushSettings = UIUserNotificationSettings.GetSettingsForTypes (
					UIUserNotificationType.Alert | UIUserNotificationType.Badge | UIUserNotificationType.Sound,
					new NSSet ());

				UIApplication.SharedApplication.RegisterUserNotificationSettings (pushSettings);
				UIApplication.SharedApplication.RegisterForRemoteNotifications ();
			} else {
				UIRemoteNotificationType notificationTypes = UIRemoteNotificationType.Alert | UIRemoteNotificationType.Badge | UIRemoteNotificationType.Sound;
				UIApplication.SharedApplication.RegisterForRemoteNotificationTypes (notificationTypes);
			}
		}

		public void SetBadgeNumber(int number) {
			UIApplication.SharedApplication.ApplicationIconBadgeNumber = number;
		}

		public void SaveToken(string DeviceToken) {
			NSUserDefaults.StandardUserDefaults.SetString(DeviceToken, "PushDeviceToken");
		}

		public string GetToken() {
			return NSUserDefaults.StandardUserDefaults.StringForKey ("PushDeviceToken");
		}

		public Task<bool> RegisterToken()
		{
			var service = (WebService) App.Current.Properties ["WebService"];
			var deviceId = UIKit.UIDevice.CurrentDevice.IdentifierForVendor.AsString ();
			return service.RegisterPush (deviceId, GetToken (), "apns");
		}

		public void ProcessPushNotification(Dictionary <string, string> data)
		{
			System.Diagnostics.Debug.WriteLine ("Received a push notification " + data.ToString());
			MessagingCenter.Send<Xamarin.Forms.Application> (App.Current, "refresh_groups");
		}
	}
}

