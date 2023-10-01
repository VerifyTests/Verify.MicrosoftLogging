class LoggerScope(Action log) :
    IDisposable
{
    public void Dispose() =>
        log();
}