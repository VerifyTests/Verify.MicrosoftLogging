using VerifyTests.MicrosoftLogging;

namespace VerifyTests;

public class LoggerProvider :
    ILoggerProvider,
    ILogger
{
    internal ConcurrentQueue<object> entries = new();
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
        if (state is IReadOnlyList<KeyValuePair<string, object>> {Count: 1} dictionary &&
            dictionary.First().Key == "{OriginalFormat}")
        {
            LogItem entry1 = new(level, category, eventId, exception, message, null);
            entries.Enqueue(entry1);
            return;
        }
        LogItem entry = new(level, category, eventId, exception, message, state);
        entries.Enqueue(entry);
    }

    public void Log<TState>(LogLevel level, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter) =>
        defaultLogger.Log(level, eventId, state, exception, formatter);

    public bool IsEnabled(LogLevel level) =>
        defaultLogger.IsEnabled(level);

    public IDisposable? BeginScope<TState>(TState state)
        where TState : notnull =>
        defaultLogger.BeginScope(state);

    internal void EndScope() =>
        entries.Enqueue(new ScopeEntry("EndScope", null));

    internal void StartScope<TState>(TState state) =>
        entries.Enqueue(new ScopeEntry("StartScope", state!));
}