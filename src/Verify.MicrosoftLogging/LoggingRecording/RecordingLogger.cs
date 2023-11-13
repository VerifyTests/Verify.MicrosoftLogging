namespace VerifyTests.MicrosoftLogging;

public class RecordingLogger(string? category = null) :
    ILogger
{
    public void Log<TState>(LogLevel level, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        var message = formatter.Invoke(state, exception);
        if (IsOriginalFormat(state))
        {
            var entry = new LogItem(level, category, eventId, exception, message, null);
            Recording.Add("log", entry);
        }
        else
        {
            var entry = new LogItem(level, category, eventId, exception, message, state);
            Recording.Add("log", entry);
        }
    }

    static bool IsOriginalFormat<TState>(TState state) =>
        state is IReadOnlyList<KeyValuePair<string, object>> {Count: 1} dictionary &&
        dictionary.First().Key == "{OriginalFormat}";

    public bool IsEnabled(LogLevel level) =>
        true;

    public IDisposable BeginScope<TState>(TState state)
        where TState : notnull
    {
        Recording.Add("log", new ScopeEntry("StartScope", state));
        return new LoggerScope(EndScope);
    }

    static void EndScope() =>
        Recording.Add("log", new ScopeEntry("EndScope", null));
}

class RecordingLogger<T>() :
    RecordingLogger(typeof(T).Name),
    ILogger<T>;