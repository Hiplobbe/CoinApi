using CoinApi.Interfaces;
using CoinApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq;
using System.Net.Http.Headers;

namespace CoinApi.Helpers
{
    public class GeckoHelper : IUrlApi
    {
        public string Name { get; set; }
        public string ApiAddress { get; set; }
        public string PresetParams { get; set; }

        public GeckoHelper(string name, string preset = "")
        {
            Name = name;
            ApiAddress = "https://api.coingecko.com/api/v3/coins/";
            PresetParams = preset;
        }

        public async Task<ICoin> GetCoin(RequestCoin coin)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(ApiAddress + coin.Name);
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json")
            );

            var response = client.GetAsync(PresetParams).Result;            
            if(response.IsSuccessStatusCode)
            {
                var jsonResp = JObject.Parse(await response.Content.ReadAsStringAsync());

                return new Coin { 
                    Name = jsonResp.SelectToken("name").ToString(), 
                    Code = jsonResp.SelectToken("symbol").ToString(),
                    DollarValue = Convert.ToDouble(jsonResp.SelectToken("market_data").SelectToken("current_price").SelectToken("usd")) 
                };
            }

            throw new Exception($"Error when contacting Gecko, {response.StatusCode} {response.Content}");
        }
    }
}
