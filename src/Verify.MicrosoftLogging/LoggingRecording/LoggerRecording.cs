namespace VerifyTests;

public static class LoggerRecording
{
    static AsyncLocal<LoggerProvider?> local = new();

    public static LoggerProvider Start() =>
        local.Value = new();
}