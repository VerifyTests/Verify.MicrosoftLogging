[UsesVerify]
public class Tests
{
    #region LoggerRecordingTyped

    [Fact]
    public Task LoggingTyped()
    {
        var provider = LoggerRecording.Start();
        var logger = provider.CreateLogger<ClassThatUsesTypedLogging>();
        ClassThatUsesTypedLogging target = new(logger);

        var result = target.Method();

        return Verify(result);
    }

    class ClassThatUsesTypedLogging
    {
        ILogger<ClassThatUsesTypedLogging> logger;

        public ClassThatUsesTypedLogging(ILogger<ClassThatUsesTypedLogging> logger) =>
            this.logger = logger;

        public string Method()
        {
            logger.LogWarning("The log entry");
            return "result";
        }
    }

    #endregion

    [Fact]
    public Task LoggingComplexState()
    {
        var provider = LoggerRecording.Start();
        provider.Log(LogLevel.Warning, default, new StateObject("Value1"), null, (_, _) => "The Message");
        using (provider.BeginScope(new StateObject("Value2")))
        {
            provider.Log(LogLevel.Warning, default, new StateObject("Value3"), null, (_, _) => "Entry in scope");
        }

        return Verify("Foo");
    }

    class StateObject(string property)
    {
        public string Property { get; } = property;
    }

    #region LoggerRecording

    [Fact]
    public Task Logging()
    {
        var provider = LoggerRecording.Start();
        var target = new ClassThatUsesLogging(provider);

        var result = target.Method();

        return Verify(result);
    }

    class ClassThatUsesLogging(ILogger logger)
    {
        public string Method()
        {
            logger.LogWarning("The log entry");
            using (logger.BeginScope("The scope"))
            {
                logger.LogWarning("Entry in scope");
            }

            return "result";
        }
    }

    #endregion
}