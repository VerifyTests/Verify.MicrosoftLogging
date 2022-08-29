namespace VerifyTests;

public static class VerifyMicrosoftLogging
{
    public static void Enable() =>
        VerifierSettings.RegisterJsonAppender(_ =>
        {
            if (!LoggerRecording.TryFinishRecording(out var entries))
            {
                return null;
            }

            return new("logs", entries!);
        });
}