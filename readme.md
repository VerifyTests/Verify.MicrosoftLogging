# <img src="/src/icon.png" height="30px"> Verify.MicrosoftLogging

[![Discussions](https://img.shields.io/badge/Verify-Discussions-yellow?svg=true&label=)](https://github.com/orgs/VerifyTests/discussions)
[![Build status](https://ci.appveyor.com/api/projects/status/nrbwjnwp2id3k7f8?svg=true)](https://ci.appveyor.com/project/SimonCropp/verify-microsoftlogging)
[![NuGet Status](https://img.shields.io/nuget/v/Verify.MicrosoftLogging.svg)](https://www.nuget.org/packages/Verify.MicrosoftLogging/)

Extends [Verify](https://github.com/VerifyTests/Verify) to allow verification of MicrosoftLogging bits.

**See [Milestones](../../milestones?state=closed) for release notes.**


## NuGet package

https://nuget.org/packages/Verify.MicrosoftLogging/


## Usage

<!-- snippet: Enable -->
<a id='snippet-enable'></a>
```cs
[ModuleInitializer]
public static void Initialize() =>
    VerifyMicrosoftLogging.Initialize();
```
<sup><a href='/src/Tests/ModuleInitializer.cs#L3-L9' title='Snippet source file'>snippet source</a> | <a href='#snippet-enable' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->
Logging Recording allows, when a method is being tested, for any [logging](https://docs.microsoft.com/en-us/dotnet/core/extensions/logging) made as part of that method call to be recorded and verified.

Call `LoggerRecording.Start();` to get an instance of the `LoggerProvider`. `LoggerProvider` implements both `ILogger` and `ILoggerProvider`.

The pass in the `LoggerProvider` instance to a class/method that write log entries:

<!-- snippet: LoggerRecording -->
<a id='snippet-loggerrecording'></a>
```cs
[Fact]
public Task Logging()
{
    Recording.Start();
    var logger = new RecordingLogger();
    var target = new ClassThatUsesLogging(logger);

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
```
<sup><a href='/src/Tests/Tests.cs#L49-L77' title='Snippet source file'>snippet source</a> | <a href='#snippet-loggerrecording' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->

Results in:

<!-- snippet: Tests.Logging.verified.txt -->
<a id='snippet-Tests.Logging.verified.txt'></a>
```txt
{
  target: result,
  log: [
    {
      Warning: The log entry
    },
    {
      Message: StartScope,
      State: The scope
    },
    {
      Warning: Entry in scope
    },
    {
      Message: EndScope
    }
  ]
}
```
<sup><a href='/src/Tests/Tests.Logging.verified.txt#L1-L18' title='Snippet source file'>snippet source</a> | <a href='#snippet-Tests.Logging.verified.txt' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->


### Typed

A common pattern is to use a type logger (`Logger<T>`). `LoggerProvider` provides a builder method `CreateLogger<T>` to construct a `Logger<T>`:

<!-- snippet: LoggerRecordingTyped -->
<a id='snippet-loggerrecordingtyped'></a>
```cs
[Fact]
public Task LoggingTyped()
{
    Recording.Start();
    var logger = RecordingProvider.CreateLogger<ClassThatUsesTypedLogging>();
    var target = new ClassThatUsesTypedLogging(logger);

    var result = target.Method();

    return Verify(result);
}

class ClassThatUsesTypedLogging(ILogger<ClassThatUsesTypedLogging> logger)
{
    public string Method()
    {
        logger.LogWarning("The log entry");
        return "result";
    }
}
```
<sup><a href='/src/Tests/Tests.cs#L5-L28' title='Snippet source file'>snippet source</a> | <a href='#snippet-loggerrecordingtyped' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->

Results in:

<!-- snippet: Tests.LoggingTyped.verified.txt -->
<a id='snippet-Tests.LoggingTyped.verified.txt'></a>
```txt
{
  target: result,
  log: {
    Warning: The log entry,
    Category: ClassThatUsesTypedLogging
  }
}
```
<sup><a href='/src/Tests/Tests.LoggingTyped.verified.txt#L1-L7' title='Snippet source file'>snippet source</a> | <a href='#snippet-Tests.LoggingTyped.verified.txt' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->


## Icon

[Log](https://thenounproject.com/term/log/324064/) designed by [Ben Davis](https://thenounproject.com/smashicons/) from [The Noun Project](https://thenounproject.com).
