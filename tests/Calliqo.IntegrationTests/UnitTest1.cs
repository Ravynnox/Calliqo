using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace Calliqo.IntegrationTests;

public class WeatherForecastTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public WeatherForecastTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task GetWeatherForecast_ReturnsSuccessfulJsonArray()
    {
        using var client = _factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            BaseAddress = new Uri("https://localhost"),
            AllowAutoRedirect = false
        });
        using var response = await client.GetAsync("/api/v1/weatherforecast");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal("application/json", response.Content.Headers.ContentType?.MediaType);

        using var document = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        Assert.Equal(JsonValueKind.Array, document.RootElement.ValueKind);
        Assert.NotEmpty(document.RootElement.EnumerateArray());

        foreach (var forecast in document.RootElement.EnumerateArray())
        {
            Assert.Equal(JsonValueKind.String, forecast.GetProperty("date").ValueKind);
            Assert.Equal(JsonValueKind.Number, forecast.GetProperty("temperatureC").ValueKind);
        }
    }
}