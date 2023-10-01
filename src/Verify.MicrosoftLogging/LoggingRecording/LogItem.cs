record LogItem(
    LogLevel Level,
    string? Category,
    EventId EventId,
    Exception? Exception,
    string Message,
    object? State);