using Microsoft.Extensions.Time.Testing;
using RobotService.Tests.Helpers;

namespace RobotService.Tests;

public class CleaningServiceTests
{
    [Fact]
    public async Task CalculateResult_ReturnsResult_SavesResultInDb()
    {
        await using var db = new MockDb().CreateDbContext();
        var fakeTime = new FakeTimeProvider(startDateTime: DateTimeOffset.UtcNow);
        var service = new CleaningService(fakeTime);
        var inputPath = new CleaningPath { Start = new Coordinate { X = 10, Y = 22 }, Commands = [new Command { Direction = Direction.East, Steps = 2 }, new Command { Direction = Direction.North, Steps = 1 }] };

        CleaningResult actual = await service.CalculateResult(inputPath, db);

        Assert.Equal(2, actual.Commands);
        Assert.Equal(4, actual.Result);
        Assert.IsType<int>(actual.ID);
        Assert.Equal(fakeTime.GetUtcNow().DateTime, actual.Timestamp);
        Assert.IsType<double>(actual.Duration);

        Assert.NotEmpty(db.CleaningResults);
        Assert.Collection(db.CleaningResults, result =>
        {
            Assert.Equal(2, result.Commands);
            Assert.Equal(4, result.Result);
            Assert.IsType<int>(result.ID);
            Assert.Equal(fakeTime.GetUtcNow().DateTime, result.Timestamp);
            Assert.IsType<double>(result.Duration);
        });
    }
}