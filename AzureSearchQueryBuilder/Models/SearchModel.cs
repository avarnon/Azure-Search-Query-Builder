using Newtonsoft.Json;

namespace AzureSearchQueryBuilder.Models
{
    public abstract class SearchModel
    {
        private const string __scoringProfileScore = "search.score()";

        [JsonIgnore]
        [JsonProperty(__scoringProfileScore)]
        public double? SearchScore { get; set; }
    }
}
