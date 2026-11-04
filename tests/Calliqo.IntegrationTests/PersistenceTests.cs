using System.Net;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Calliqo.Api.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Testcontainers.PostgreSql;
using Xunit;

namespace Calliqo.IntegrationTests;

public sealed class PersistenceTests : IAsyncLifetime
{
    private readonly PostgreSqlContainer _postgres =
        new PostgreSqlBuilder("postgres:18.6")
            .WithDatabase("calliqo_tests")
            .WithUsername("calliqo_test")
            .WithPassword("test-only-password")
            .Build();

    public Task InitializeAsync() => _postgres.StartAsync();

    public Task DisposeAsync() => _postgres.DisposeAsync().AsTask();

    [Fact]
    public async Task SavesAndReadsProbeUsingSeparateContexts()
    {
        var id = Guid.NewGuid();
        var createdAt = new DateTimeOffset(2026, 1, 1, 0, 0, 0, TimeSpan.Zero);

        await using (var writer = CreateContext())
        {
            await writer.Database.MigrateAsync();
            writer.PersistenceProbes.Add(new PersistenceProbe
            {
                Id = id,
                CreatedAtUtc = createdAt
            });

            Assert.Equal(1, await writer.SaveChangesAsync());
        }

        await using var reader = CreateContext();
        var saved = await reader.PersistenceProbes
            .AsNoTracking()
            .SingleAsync(probe => probe.Id == id);

        Assert.Equal(id, saved.Id);
        Assert.Equal(createdAt, saved.CreatedAtUtc);
    }

    [Fact]
    public async Task Readiness_ReturnsHealthyWithAvailableDatabase()
    {
        using var factory = new WebApplicationFactory<Program>();

        using var application = factory.WithWebHostBuilder(builder =>
            builder.ConfigureAppConfiguration((_, configuration) =>
            {
                configuration.AddInMemoryCollection(
                    new Dictionary<string, string?>
                    {
                        ["ConnectionStrings:Calliqo"] =
                            _postgres.GetConnectionString(),
                        ["Observability:Enabled"] = "false"
                    });
            }));

        using var client = application.CreateClient(
            new WebApplicationFactoryClientOptions
            {
                BaseAddress = new Uri("https://localhost"),
                AllowAutoRedirect = false
            });

        using var response = await client.GetAsync("/health/ready");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal(
            "Healthy",
            await response.Content.ReadAsStringAsync());
    }
    private CalliqoDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<CalliqoDbContext>()
            .UseNpgsql(_postgres.GetConnectionString())
            .Options;

        return new CalliqoDbContext(options);
    }
}