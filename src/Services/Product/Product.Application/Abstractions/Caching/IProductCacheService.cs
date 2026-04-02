using Product.Application.DTOs;

namespace Product.Application.Abstractions.Caching;

public interface IProductCacheService
{
    Task<IReadOnlyList<ProductDto>?> GetProductsAsync(CancellationToken cancellationToken = default);
    Task SetProductsAsync(IReadOnlyList<ProductDto> products, CancellationToken cancellationToken = default);
    Task RemoveProductsAsync(CancellationToken cancellationToken = default);
}