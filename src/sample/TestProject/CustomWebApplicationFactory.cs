using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using VerifyTests.MicrosoftLogging;

namespace TestProject
{
    internal class CustomWebApplicationFactory : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder) => ConfigureVerifyLoggingV4(builder);

        void ConfigureVerifyLoggingV4(IWebHostBuilder builder) =>
            builder.ConfigureTestServices(services =>
            {
                services.AddLogging(options =>
                {
                    options.ClearProviders();

                    Recording.Start();
                    var verifyLoggerProvider = new RecordingProvider();
                    options.AddProvider(verifyLoggerProvider);
                });
            });
    }
}