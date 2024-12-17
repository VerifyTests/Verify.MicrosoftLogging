namespace TestProject
{
    public class WeatherforecastTests
    {
        readonly CustomWebApplicationFactory applicationFactory = new();


        [Fact(Timeout = 3000)] // 👈 Needs this timeout because the Exception is throw "'System.Exception' in Verify.dll" and the run stay frezzing on "client.GetAsync"
        public async Task Test1()
        {
            // Arrange
            var client = applicationFactory.CreateClient();

            // Act
            var result = await client.GetAsync("/weatherforecast");

            // Assert
            await Verify(new { result.StatusCode });
        }
    }
}