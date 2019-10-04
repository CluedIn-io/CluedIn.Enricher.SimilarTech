using System.Collections.Generic;
using Newtonsoft.Json;

namespace CluedIn.ExternalSearch.Providers.SimilarTech.Models
{
    public class TechnologiesResponse
    {
        [JsonProperty("site")]
        public string Site { get; set; }

        [JsonProperty("found")]
        public bool Found { get; set; }

        [JsonProperty("technologies")]
        public List<Technology> Technologies { get; set; }
    }
}
