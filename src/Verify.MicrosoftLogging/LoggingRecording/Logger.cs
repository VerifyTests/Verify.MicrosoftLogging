class Logger(string? category, LogLevel level, LoggerProvider provider) :
    ILogger
{
    string? category = category;
    LogLevel level = level;
    LoggerProvider provider = provider;

    public void Log<TState>(LogLevel level, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter) =>
        provider.AddEntry(level, category, eventId, state, exception, formatter);

    public bool IsEnabled(LogLevel level) =>
        level >= this.level;

    public IDisposable BeginScope<TState>(TState state)
        where TState : notnull
    {
        provider.StartScope(state);
        return new LoggerScope(() => provider.EndScope());
    }
}

class Logger<T> :
    Logger,
    ILogger<T>
{
    public Logger(LogLevel level, LoggerProvider provider) :
        base(typeof(T).Name, level, provider)
    {
    }
}