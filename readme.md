# <img src="/src/icon.png" height="30px"> Verify.MicrosoftLogging

[![Build status](https://ci.appveyor.com/api/projects/status/nrbwjnwp2id3k7f8?svg=true)](https://ci.appveyor.com/project/SimonCropp/verify-verify-microsoftlogging)
[![NuGet Status](https://img.shields.io/nuget/v/Verify.MicrosoftLogging.svg)](https://www.nuget.org/packages/Verify.MicrosoftLogging/)

Extends [Verify](https://github.com/VerifyTests/Verify) to allow verification of MicrosoftLogging bits.



## NuGet package

https://nuget.org/packages/Verify.Verify.MicrosoftLogging/


## Usage

Enable once at assembly load time:

<!-- snippet: Enable -->
<a id='snippet-enable'></a>
```cs
VerifyMicrosoftLogging.Enable();
```
<sup><a href='/src/Tests/ModuleInitializer.cs#L6-L8' title='Snippet source file'>snippet source</a> | <a href='#snippet-enable' title='Start of snippet'>anchor</a></sup>
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
    var provider = LoggerRecording.Start();
    ClassThatUsesLogging target = new(provider);

    var result = target.Method();

    return Verify(result);
}

class ClassThatUsesLogging
{
    ILogger logger;

    public ClassThatUsesLogging(ILogger logger)
    {
        this.logger = logger;
    }

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
<sup><a href='/src/Tests/Tests.cs#L61-L95' title='Snippet source file'>snippet source</a> | <a href='#snippet-loggerrecording' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->

Results in:

<!-- snippet: Tests.Logging.verified.txt -->
<a id='snippet-Tests.Logging.verified.txt'></a>
```txt
{
  target: result,
  logs: [
    {
      Level: Warning,
      Message: The log entry
    },
    {
      Message: StartScope,
      State: The scope
    },
    {
      Level: Warning,
      Message: Entry in scope
    },
    {
      Message: EndScope
    }
  ]
}
```
<sup><a href='/src/Tests/Tests.Logging.verified.txt#L1-L20' title='Snippet source file'>snippet source</a> | <a href='#snippet-Tests.Logging.verified.txt' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->


### Typed

A common pattern is to use a type logger (`Logger<T>`). `LoggerProvider` provides a builder method `CreateLogger<T>` to construct a `Logger<T>`:

<!-- snippet: LoggerRecordingTyped -->
<a id='snippet-loggerrecordingtyped'></a>
```cs
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

    public ClassThatUsesTypedLogging(ILogger<ClassThatUsesTypedLogging> logger)
    {
        this.logger = logger;
    }

    public string Method()
    {
        logger.LogWarning("The log entry");
        return "result";
    }
}
```
<sup><a href='/src/Tests/Tests.cs#L6-L36' title='Snippet source file'>snippet source</a> | <a href='#snippet-loggerrecordingtyped' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->

Results in:

<!-- snippet: Tests.LoggingTyped.verified.txt -->
<a id='snippet-Tests.LoggingTyped.verified.txt'></a>
```txt
{
  target: result,
  logs: [
    {
      Level: Warning,
      Category: ClassThatUsesTypedLogging,
      Message: The log entry
    }
  ]
}
```
<sup><a href='/src/Tests/Tests.LoggingTyped.verified.txt#L1-L10' title='Snippet source file'>snippet source</a> | <a href='#snippet-Tests.LoggingTyped.verified.txt' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->


## Icon

[Log](https://thenounproject.com/term/log/324064/) designed by [Ben Davis](https://thenounproject.com/smashicons/) from [The Noun Project](https://thenounproject.com).
