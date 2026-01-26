using AuditLogs.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace AuditLogs.Api.Data;

public class AppDbContext :DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }

    public DbSet<AuditLogEntry> AuditLogEntries => Set<AuditLogEntry>();


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AuditLogEntry>(entity =>
        {
            entity.Property(x => x.Action)
                .HasMaxLength(100)
                .IsRequired();
            
            entity.Property(x => x.Entity)
                .HasMaxLength(100)
                .IsRequired();
            
            entity.Property(x => x.PerformedBy)
                .HasMaxLength(200)
                .IsRequired();

            entity.HasIndex(x => x.OccuredOnUtc);
        });
    }
}