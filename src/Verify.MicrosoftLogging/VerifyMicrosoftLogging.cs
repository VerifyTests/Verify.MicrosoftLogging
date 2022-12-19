namespace VerifyTests;

public static class VerifyMicrosoftLogging
{
    public static void Enable()
    {
        InnerVerifier.ThrowIfVerifyHasBeenRun();
        VerifierSettings.AddExtraSettings(settings =>
        {
            var converters = settings.Converters;
            converters.Add(new LogItemConverter());
        });
        VerifierSettings.RegisterJsonAppender(_ =>
        {
            if (!LoggerRecording.TryFinishRecording(out var entries))
            {
                return null;
            }

            return new("logs", entries!);
        });
    }
}