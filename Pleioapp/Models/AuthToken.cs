using System;
using Newtonsoft.Json;

namespace Pleioapp
{
	public class AuthToken
	{
		[JsonProperty(PropertyName="access_token")]
		public string accessToken;
		[JsonProperty(PropertyName="expires_in")]
		public string expiresIn;
		[JsonProperty(PropertyName="token_type")]
		public string tokenType;
		[JsonProperty(PropertyName="refresh_token")]
		public string refreshToken;
		[JsonProperty(PropertyName="scope")]
		public string scope;
	}
}

