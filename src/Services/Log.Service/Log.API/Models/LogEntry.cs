namespace Log.API.Models;

public class LogEntry
{
    public int Id { get; set; }
    public string ServiceName { get; set; } = string.Empty;
    public string Level { get; set; } = string.Empty; // INFO, WARNING, ERROR, CRITICAL
    public string Message { get; set; } = string.Empty;
    public string? Exception { get; set; }
    public DateTime Timestamp { get; set; }
}
