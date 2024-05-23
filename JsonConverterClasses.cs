using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace KufarAppProject
{
    public class KufarApiResponce
    {
        [JsonPropertyName("ads")]
        public List<Ads> Ads { get; set; }
    }

    public class Ads
    {
        [JsonPropertyName("price_usd")]
        public double Prive { get; set; }

        [JsonPropertyName("ad_parameter")]
        public List<AdParameter> AdParameters { get; set; }
    }

    public class AdParameter
    {
        [JsonPropertyName("p")]
        public string P { get; set; }

        [JsonPropertyName("v")]
        public string V { get; set; }
    }
}
