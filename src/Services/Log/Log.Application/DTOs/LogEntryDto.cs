namespace Log.Application.DTOs;

public class LogEntryDto
{
    public Guid Id { get; set; }
    public string ServiceName { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public string? Exception { get; set; }
    public string Level { get; set; } = string.Empty;
    public DateTime CreatedAtUtc { get; set; }
}