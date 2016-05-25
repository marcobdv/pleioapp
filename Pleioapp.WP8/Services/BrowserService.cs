using System;
using Xamarin.Forms;
using Pleioapp.WP8;

[assembly: Dependency(typeof(BrowserService))]
namespace Pleioapp.WP8
{

	public class BrowserService : IBrowserService
	{
		public void OpenUrl(string Url) {
			
		}
	}
}

