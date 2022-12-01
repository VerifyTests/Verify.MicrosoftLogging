namespace VerifyTests;

public static class VerifyMicrosoftLogging
{
    public static void Enable() =>
        VerifierSettings.RegisterJsonAppender(_ =>
        {
            VerifierSettings.AddExtraSettings(settings =>
            {
                var converters = settings.Converters;
                converters.Add(new LogItemConverter());
            });

            if (!LoggerRecording.TryFinishRecording(out var entries))
            {
                return null;
            }

            return new("logs", entries!);
        });
}