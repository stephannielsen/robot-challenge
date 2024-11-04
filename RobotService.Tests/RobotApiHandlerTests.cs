using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Time.Testing;
using Namotion.Reflection;
using RobotService.Tests.Helpers;

namespace RobotService.Tests;

public class RobotApiHandlerTests
{
    [Fact]
    public async Task CalculateCleaningPath_SavesResult_ReturnsResult()
    {
        var fakeTime = new FakeTimeProvider(startDateTime: DateTimeOffset.UtcNow);
        await using var db = MockDb.CreateDbContext(fakeTime);
        var service = new CleaningService();
        var inputPath = new CleaningPath { Start = new Coordinate { X = 10, Y = 22 }, Commands = [new Command { Direction = Direction.East, Steps = 2 }, new Command { Direction = Direction.North, Steps = 1 }] };

        var response = await RobotApiHandler.CalculateCleaningPath(inputPath, service, db);

        Assert.IsType<Results<Created<CleaningResult>, BadRequest<ValidationProblem>>>(response);
        var actual = response.Result.TryGetPropertyValue<CleaningResult>("Value");

        Assert.NotNull(actual);

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