﻿using OxyPlot.Axes;
using System.Net.Http;
using System.Text.Json;

namespace KufarAppProject.ApiClasses
{
    public class KufarApi
    {
        private const int NumberOfFlats = 30;
        private string _urlApi = $"https://api.kufar.by/search-api/v2/search/rendered-paginated?cat=1010&cur=USD&gtsy=country-belarus~province-minsk~locality-minsk&lang=ru&size={NumberOfFlats}&typ=sell";
        private HttpClient _httpClient = new();

        public KufarApiResponse Response { get; private set; }

        public KufarApi()
        {
            try
            {
                var responseString = _httpClient.GetStringAsync(_urlApi).Result;
                Response = JsonSerializer.Deserialize<KufarApiResponse>(responseString) ?? throw new ArgumentNullException("No json data");
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Method to retrieve prices of flats by their floor.
        /// </summary>
        /// <returns>Dictionary with price as a valye and floor as a key.</returns>
        public Dictionary<int, double> GetPriceByFloor()
        {
            var interimResult = new Dictionary<int, List<double>>();
            foreach (var ad in Response.Ads)
            {
                var floor = GetParameter<int>(ad.AdParameters, "floor");
                var price = GetParameter<double>(ad.AdParameters, "square_meter");
                if (price < 300) continue;
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
                var rooms = Convert.ToInt32(GetParameter<string>(ad.AdParameters, "rooms"));
                var price = GetParameter<double>(ad.AdParameters, "square_meter");
                if (price < 300) continue;
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
                var metro = GetParameter<string>(ad.AdParameters, "metro", KeyToRetrieve.Vl);
                if (metro is null)
                {
                    continue;
                }
                var price = GetParameter<double>(ad.AdParameters, "square_meter");
                if (price < 300) continue;
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

        public List<string> GetFlatsInCoordinates(List<(double, double)> coordinates)
        {
            if (coordinates.Count < 3)
            {
                throw new ArgumentException("Points dont form the closed figure");
            }
            var result = new List<string>();
            var boundaryPoints = coordinates;
            foreach (var ad in Response.Ads)
            {
                var coords = GetParameter<List<double>>(ad.AdParameters, "coordinates");
                if (coords is null)
                {
                    continue;
                }
                if (IsPointInsideFigure((coords[1], coords[0])))
                {
                    result.Add(ad.AdLink);
                }
            }

            bool IsPointInsideFigure((double, double) point)
            {
                int n = boundaryPoints.Count;
                bool isInside = false;

                for (int i = 0, j = n - 1; i < n; j = i++)
                {
                    double xi = boundaryPoints[i].Item1, yi = boundaryPoints[i].Item2;
                    double xj = boundaryPoints[j].Item1, yj = boundaryPoints[j].Item2;

                    bool intersect = ((yi > point.Item2) != (yj > point.Item2)) &&
                                     (point.Item1 < (xj - xi) * (point.Item2 - yi) / (yj - yi) + xi);
                    if (intersect)
                    {
                        isInside = !isInside;
                    }
                }

                return isInside;
            }
            return result;
        }
        /// <summary>
        /// Get required parameter from Kufar Api.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="adParameters"></param>
        /// <param name="pName"></param>
        /// <param name="parameterKeyToRetrieve"></param>
        /// <returns></returns>
        private static T? GetParameter<T>(List<AdParameter> adParameters, string pName, KeyToRetrieve parameterKeyToRetrieve = KeyToRetrieve.V)
        {
            var parameter = adParameters.
                Where(parameter => parameter.P == pName)
                .FirstOrDefault();
            if (parameter == null) return default;
            if (parameterKeyToRetrieve == KeyToRetrieve.V)
            {
                if (parameter.V is JsonElement jsonElement)
                {
                    if (jsonElement.ValueKind == JsonValueKind.Array && pName != "coordinates")
                    {
                        return JsonSerializer.Deserialize<T>(jsonElement[0].GetRawText());
                    }
             
                    else
                    {
                        return JsonSerializer.Deserialize<T>(jsonElement.GetRawText());
                    }
                }
                return (T)parameter.V;
            }
            else
            {
                if (parameter.Vl is JsonElement jsonElement)
                {
                    return JsonSerializer.Deserialize<T>(jsonElement[0].GetRawText());
                }
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