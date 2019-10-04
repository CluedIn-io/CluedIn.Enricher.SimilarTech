using System.Collections.Generic;
using Newtonsoft.Json;

namespace CluedIn.ExternalSearch.Providers.SimilarTech.Models
{
    public class Ranking
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("year")]
        public int Year { get; set; }
    }
}
