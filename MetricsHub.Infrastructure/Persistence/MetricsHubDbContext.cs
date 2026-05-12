using MetricsHub.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace MetricsHub.Infrastructure.Persistence
{
    public class MetricsHubDbContext : DbContext
    {
        public DbSet<IngestionEventEntity> IngestionEvents { get; set; }

        public MetricsHubDbContext(DbContextOptions<MetricsHubDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<IngestionEventEntity>(b =>
            {
                b.HasKey(x => x.EventId);

                b.Property(x => x.EventId)
                    .IsRequired();

                b.HasIndex(x => x.EventId)
                    .IsUnique();

                b.HasIndex(x => x.CorrelationId);
            });
        }
    }
}
