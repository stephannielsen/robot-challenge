using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Time.Testing;
using Namotion.Reflection;
using RobotService.Tests.Helpers;

namespace RobotService.Tests;

public class RobotApiTests
{
    [Fact]
    public async Task CleanPath_ReturnsCreatedOfCleaningResult()
    {
        await using var db = new MockDb().CreateDbContext();
        var fakeTime = new FakeTimeProvider(startDateTime: DateTimeOffset.UtcNow);
        var service = new CleaningService(fakeTime);

        var input = new CleaningPath { Start = new Coordinate { X = 10, Y = 22 }, Commands = [new Command { Direction = Direction.East, Steps = 2 }, new Command { Direction = Direction.North, Steps = 1 }] };
        var result = await Endpoints.CleanPath(input, service, db);

        Assert.IsType<Results<Created<CleaningResult>, BadRequest<ValidateError>>>(result);
        var actual = result.Result.TryGetPropertyValue<CleaningResult>("Value");

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

    [Fact]
    public async Task CleanPath_ReturnsBadRequest ()
    {
        await using var db = new MockDb().CreateDbContext();
        var fakeTime = new FakeTimeProvider(startDateTime: DateTimeOffset.UtcNow);
        var service = new CleaningService(fakeTime);

        var input = new CleaningPath { Start = new Coordinate { X = -100_001, Y = 0 }, Commands = [] };
        var result = await Endpoints.CleanPath(input, service, db);

        Assert.IsType<Results<Created<CleaningResult>, BadRequest<ValidateError>>>(result);
        var actual = result.Result.TryGetPropertyValue<ValidateError>("Value");
        Assert.NotNull(actual);
        var expected = new ValidateError { Message = "Validation error: Start.X must be -100 000 <= X <= 100 000." };
        Assert.Equal(expected, actual);

        Assert.Empty(db.CleaningResults);
    }
}