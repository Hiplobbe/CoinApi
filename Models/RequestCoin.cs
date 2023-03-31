using System.ComponentModel.DataAnnotations;

namespace CoinApi.Models
{
    public class RequestCoin
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Code { get; set; }
    }
}
