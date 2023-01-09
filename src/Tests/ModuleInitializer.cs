public static class ModuleInitializer
{
    #region Enable

    [ModuleInitializer]
    public static void Initialize() =>
        VerifyMicrosoftLogging.Enable();

    #endregion

    [ModuleInitializer]
    public static void InitializeOther() =>
        VerifyDiffPlex.Initialize();
}