using Basket.Api.Entities;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace Basket.Api.Repository
{
    public class BasketRepository : IBasketRepository
    {
        private readonly IDistributedCache _redisCache;

        public BasketRepository(IDistributedCache redisCache)
        {
            _redisCache = redisCache ?? throw new ArgumentNullException(nameof(redisCache));
        }

        public async Task<ShoppingCart?> GetShoppingCart(string userName)
        {
            var basket = await _redisCache.GetStringAsync(userName);
            if (string.IsNullOrEmpty(basket))
            {
                return null;
            }
            return JsonConvert.DeserializeObject<ShoppingCart>(basket);
        }

        public async Task<ShoppingCart?> UpdateShoppingCart(ShoppingCart basket)
        {
            if (!string.IsNullOrEmpty(basket.UserName))
            {
                await _redisCache.SetStringAsync(basket.UserName, JsonConvert.SerializeObject(basket));
                return await GetShoppingCart(basket.UserName);
            }
            else return null;
           
        }

        public async Task DeleteShoppingCart(string userName)
        {
            await _redisCache.RemoveAsync(userName);
        }
    }
}
