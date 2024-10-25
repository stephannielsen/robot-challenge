using Microsoft.EntityFrameworkCore;

public class RobotDb : DbContext
{
    public RobotDb(DbContextOptions<RobotDb> options)
        : base(options) { }

    public DbSet<CleaningResult> CleaningResults => Set<CleaningResult>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Using non-UTC timestamps to conform with requirements
        // normally would be using "timestamp with time zone" as preferred type (which is the default) for timestamps in postgres 
        modelBuilder.Entity<CleaningResult>().Property(r => r.Timestamp).HasColumnType("timestamp without time zone");
    }
}
