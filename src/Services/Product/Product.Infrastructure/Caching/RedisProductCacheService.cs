using System.Text.Json;
using Product.Application.Abstractions.Caching;
using Product.Application.DTOs;
using StackExchange.Redis;

namespace Product.Infrastructure.Caching;

public class RedisProductCacheService : IProductCacheService
{
    private const string ProductsCacheKey = "products:list";
    private readonly IConnectionMultiplexer _connectionMultiplexer;

    public RedisProductCacheService(IConnectionMultiplexer connectionMultiplexer)
    {
        _connectionMultiplexer = connectionMultiplexer;
    }

    public async Task<IReadOnlyList<ProductDto>?> GetProductsAsync(CancellationToken cancellationToken = default)
    {
        var db = _connectionMultiplexer.GetDatabase();
        var cachedValue = await db.StringGetAsync(ProductsCacheKey);

        if (cachedValue.IsNullOrEmpty)
        {
            return null;
        }

        return JsonSerializer.Deserialize<IReadOnlyList<ProductDto>>(cachedValue!);
    }

    public async Task SetProductsAsync(IReadOnlyList<ProductDto> products, CancellationToken cancellationToken = default)
    {
        var db = _connectionMultiplexer.GetDatabase();
        var serialized = JsonSerializer.Serialize(products);

        await db.StringSetAsync(ProductsCacheKey, serialized, TimeSpan.FromMinutes(10));
    }

    public async Task RemoveProductsAsync(CancellationToken cancellationToken = default)
    {
        var db = _connectionMultiplexer.GetDatabase();
        await db.KeyDeleteAsync(ProductsCacheKey);
    }
}