using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace KufarAppProject
{
    public class KufarApi
    {
        private const string UrlApi = "https://api.kufar.by/search-api/v2/search/rendered-paginated?cat=1010&cur=USD&gtsy=country-belarus~province-minsk~locality-minsk&lang=ru&size=30&typ=sell";
        private HttpClient _httpClient = new();

        public KufarApi() 
        {
            try
            {
                var responseString = _httpClient.GetStringAsync(UrlApi);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
