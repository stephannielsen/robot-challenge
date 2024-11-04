using Microsoft.EntityFrameworkCore;

namespace RobotService;

public class RobotDb(DbContextOptions<RobotDb> options, TimeProvider timeProvider) : DbContext(options)
{
    public DbSet<CleaningResult> CleaningResults => Set<CleaningResult>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Using non-UTC timestamps to conform with requirements
        // normally would be using "timestamp with time zone" as preferred type (which is the default) for timestamps in postgres 
        modelBuilder.Entity<CleaningResult>().Property(r => r.Timestamp).HasColumnType("timestamp without time zone");
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var changeSet = ChangeTracker.Entries<CleaningResult>();
        if (changeSet != null)
        {
            foreach (var change in changeSet.Where(c => c.State == EntityState.Added)) {
                change.Entity.Timestamp = timeProvider.GetUtcNow().DateTime;
            }
        }
        return base.SaveChangesAsync(cancellationToken);
    }
}
