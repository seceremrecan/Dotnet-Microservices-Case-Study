using Log.Application.Abstractions.Persistence;
using Log.Domain.Entities;
using Log.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Log.Infrastructure.Repositories;

public class LogRepository : ILogRepository
{
    private readonly LogDbContext _context;

    public LogRepository(LogDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(LogEntry logEntry, CancellationToken cancellationToken = default)
    {
        await _context.Logs.AddAsync(logEntry, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<LogEntry>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Logs
            .OrderByDescending(x => x.CreatedAtUtc)
            .ToListAsync(cancellationToken);
    }
}