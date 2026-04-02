using Log.Application.Abstractions.Persistence;
using Log.Domain.Entities;
using Log.Domain.Enums;
using MassTransit;
using Shared.Contracts.Events;

namespace Log.Infrastructure.Consumers;

public class ProductCreatedEventConsumer : IConsumer<ProductCreatedEvent>
{
    private readonly ILogRepository _logRepository;

    public ProductCreatedEventConsumer(ILogRepository logRepository)
    {
        _logRepository = logRepository;
    }

    public async Task Consume(ConsumeContext<ProductCreatedEvent> context)
    {
        var message = context.Message;

        var logEntry = new LogEntry
        {
            ServiceName = "Product.API",
            Message = $"Product created: {message.Name} (Id: {message.ProductId})",
            Level = LogLevelType.Info,
            Exception = null
        };

        await _logRepository.AddAsync(logEntry, context.CancellationToken);
    }
}