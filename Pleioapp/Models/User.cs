using Newtonsoft.Json;

namespace Pleioapp
{
	public class User
	{
		string _iconUrl;

		[JsonProperty]
		public string guid { get; set; }

		[JsonProperty]
		public string name { get; set; }

		[JsonProperty]
		public string url { get; set; }

		[JsonProperty(PropertyName="icon_url")]
		public string iconUrl {
			get { return _iconUrl + "?guid=" + guid; }
			set { _iconUrl = value; }
		}
	}
}

