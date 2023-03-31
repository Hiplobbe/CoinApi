using Microsoft.AspNetCore.Mvc;
using CoinApi.Helpers;
using CoinApi.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using CoinApi.Models;

namespace CoinApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CoinController : ControllerBase
    {
        private IMemoryCache _cache;
        private readonly ILogger<CoinController> _logger;
        private List<IApiHelper> _apiHelpers = new List<IApiHelper>();

        public CoinController(ILogger<CoinController> logger, IMemoryCache cache)
        {
            _logger = logger;
            _cache = cache;
            
            _apiHelpers.Add(new GeckoHelper("Coingecko", "?localization=false"));
            _apiHelpers.Add(new LCHelper("Livecoinwatch"));
        }

        [HttpPost(Name = "GetCoin")]
        public async Task<ActionResult<ICoin>> Get(RequestCoin reqCoin)
        {
            ICoin coin;

            if (!_cache.TryGetValue(reqCoin.Code.ToUpper(), out coin))
            {
                coin = await UpdateCache(reqCoin);                

                if(coin != null)
                {
                    var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(1));

                    // Save data in cache.
                    _cache.Set(coin.Code.ToUpper(), coin, cacheEntryOptions);
                }
            }

            if(coin != null)
            {
                return Ok(coin);
            }

            return BadRequest();
        }

        private async Task<ICoin> UpdateCache(RequestCoin reqCoin)
        {
            ICoin coin;

            for (int i = 0; i < _apiHelpers.Count; i++)
            {
                coin = await CallApi(reqCoin, _apiHelpers[i]);
                    
                if(coin != null)
                {
                    return coin;
                }
            }

            throw new Exception($"Unable to update cache for {reqCoin.Name}");
        }

        private async Task<ICoin> CallApi(RequestCoin reqCoin, IApiHelper helper)
        {
            ICoin coin;

            try
            {
                coin = await helper.GetCoin(reqCoin);

                return coin;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Unable to get {reqCoin.Name} from {helper.ApiAddress}. Error: {ex.Message}");
            }

            return null;
        }
    }
}