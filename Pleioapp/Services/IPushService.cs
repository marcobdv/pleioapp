using System;

namespace Pleioapp
{
	public interface IPushService
	{
		void Register();
		void PublishRegistration(string deviceToken);
	}

}

