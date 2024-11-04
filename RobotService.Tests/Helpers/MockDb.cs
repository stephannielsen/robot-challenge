using Microsoft.EntityFrameworkCore;

namespace RobotService.Tests.Helpers;

public class MockDb
{
    public static RobotDb CreateDbContext(TimeProvider timeProvider)
    {
        var options = new DbContextOptionsBuilder<RobotDb>()
            .UseInMemoryDatabase($"InMemoryTestDb-{DateTime.Now.ToFileTimeUtc()}")
            .Options;

        return new RobotDb(options, timeProvider);
    }
}