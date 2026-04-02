using Product.Application.Abstractions.Messaging;
using ProductEntity = Product.Domain.Entities.Product;

namespace Product.Infrastructure.Messaging;

public class FakeProductEventPublisher : IProductEventPublisher
{
    public Task PublishProductCreatedAsync(ProductEntity product, CancellationToken cancellationToken = default)
    {
        Console.WriteLine($"ProductCreated event published for product: {product.Name} - {product.Id}");
        return Task.CompletedTask;
    }
}