using Log.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Log.API.Data;

public class LogDbContext : DbContext
{
    public LogDbContext(DbContextOptions<LogDbContext> options) : base(options)
    {
    }

    public DbSet<LogEntry> Logs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<LogEntry>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.ServiceName)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(e => e.Level)
                .IsRequired()
                .HasMaxLength(20);

            entity.Property(e => e.Message)
                .IsRequired()
                .HasMaxLength(2000);

            entity.Property(e => e.Exception)
                .HasMaxLength(5000);

            entity.Property(e => e.Timestamp)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasIndex(e => e.ServiceName);
            entity.HasIndex(e => e.Level);
            entity.HasIndex(e => e.Timestamp);
        });
    }
}
