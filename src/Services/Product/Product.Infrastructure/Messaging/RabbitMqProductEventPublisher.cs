using MassTransit;
using Product.Application.Abstractions.Messaging;
using ProductEntity = Product.Domain.Entities.Product;
using Shared.Contracts.Events;

namespace Product.Infrastructure.Messaging;

public class RabbitMqProductEventPublisher : IProductEventPublisher
{
    private readonly IPublishEndpoint _publishEndpoint;

    public RabbitMqProductEventPublisher(IPublishEndpoint publishEndpoint)
    {
        _publishEndpoint = publishEndpoint;
    }

    public async Task PublishProductCreatedAsync(ProductEntity product, CancellationToken cancellationToken = default)
    {
        var productCreatedEvent = new ProductCreatedEvent
        {
            ProductId = product.Id,
            Name = product.Name,
            Price = product.Price,
            Stock = product.Stock,
            OccurredAtUtc = DateTime.UtcNow
        };

        await _publishEndpoint.Publish(productCreatedEvent, cancellationToken);
    }

    public async Task PublishProductUpdatedAsync(ProductEntity product, CancellationToken cancellationToken = default)
    {
        var productUpdatedEvent = new ProductUpdatedEvent
        {
            ProductId = product.Id,
            Name = product.Name,
            Price = product.Price,
            Stock = product.Stock,
            OccurredAtUtc = DateTime.UtcNow
        };

        await _publishEndpoint.Publish(productUpdatedEvent, cancellationToken);
    }
}