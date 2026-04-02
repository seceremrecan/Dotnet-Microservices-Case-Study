using Log.Application.Abstractions.Persistence;
using Log.Application.DTOs;

namespace Log.Application.Features.Logs.Queries.GetLogs;

public class GetLogsQueryHandler
{
    private readonly ILogRepository _logRepository;

    public GetLogsQueryHandler(ILogRepository logRepository)
    {
        _logRepository = logRepository;
    }

    public async Task<IReadOnlyList<LogEntryDto>> HandleAsync(CancellationToken cancellationToken = default)
    {
        var logs = await _logRepository.GetAllAsync(cancellationToken);

        return logs
            .Select(log => new LogEntryDto
            {
                Id = log.Id,
                ServiceName = log.ServiceName,
                Message = log.Message,
                Exception = log.Exception,
                Level = log.Level.ToString(),
                CreatedAtUtc = log.CreatedAtUtc
            })
            .ToList();
    }
}