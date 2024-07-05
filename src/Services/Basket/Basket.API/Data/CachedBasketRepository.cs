
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace Basket.API.Data
{
    public class CachedBasketRepository
        (IBasketRepository _repository, IDistributedCache _cache)
        : IBasketRepository
    {
        public async Task<bool> DeleteBasket(Guid userId, CancellationToken cancellationToken = default)
        {

             await _repository.DeleteBasket(userId, cancellationToken);
             await _cache.RemoveAsync(userId.ToString(), cancellationToken);
            return true;
     
        }

        public async Task<ShoppingCart> GetBasket(Guid userId, CancellationToken cancellationToken = default)
        {
           var cachedBasket  = await  _cache.GetStringAsync(userId.ToString(), cancellationToken);
            if (!string.IsNullOrEmpty(cachedBasket))
            {
                return JsonSerializer.Deserialize<ShoppingCart>(cachedBasket)!;
            }
            var basket = await _repository.GetBasket(userId, cancellationToken);
            await _cache.SetStringAsync(userId.ToString(), JsonSerializer.Serialize(basket), cancellationToken);
            return basket;
        }

        public async Task<ShoppingCart> StoreBasket(ShoppingCart basket, CancellationToken cancellationToken = default)
        {
            var result =  await _repository.StoreBasket(basket, cancellationToken);
            await _cache.SetStringAsync(result.UserId.ToString(), JsonSerializer.Serialize(result));
            return result;
        }
    }
}
