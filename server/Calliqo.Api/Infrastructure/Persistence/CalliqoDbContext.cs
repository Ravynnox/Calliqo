using Microsoft.EntityFrameworkCore;

namespace Calliqo.Api.Infrastructure.Persistence;

public sealed class CalliqoDbContext : DbContext
{
    public CalliqoDbContext(DbContextOptions<CalliqoDbContext> options)
        : base(options)
    {
    }

    public DbSet<PersistenceProbe> PersistenceProbes => Set<PersistenceProbe>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        var probe = modelBuilder.Entity<PersistenceProbe>();
        probe.ToTable("persistence_probes", "infrastructure");
        probe.HasKey(x => x.Id);
        probe.Property(x => x.Id).HasColumnName("id");
        probe.Property(x => x.CreatedAtUtc)
            .HasColumnName("created_at_utc")
            .HasColumnType("timestamp with time zone");
    }
}