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
	        throw new NotImplementedException();
	    }

	    public void SaveToken(string deviceToken)
	    {
	        throw new NotImplementedException();
	    }

	    public void SetBadgeNumber(int number)
	    {
	        throw new NotImplementedException();
	    }

	    public void ProcessPushNotification(Dictionary<string, string> data)
	    {
	        throw new NotImplementedException();
	    }

	    public string GetToken()
	    {
	        throw new NotImplementedException();
	    }

	    public Task<bool> RegisterToken()
	    {
	        throw new NotImplementedException();
	    }

	    public Task<bool> DeregisterToken()
	    {
	        throw new NotImplementedException();
	    }
	}
}

