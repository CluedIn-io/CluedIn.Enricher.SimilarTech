using System.Collections.Generic;
using Newtonsoft.Json;

namespace CluedIn.ExternalSearch.Providers.SimilarTech.Models
{
    public class SimilarTechResponse
    {
        [JsonIgnore]
        public TechnologiesResponse TechnologiesResponse { get; set; }

        [JsonProperty("site")]
        public string Site { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("yearFounded")]
        public int YearFounded { get; set; }

        [JsonProperty("homepage")]
        public string Homepage { get; set; }

        [JsonProperty("favicon")]
        public string Favicon { get; set; }

        [JsonProperty("logo")]
        public string Logo { get; set; }

        [JsonProperty("rankings")]
        public List<Ranking> Rankings { get; set; }

        [JsonProperty("address")]
        public string Address { get; set; }

        [JsonProperty("location")]
        public string Location { get; set; }

        [JsonProperty("country")]
        public string Country { get; set; }

        [JsonProperty("state")]
        public string State { get; set; }

        [JsonProperty("city")]
        public string City { get; set; }

        [JsonProperty("zip")]
        public string Zip { get; set; }

        [JsonProperty("phone")]
        public string Phone { get; set; }

        [JsonProperty("fax")]
        public string Fax { get; set; }

        [JsonProperty("employeeRange")]
        public string EmployeeRange { get; set; }

        [JsonProperty("revenueRange")]
        public string RevenueRange { get; set; }

        [JsonProperty("linkedinUrl")]
        public string LinkedInUrl { get; set; }

        [JsonProperty("facebookUrl")]
        public string FacebookUrl { get; set; }

        [JsonProperty("twitterUrl")]
        public string TwitterUrl { get; set; }

        [JsonProperty("facebookLikes")]
        public int FacebookLikes { get; set; }

        [JsonProperty("twitterFollowers")]
        public int TwitterFollowers { get; set; }

        [JsonProperty("linkedInId")]
        public int LinkedInId { get; set; }
    }
}
