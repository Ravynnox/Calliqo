using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace Calliqo.IntegrationTests;

public sealed class OpenApiTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public OpenApiTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task Document_DescribesVersionedForecastEndpoint()
    {
        using var application = _factory.WithWebHostBuilder(builder =>
            builder.UseEnvironment("Development"));
        using var client = application.CreateClient(new WebApplicationFactoryClientOptions
        {
            BaseAddress = new Uri("https://localhost"),
            AllowAutoRedirect = false
        });

        using var response = await client.GetAsync("/openapi/v1.json");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        using var document = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        var paths = document.RootElement.GetProperty("paths");
        Assert.True(paths.TryGetProperty("/api/v1/weatherforecast", out var endpoint));
        var operation = endpoint.GetProperty("get");
        Assert.Equal("GetWeatherForecastV1", operation.GetProperty("operationId").GetString());
        Assert.False(paths.TryGetProperty("/weatherforecast", out _));
    }
}