using CoinApi.Interfaces;

namespace CoinApi.Models
{
    public struct Coin : ICoin
    {
        public string Name {get; set;}
        public string Code { get; set; }
        public string Chain {get; set;}
        public double DollarValue {get; set;}
    }
}
