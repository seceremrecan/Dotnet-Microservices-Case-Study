using Product.Application.Abstractions.Caching;
using Product.Application.DTOs;

namespace Product.Infrastructure.Caching;

public class InMemoryProductCacheService : IProductCacheService
{
    private static IReadOnlyList<ProductDto>? _cachedProducts;

    public Task<IReadOnlyList<ProductDto>?> GetProductsAsync(CancellationToken cancellationToken = default)
    {
        return Task.FromResult(_cachedProducts);
    }

    public Task SetProductsAsync(IReadOnlyList<ProductDto> products, CancellationToken cancellationToken = default)
    {
        _cachedProducts = products;
        return Task.CompletedTask;
    }

    public Task RemoveProductsAsync(CancellationToken cancellationToken = default)
    {
        _cachedProducts = null;
        return Task.CompletedTask;
    }
}