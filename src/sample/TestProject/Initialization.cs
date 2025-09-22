using System.Runtime.CompilerServices;

namespace TestProject
{
    public static class Initialization
    {
        [ModuleInitializer]
        public static void Initialize() => VerifyMicrosoftLogging.Initialize();
    }
}
