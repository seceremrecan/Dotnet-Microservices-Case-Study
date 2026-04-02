using LogEntryEntity = Log.Domain.Entities.LogEntry;
using Microsoft.EntityFrameworkCore;

namespace Log.Infrastructure.Persistence;

public class LogDbContext : DbContext
{
    public LogDbContext(DbContextOptions<LogDbContext> options) : base(options)
    {
    }

    public DbSet<LogEntryEntity> Logs => Set<LogEntryEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<LogEntryEntity>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.Property(x => x.ServiceName)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(x => x.Message)
                .IsRequired()
                .HasMaxLength(2000);

            entity.Property(x => x.Exception)
                .HasMaxLength(4000);

            entity.Property(x => x.Level)
                .IsRequired();
        });
    }
}