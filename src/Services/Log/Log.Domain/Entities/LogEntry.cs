using Log.Domain.Common;
using Log.Domain.Enums;

namespace Log.Domain.Entities;

public class LogEntry : BaseEntity
{
    public string ServiceName { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public string? Exception { get; set; }
    public LogLevelType Level { get; set; }
}