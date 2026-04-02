using Log.Domain.Enums;

namespace Log.Application.Features.Logs.Commands.CreateLog;

public class CreateLogCommand
{
    public string ServiceName { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public string? Exception { get; set; }
    public LogLevelType Level { get; set; }
}