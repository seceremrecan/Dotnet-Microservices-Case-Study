using ProductEntity = Product.Domain.Entities.Product;

namespace Product.Application.Abstractions.Messaging;

public interface IProductEventPublisher
{
    Task PublishProductCreatedAsync(ProductEntity product, CancellationToken cancellationToken = default);
}