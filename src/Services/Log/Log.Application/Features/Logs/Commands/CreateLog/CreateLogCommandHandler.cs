using Log.Application.Abstractions.Persistence;
using Log.Domain.Entities;

namespace Log.Application.Features.Logs.Commands.CreateLog;

public class CreateLogCommandHandler
{
    private readonly ILogRepository _logRepository;

    public CreateLogCommandHandler(ILogRepository logRepository)
    {
        _logRepository = logRepository;
    }

    public async Task<Guid> HandleAsync(CreateLogCommand command, CancellationToken cancellationToken = default)
    {
        var logEntry = new LogEntry
        {
            ServiceName = command.ServiceName,
            Message = command.Message,
            Exception = command.Exception,
            Level = command.Level
        };

        await _logRepository.AddAsync(logEntry, cancellationToken);

        return logEntry.Id;
    }
}