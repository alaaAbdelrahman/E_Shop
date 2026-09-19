using Basket.Basket.Models;
using Basket.Data.Converter;
using Basket.Data.Repository;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;
using System.Text.Json.Serialization;

public class CachedBasketRepository(
    IBasketRepository basketRepository,
    IDistributedCache cache) : IBasketRepository
{

    private readonly JsonSerializerOptions _options = new JsonSerializerOptions
    { 
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        Converters = {new ShoppingCartConverter(), new ShoppingCartItemsConverter()}
    };
    public async Task<ShoppingCart> GetBasket(
        string userName,
        bool asNoTracking = true,
        CancellationToken cancellationToken = default)
    {
        // If we need a tracked entity, go directly to DB
        if (!asNoTracking)
        {
            return await basketRepository.GetBasket(
                userName,
                false,
                cancellationToken);
        }

        // 1. Check Cache First
        var cachedBasket = await cache.GetStringAsync(
            userName,
            cancellationToken);

        if (! string.IsNullOrEmpty(cachedBasket))
        {
            
            return  JsonSerializer.Deserialize<ShoppingCart>(
                cachedBasket, _options);
        }

        // 2. Cache Miss -> Database
        var basket = await basketRepository.GetBasket(
            userName,
            true,
            cancellationToken);

        // 3. Store in Cache
        await cache.SetStringAsync(
            userName,
            JsonSerializer.Serialize(basket, _options),
            new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow =
                    TimeSpan.FromMinutes(30)
            },
            cancellationToken);

        // 4. Return
        return basket;
    }

    public async Task<ShoppingCart> CreatBasket(
        ShoppingCart shoppingCart,
        CancellationToken cancellationToken)
    {
        // Save to DB
        var basket = await basketRepository.CreatBasket(
            shoppingCart,
            cancellationToken);

        // Update cache
        await cache.SetStringAsync(
            shoppingCart.UserName,
            JsonSerializer.Serialize(basket, _options),
            new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow =
                    TimeSpan.FromMinutes(30)
            },
            cancellationToken);

        return basket;
    }

    public async Task<bool> DeleteBasket(
        string userName,
        CancellationToken cancellationToken)
    {
        // Delete from DB
        var result = await basketRepository.DeleteBasket(
            userName,
            cancellationToken);

        // Delete from cache only if DB deletion succeeded
        if (result)
        {
            await cache.RemoveAsync(
                userName,
                cancellationToken);
        }

        return result;
     }

    public async Task<int> SaveChangesAsync(
        string? userName = null,
        CancellationToken cancellationToken = default)
    {
        var result = await basketRepository.SaveChangesAsync(
            userName,
            cancellationToken);

        // Invalidate cache after changes
        if (userName is not null)
        {
            await cache.RemoveAsync(
                userName,
                cancellationToken);
        }

        return result;
    }
}