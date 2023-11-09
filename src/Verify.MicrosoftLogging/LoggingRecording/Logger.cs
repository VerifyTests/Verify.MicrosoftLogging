class Logger(string? category) :
    ILogger
{
    public void Log<TState>(LogLevel level, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter) =>
        LoggerProvider.AddEntry(level, category, eventId, state, exception, formatter);

    public bool IsEnabled(LogLevel level) =>
        true;

    public IDisposable BeginScope<TState>(TState state)
        where TState : notnull
    {
        LoggerProvider.StartScope(state);
        return new LoggerScope(LoggerProvider.EndScope);
    }
}

class Logger<T>() :
    Logger(typeof(T).Name),
    ILogger<T>;