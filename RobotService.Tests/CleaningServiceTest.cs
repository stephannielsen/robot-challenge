
using Microsoft.Extensions.Time.Testing;

namespace RobotService.Tests;

public class CleaningServiceTests
{
    [Fact]
    public async void CalculateReturnsResult()
    {
        var input = new CleaningPath { Start = new Coordinate { X = 10, Y = 22 }, Commands = [new Command { Direction = Direction.East, Steps = 2 }, new Command { Direction = Direction.North, Steps = 1 }] };

        var fakeTime = new FakeTimeProvider(startDateTime: DateTimeOffset.UtcNow);
        var service = new CleaningService(fakeTime);

        var actual = await service.Calculate(input);

        var expected = new CleaningResult { ID = "1243", Timestamp = fakeTime.GetUtcNow().DateTime, Commands = 2, Result = 4, Duration = 0.00123 };

        Assert.Equal(expected, actual);
    }
}