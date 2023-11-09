namespace VerifyTests;

public static class LoggerRecording
{
    static AsyncLocal<LoggerProvider?> local = new();

    public static LoggerProvider Start() =>
        local.Value = new();

    public static bool TryFinishRecording(out IEnumerable<object>? entries)
    {
        var provider = local.Value;

        if (provider is null)
        {
            local.Value = null;
            entries = null;
            return false;
        }

        entries = provider.entries.ToArray();
        local.Value = null;
        return true;
    }
}