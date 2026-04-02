using Product.Application.Abstractions.Caching;
using Product.Application.Abstractions.Persistence;
using Product.Application.DTOs;

namespace Product.Application.Features.Products.Queries.GetProducts;

public class GetProductsQueryHandler
{
    private readonly IProductRepository _productRepository;
    private readonly IProductCacheService _productCacheService;

    public GetProductsQueryHandler(
        IProductRepository productRepository,
        IProductCacheService productCacheService)
    {
        _productRepository = productRepository;
        _productCacheService = productCacheService;
    }

    public async Task<IReadOnlyList<ProductDto>> HandleAsync(CancellationToken cancellationToken = default)
    {
        var cachedProducts = await _productCacheService.GetProductsAsync(cancellationToken);

        if (cachedProducts is not null)
        {
            return cachedProducts;
        }

        var products = await _productRepository.GetAllAsync(cancellationToken);

        var result = products
            .Select(product => new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Stock = product.Stock
            })
            .ToList();

        await _productCacheService.SetProductsAsync(result, cancellationToken);

        return result;
    }
}