using System;
using System.Threading.Tasks;

namespace Pleioapp
{
	public interface ISSOService
	{
		void OpenUrl(string url);
		void Expire();
		Task<bool> LoadToken();
	}
}

