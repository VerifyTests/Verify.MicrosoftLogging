public static class ModuleInitializer
{
    [ModuleInitializer]
    public static void Initialize()
    {
        #region Enable
        VerifyMicrosoftLogging.Enable();
        #endregion
    }
}

#if(NET48)
namespace System.Runtime.CompilerServices
{
    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    public sealed class ModuleInitializerAttribute : Attribute
    {
    }
}
#endif