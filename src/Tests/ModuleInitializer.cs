public static class ModuleInitializer
{
    #region Enable

    [ModuleInitializer]
    public static void Initialize()
       { VerifyMicrosoftLogging.Enable();

    #endregion
        VerifyDiffPlex.Initialize();
       }
}