using System.Text.Json.Serialization;

namespace KufarAppProject.ApiClasses
{
    public class KufarApiResponse
    {
        [JsonPropertyName("ads")]
        public List<Ad> Ads { get; set; }
    }

    public class Ad
    {
        [JsonPropertyName("price_usd")]
        public string Price { get; set; }

        [JsonPropertyName("ad_parameters")]
        public List<AdParameter> AdParameters { get; set; }

        [JsonPropertyName("ad_link")]
        public string AdLink { get; set; }
    }

    public class AdParameter
    {
        [JsonPropertyName("p")]
        public string P { get; set; }

        [JsonPropertyName("v")]
        public object V { get; set; }

        [JsonPropertyName("vl")]
        public object Vl { get; set; }
    }
}
