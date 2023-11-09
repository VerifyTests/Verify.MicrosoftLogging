class Logger(string? category, LoggerProvider provider) :
    ILogger
{
    string? category = category;
    LoggerProvider provider = provider;

    public void Log<TState>(LogLevel level, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter) =>
        provider.AddEntry(level, category, eventId, state, exception, formatter);

    public bool IsEnabled(LogLevel level) =>
        true;

    public IDisposable BeginScope<TState>(TState state)
        where TState : notnull
    {
        provider.StartScope(state);
        return new LoggerScope(() => provider.EndScope());
    }
}

class Logger<T>(LoggerProvider provider) :
    Logger(typeof(T).Name, provider),
    ILogger<T>;