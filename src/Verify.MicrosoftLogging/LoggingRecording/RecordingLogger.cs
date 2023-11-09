namespace VerifyTests.MicrosoftLogging;

public class RecordingLogger(string? category = null) :
    ILogger
{
    Logger defaultLogger = new(category);

    public void Log<TState>(LogLevel level, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter) =>
        defaultLogger.Log(level, eventId, state, exception, formatter);

    public bool IsEnabled(LogLevel level) =>
        defaultLogger.IsEnabled(level);

    public IDisposable? BeginScope<TState>(TState state)
        where TState : notnull =>
        defaultLogger.BeginScope(state);
}