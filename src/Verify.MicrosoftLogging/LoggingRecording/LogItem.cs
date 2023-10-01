class LogItem(
    LogLevel level,
    string? category,
    EventId eventId,
    Exception? exception,
    string message,
    object? state)
{
    public LogLevel Level { get; } = level;
    public string? Category { get; } = category;
    public string Message { get; } = message;
    public object? State { get; } = state;
    public EventId EventId { get; } = eventId;
    public Exception? Exception { get; } = exception;
}