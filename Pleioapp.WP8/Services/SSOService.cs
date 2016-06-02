using System;
using Xamarin.Forms;
using System.Threading.Tasks;
using Pleioapp.WP8;

[assembly: Dependency(typeof(SSOService))]
namespace Pleioapp.WP8
{
	public class SSOService : ISSOService
	{
	    public void OpenUrl(string url)
	    {
	        throw new NotImplementedException();
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

