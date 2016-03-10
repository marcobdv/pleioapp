using System;
using Newtonsoft.Json;

namespace Pleioapp
{
	public class SSOToken
	{
		[JsonProperty]
		public string token { get; set; }

		[JsonProperty]
		public double expiry { get; set; }

		[JsonProperty(PropertyName="user_guid")]
		public string userGuid { get; set; }
	}
}

