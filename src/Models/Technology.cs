using System.Collections.Generic;
using Newtonsoft.Json;

namespace CluedIn.ExternalSearch.Providers.SimilarTech.Models
{
    public class Technology
    {
        [JsonProperty("coverage")]
        public double Coverage { get; set; }

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("categories")]
        public List<string> Categories { get; set; }

        [JsonProperty("paying")]
        public string Paying { get; set; }
    }



}
