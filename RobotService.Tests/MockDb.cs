using Microsoft.EntityFrameworkCore;

namespace RobotService.Tests.Helpers;

public class MockDb : IDbContextFactory<RobotDb>
{
    public RobotDb CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<RobotDb>()
            .UseInMemoryDatabase($"InMemoryTestDb-{DateTime.Now.ToFileTimeUtc()}")
            .Options;

        return new RobotDb(options);
    }
}