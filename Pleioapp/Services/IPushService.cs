using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Pleioapp
{
	public interface IPushService
	{
		void RequestToken();
		void SaveToken(string deviceToken);
		void SetBadgeNumber(int number);
		void ProcessPushNotification (Dictionary <string, string> data);
		string GetToken();
		Task<bool> RegisterToken();
	}

}

