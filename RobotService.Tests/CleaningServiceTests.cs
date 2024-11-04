namespace RobotService.Tests;

public class CleaningServiceTests
{
    [Fact]
    public void CalculateResult_ReturnsResult()
    {
        var service = new CleaningService();
        var inputPath = new CleaningPath { Start = new Coordinate { X = 10, Y = 22 }, Commands = [new Command { Direction = Direction.East, Steps = 2 }, new Command { Direction = Direction.North, Steps = 1 }] };

        CleaningResult actual = service.CalculateResult(inputPath);

        Assert.Equal(2, actual.Commands);
        Assert.Equal(4, actual.Result);
        Assert.IsType<double>(actual.Duration);
        // ID and Timestamp are not set yet
        Assert.Equal(0, actual.ID);
        Assert.Equal(DateTime.MinValue, actual.Timestamp);
    }
}