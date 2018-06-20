using System.Collections.Generic;
using Newtonsoft.Json;

namespace Pleioapp
{
    public class SiteList
    {
        [JsonProperty]
        public List<Site> sites { get; set; }
    }
}