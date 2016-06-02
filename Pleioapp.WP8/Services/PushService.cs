using System;
using Xamarin.Forms;
using System.Threading.Tasks;
using System.Collections.Generic;
using Pleioapp.WP8;

[assembly: Dependency(typeof(PushService))]
namespace Pleioapp.WP8
{
	public class PushService : IPushService
	{
        public void RequestToken()
        {
            return;
        }

        public void SetBadgeNumber(int number)
        {
            return;
        }

        public void SaveToken(string DeviceToken)
        {
            return;
        }

        public string GetToken()
        {
            return "";
        }

        public async Task<bool> RegisterToken()
        {
            return true;
        }

        public async Task<bool> DeregisterToken()
        {
            return true;
        }

        public void ProcessPushNotification(Dictionary<string, string> data)
        {
            //System.Diagnostics.Debug.WriteLine("Received a push notification " + data.ToString());
            //MessagingCenter.Send<Xamarin.Forms.Application>(App.Current, "refresh_menu");
        }
    }
}

