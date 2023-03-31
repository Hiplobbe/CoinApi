using CoinApi;
using CoinApi.Models;

namespace CoinApi.Interfaces
{
    public interface IApiHelper
    {
        public string Name { get; set; }
        public string ApiAddress { get; set; }

        /// <summary>
        /// Searches the api for the value of a coin.
        /// </summary>
        /// <param name="reqCoin">RequestCoin object that hold the name and the code of the coin.</param>
        /// <returns></returns>
        Task<ICoin> GetCoin(RequestCoin reqCoin);
    }
}
