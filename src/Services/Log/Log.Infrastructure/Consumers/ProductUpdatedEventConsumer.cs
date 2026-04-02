using Log.Application.Abstractions.Persistence;
using Log.Domain.Entities;
using Log.Domain.Enums;
using MassTransit;
using Shared.Contracts.Events;

namespace Log.Infrastructure.Consumers;

public class ProductUpdatedEventConsumer : IConsumer<ProductUpdatedEvent>
{
    private readonly ILogRepository _logRepository;

    public ProductUpdatedEventConsumer(ILogRepository logRepository)
    {
        _logRepository = logRepository;
    }

    public async Task Consume(ConsumeContext<ProductUpdatedEvent> context)
    {
        var message = context.Message;

        var logEntry = new LogEntry
        {
            ServiceName = "Product.API",
            Message = $"Product updated: {message.Name} (Id: {message.ProductId})",
            Level = LogLevelType.Warning,
            Exception = null
        };

        await _logRepository.AddAsync(logEntry, context.CancellationToken);
    }
}