namespace Calliqo.Api.Infrastructure.Persistence;

public sealed class PersistenceProbe
{
    public Guid Id { get; set; }
    public DateTimeOffset CreatedAtUtc { get; set; }
}