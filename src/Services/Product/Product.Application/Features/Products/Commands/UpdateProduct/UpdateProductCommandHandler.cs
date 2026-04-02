using Product.Application.Abstractions.Caching;
using Product.Application.Abstractions.Persistence;

namespace Product.Application.Features.Products.Commands.UpdateProduct;

public class UpdateProductCommandHandler
{
    private readonly IProductRepository _productRepository;
    private readonly IProductCacheService _productCacheService;

    public UpdateProductCommandHandler(
        IProductRepository productRepository,
        IProductCacheService productCacheService)
    {
        _productRepository = productRepository;
        _productCacheService = productCacheService;
    }

    public async Task<bool> HandleAsync(UpdateProductCommand command, CancellationToken cancellationToken = default)
    {
        var existingProduct = await _productRepository.GetByIdAsync(command.Id, cancellationToken);

        if (existingProduct is null)
        {
            return false;
        }

        existingProduct.Name = command.Name;
        existingProduct.Description = command.Description;
        existingProduct.Price = command.Price;
        existingProduct.Stock = command.Stock;
        existingProduct.UpdatedAtUtc = DateTime.UtcNow;

        await _productRepository.UpdateAsync(existingProduct, cancellationToken);
        await _productCacheService.RemoveProductsAsync(cancellationToken);

        return true;
    }
}