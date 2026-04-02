using Log.Domain.Entities;

namespace Log.Application.Abstractions.Persistence;

public interface ILogRepository
{
    Task AddAsync(LogEntry logEntry, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<LogEntry>> GetAllAsync(CancellationToken cancellationToken = default);
}