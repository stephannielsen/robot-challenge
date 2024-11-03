namespace RobotService.Tests;

public class CleaningPathTests
{
    [Fact]
    public void IsValid_ReturnsNoError()
    {
        var input = new CleaningPath { Start = new Coordinate { X = 10, Y = 22 }, Commands = [new Command { Direction = Direction.East, Steps = 2 }, new Command { Direction = Direction.North, Steps = 1 }] };
        var actual = CleaningPath.IsValid(input, out _);

        Assert.True(actual);
    }

    public static IEnumerable<object[]> InvalidPaths => [
        [new CleaningPath { Start = new Coordinate { X = -100_001, Y = 0 }, Commands = [] }, "Start.X must be -100 000 <= X <= 100 000."],
        [new CleaningPath { Start = new Coordinate { X = 100_001, Y = 0 }, Commands = [] }, "Start.X must be -100 000 <= X <= 100 000."],
        [new CleaningPath { Start = new Coordinate { X = 0, Y = -100_001 }, Commands = [] }, "Start.Y must be -100 000 <= Y <= 100 000."],
        [new CleaningPath { Start = new Coordinate { X = 0, Y = 100_001 }, Commands = [] }, "Start.Y must be -100 000 <= Y <= 100 000."],
        [new CleaningPath { Start = new Coordinate { X = 0, Y = 0 }, Commands = [new Command { Direction = Direction.North, Steps = 0}] }, "Step size must be 1 < Steps < 100 000."],
        [new CleaningPath { Start = new Coordinate { X = 0, Y = 0 }, Commands = [new Command { Direction = Direction.North, Steps = 100_001}] }, "Step size must be 1 < Steps < 100 000."],
        [new CleaningPath { Start = new Coordinate { X = 0, Y = 0 }, Commands = Enumerable.Repeat(new Command { Direction = Direction.North, Steps = 1 }, 10001).ToArray() }, "Too many commands (max: 10000)."]
    ];

    [Theory]
    [MemberData(nameof(InvalidPaths))]
    public void IsValid_ReturnsError(CleaningPath path, string error)
    {
        var actual = CleaningPath.IsValid(path, out string actualError);

        Assert.False(actual);
        Assert.Equal(error, actualError);
    }
}