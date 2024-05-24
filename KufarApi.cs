using System.Net.Http;
using System.Text.Json;

namespace KufarAppProject
{
    public class KufarApi
    {
        private const string UrlApi = "https://api.kufar.by/search-api/v2/search/rendered-paginated?cat=1010&cur=USD&gtsy=country-belarus~province-minsk~locality-minsk&lang=ru&size=30&typ=sell";
        private HttpClient _httpClient = new();

        public KufarApiResponse Response { get; private set; }

        public KufarApi() 
        {
            try
            {
                var responseString = _httpClient.GetStringAsync(UrlApi).Result;
                Response = JsonSerializer.Deserialize<KufarApiResponse>(responseString) ?? throw new ArgumentNullException("No json data");
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Dictionary<int, double> GetPriceByFloor()
        {
            var interimResult = new Dictionary<int, List<double>>();
            foreach(var ad in Response.Ads)
            {
                var floor = GetParameter<int>(ad.AdParameters, "floor");
                var price = GetParameter<double>(ad.AdParameters, "square_meter");
                if (interimResult.TryGetValue(floor, out List<double>? value))
                {
                    value.Add(price);
                }
                else
                {
                    interimResult[floor] = [price];
                }
            }
            var result = new Dictionary<int, double>();
            foreach (var pair in interimResult)
            {
                result[pair.Key] = pair.Value.Average();
            }
            return result;
        }

        public Dictionary<int, double> GetPriceByNumberOfRooms()
        {
            var interimResult = new Dictionary<int, List<double>>();
            foreach (var ad in Response.Ads)
            {
                var rooms = GetParameter<int>(ad.AdParameters, "rooms");
                var price = GetParameter<double>(ad.AdParameters, "square_meter");
                if (interimResult.TryGetValue(rooms, out List<double>? value))
                {
                    value.Add(price);
                }
                else
                {
                    interimResult[rooms] = [price];
                }
            }
            var result = new Dictionary<int, double>();
            foreach (var pair in interimResult)
            {
                result[pair.Key] = pair.Value.Average();
            }
            return result;
        }

        public Dictionary<string, double> GetPriceByMetroStation() 
        {
            var interimResult = new Dictionary<string, List<double>>();
            foreach (var ad in Response.Ads)
            {
                var metro = GetParameter<string>(ad.AdParameters, "metro");
                if (metro is null)
                {
                    continue;
                }
                var price = GetParameter<double>(ad.AdParameters, "square_meter");
                if (interimResult.TryGetValue(metro, out List<double>? value))
                {
                    value.Add(price);
                }
                else
                {
                    interimResult[metro] = [price];
                }
            }
            var result = new Dictionary<string, double>();
            foreach (var pair in interimResult)
            {
                result[pair.Key] = pair.Value.Average();
            }
            return result;
        }

        private static T? GetParameter<T>(List<AdParameter> adParameters, string pName, KeyToRetrieve parameterKeyToRetrieve = KeyToRetrieve.V)
        {
            var parameter = adParameters.
                Where(parameter => parameter.P == pName)
                .FirstOrDefault();
            if (parameter == null) return default;
            if (parameterKeyToRetrieve == KeyToRetrieve.V)
            {
                if (parameter.V is JsonElement jsonElement && jsonElement.ValueKind == JsonValueKind.Array)
                {
                    return JsonSerializer.Deserialize<T>(jsonElement[0].GetRawText());
                }
                return (T)parameter.V;
            }
            else
            {
                return (T)parameter.Vl;
            }
        }

        private enum KeyToRetrieve
        {
            V,
            Vl
        }
    }
}
