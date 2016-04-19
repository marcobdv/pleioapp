using System;
using Xamarin.Forms;
using Pleioapp.Droid;

[assembly: Dependency(typeof(BrowserService))]
namespace Pleioapp.Droid
{
	public class BrowserService : IBrowserService
	{
		public void OpenUrl(string Url) {
			Device.OpenUri (new Uri (Url));
		}
	}
}

