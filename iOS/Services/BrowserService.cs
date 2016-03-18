using System;
using Xamarin.Forms;
using Pleioapp.iOS;
using Foundation;

[assembly: Dependency(typeof(BrowserService))]
namespace Pleioapp.iOS
{

	public class BrowserService : IBrowserService
	{
		public void OpenUrl(string Url) {
			UIKit.UIApplication.SharedApplication.OpenUrl (new NSUrl (Url));
		}
	}
}

