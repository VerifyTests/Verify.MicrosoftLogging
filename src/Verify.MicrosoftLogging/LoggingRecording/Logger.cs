﻿class Logger(string? category) :
    ILogger
{
    public void Log<TState>(LogLevel level, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
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

    public bool IsEnabled(LogLevel level) =>
        true;

    public IDisposable BeginScope<TState>(TState state)
        where TState : notnull
    {
        Recording.Add("logs", new ScopeEntry("StartScope", state));
        return new LoggerScope(EndScope);
    }

    static void EndScope() =>
        Recording.Add("logs", new ScopeEntry("EndScope", null));
}

class Logger<T>() :
    Logger(typeof(T).Name),
    ILogger<T>;