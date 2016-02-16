using System;
using UIKit;
using Foundation;
using Xamarin.Forms;
using Pleioapp.iOS;

[assembly: Dependency(typeof(PushService))]
namespace Pleioapp.iOS
{
	public class PushService : IPushService
	{
		public void Register() {
			System.Diagnostics.Debug.WriteLine ("Registering for push notifications");
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

		public void PublishRegistration(string DeviceToken) {
			System.Diagnostics.Debug.WriteLine ("Request to publish registration " + DeviceToken);
		}
	}
}

