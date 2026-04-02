using Product.Application.Abstractions.Caching;
using Product.Application.Abstractions.Messaging;
using Product.Application.Abstractions.Persistence;
using ProductEntity = Product.Domain.Entities.Product;

namespace Product.Application.Features.Products.Commands.CreateProduct;

public class CreateProductCommandHandler
{
    private readonly IProductRepository _productRepository;
    private readonly IProductEventPublisher _productEventPublisher;
    private readonly IProductCacheService _productCacheService;

    public CreateProductCommandHandler(
        IProductRepository productRepository,
        IProductEventPublisher productEventPublisher,
        IProductCacheService productCacheService)
    {
        _productRepository = productRepository;
        _productEventPublisher = productEventPublisher;
        _productCacheService = productCacheService;
    }

    public async Task<Guid> HandleAsync(CreateProductCommand command, CancellationToken cancellationToken = default)
    {
        var product = new ProductEntity
        {
            Name = command.Name,
            Description = command.Description,
            Price = command.Price,
            Stock = command.Stock
        };

        await _productRepository.AddAsync(product, cancellationToken);
        await _productEventPublisher.PublishProductCreatedAsync(product, cancellationToken);
        await _productCacheService.RemoveProductsAsync(cancellationToken);

        return product.Id;
    }
}