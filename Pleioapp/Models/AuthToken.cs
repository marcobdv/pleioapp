using System;
using Newtonsoft.Json;

namespace Pleioapp
{
	public class AuthToken
	{
	    [JsonProperty(PropertyName="access_token")]
		public string accessToken { get; set; }
		[JsonProperty(PropertyName="expires_in")]
		public string expiresIn { get; set; }
		[JsonProperty(PropertyName="token_type")]
		public string tokenType { get; set; }
		[JsonProperty(PropertyName="refresh_token")]
		public string refreshToken { get; set; }
		[JsonProperty(PropertyName="scope")]
		public string scope { get; set; }
	    [JsonIgnore]
	    public string mainSiteName { get; set; }
	    [JsonIgnore]
        public string mainSiteUrl { get; set; }
	}
}

