using System;

namespace Pleioapp
{
	public interface ITokenStore
	{
		void saveToken(AuthToken token);
		AuthToken getToken();
		void clearTokens();
	}
		
}

