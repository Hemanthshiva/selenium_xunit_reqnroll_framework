using Polly;
using Newtonsoft.Json.Linq;

namespace selenium_xunit_reqnroll_framework.ApiTests;

public class ProductsApiTests
{
    private static readonly HttpClient client = new();

    [Fact]
    public async Task GetAllProductsList_ShouldReturn200AndValidJson()
    {
        var policy = Policy
            .Handle<Exception>()
            .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(1));

        await policy.ExecuteAsync(async () =>
        {
            var url = "https://automationexercise.com/api/productsList";
            var response = await client.GetAsync(url);
            var content = await response.Content.ReadAsStringAsync();

            Assert.Equal(200, (int)response.StatusCode);
            Assert.False(string.IsNullOrWhiteSpace(content));

            var json = JObject.Parse(content);
            Assert.True(json["products"] != null, "Response JSON should contain 'products' key.");
        });
    }
}
