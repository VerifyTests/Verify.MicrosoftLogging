namespace VerifyTests.MicrosoftLogging;

public class RecordingProvider :
    ILoggerProvider
{
    public void Dispose()
    {
    }

    public ILogger CreateLogger(string category) =>
        new RecordingLogger(category);

    public ILogger<T> CreateLogger<T>() =>
        new RecordingLogger<T>();
}