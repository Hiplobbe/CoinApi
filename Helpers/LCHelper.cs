using CoinApi.Interfaces;
using CoinApi.Models;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using System.Text;

namespace CoinApi.Helpers
{
    public class LCHelper : IJsonApi
    {
        public string Name { get; set; }
        public string ApiAddress { get; set; }
        public string JsonTemplate { get; set; }

        public LCHelper(string name)
        {
            Name = name;
            ApiAddress = "https://api.livecoinwatch.com/coins/single";

            JsonTemplate = "'code': '{0}', 'currency': 'USD', 'meta': true";
        }

        public async Task<ICoin> GetCoin(RequestCoin coin)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(ApiAddress);
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json")
            );            

            HttpRequestMessage httpRequest = new HttpRequestMessage(HttpMethod.Post, client.BaseAddress);
            httpRequest.Content = new StringContent(
                "{" + String.Format(JsonTemplate, coin.Code) + "}", 
                Encoding.UTF8, 
                "application/json"
            );
            httpRequest.Headers.Add("x-api-key", ""); //TODO: Insert key

            var response = client.SendAsync(httpRequest).Result;

            if (response.IsSuccessStatusCode)
            {
                var jsonResp = JObject.Parse(await response.Content.ReadAsStringAsync());

                return new Coin
                {
                    Name = jsonResp.SelectToken("name").ToString(),
                    DollarValue = Convert.ToDouble(jsonResp.SelectToken("market_data").SelectToken("current_price").SelectToken("usd"))
                };
            }

            throw new Exception($"Error when contacting Livecoin, {response.StatusCode} {response.Content}");
        }
    }
}
