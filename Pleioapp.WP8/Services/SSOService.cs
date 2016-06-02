using System;
using Xamarin.Forms;
using System.Threading.Tasks;
using Windows.System;
using Pleioapp.WP8;

[assembly: Dependency(typeof(SSOService))]
namespace Pleioapp.WP8
{
	public class SSOService : ISSOService
	{
	    public async void OpenUrl(string url)
	    {
            await Launcher.LaunchUriAsync(new Uri(url));
        }

	    public void Expire()
	    {
	        throw new NotImplementedException();
	    }

	    public Task<bool> LoadToken()
	    {
	        throw new NotImplementedException();
	    }
	}
}

