namespace VerifyTests;

public class LoggerProvider :
    ILoggerProvider,
    ILogger
{
    Logger defaultLogger;

    public LoggerProvider() =>
        defaultLogger = new(null, this);

    public void Dispose()
    {
    }

    public ILogger CreateLogger(string category) =>
        new Logger(category, this);

    public ILogger<T> CreateLogger<T>() =>
        new Logger<T>(this);

    internal void AddEntry<TState>(LogLevel level, string? category, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        var message = formatter.Invoke(state, exception);
        if (IsOriginalFormat(state))
        {
            var entry = new LogItem(level, category, eventId, exception, message, null);
            Recording.Add("logs", entry);
        }
        else
        {
            var entry = new LogItem(level, category, eventId, exception, message, state);
            Recording.Add("logs", entry);
        }
    }

    static bool IsOriginalFormat<TState>(TState state) =>
        state is IReadOnlyList<KeyValuePair<string, object>> {Count: 1} dictionary &&
        dictionary.First().Key == "{OriginalFormat}";

    public void Log<TState>(LogLevel level, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter) =>
        defaultLogger.Log(level, eventId, state, exception, formatter);

    public bool IsEnabled(LogLevel level) =>
        defaultLogger.IsEnabled(level);

    public IDisposable? BeginScope<TState>(TState state)
        where TState : notnull =>
        defaultLogger.BeginScope(state);

    internal void EndScope() =>
        Recording.Add("logs", new ScopeEntry("EndScope", null));

    internal void StartScope<TState>(TState state) =>
        Recording.Add("logs", new ScopeEntry("StartScope", state!));
}