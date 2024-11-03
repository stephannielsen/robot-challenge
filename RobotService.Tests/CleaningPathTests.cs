namespace RobotService.Tests;

public class CleaningPathTests
{
    [Fact]
    public void IsValid_ReturnsNoError()
    {
        var inputPath = new CleaningPath { Start = new Coordinate { X = 10, Y = 22 }, Commands = [new Command { Direction = Direction.East, Steps = 2 }, new Command { Direction = Direction.North, Steps = 1 }] };
        var actualIsValid = CleaningPath.IsValid(inputPath, out Dictionary<string, string[]> actualErrors);

        Assert.True(actualIsValid);
        Assert.Empty(actualErrors);
    }

    public static IEnumerable<object[]> InvalidPaths => [
        [
            new CleaningPath { Start = new Coordinate { X = -100_001, Y = -100_001 }, Commands = Enumerable.Repeat(new Command { Direction = Direction.North, Steps = 1 }, 10001).ToArray() }, new Dictionary<string, string[]>
            {
                { nameof(CleaningPath.Commands), ["Too many commands (max: 10000)."]},
                { nameof(CleaningPath.Start.X), ["Start.X must be -100 000 <= X <= 100 000."] },
                { nameof(CleaningPath.Start.Y), ["Start.Y must be -100 000 <= Y <= 100 000."] }
            }
        ],
        [
            new CleaningPath { Start = new Coordinate { X = 100_001, Y = 100_001 }, Commands = [] }, new Dictionary<string, string[]>
            {
                { nameof(CleaningPath.Start.X), ["Start.X must be -100 000 <= X <= 100 000."] },
                { nameof(CleaningPath.Start.Y), ["Start.Y must be -100 000 <= Y <= 100 000."] }
            }
        ],
        [
            new CleaningPath { Start = new Coordinate { X = 0, Y = 0 }, Commands = [new Command { Direction = Direction.North, Steps = 0 }] }, new Dictionary<string, string[]>
            {
                { nameof(Command.Steps), ["Step size must be 1 < Steps < 100 000."] }
            }
        ],
        [
            new CleaningPath { Start = new Coordinate { X = 0, Y = 0 }, Commands = [new Command { Direction = Direction.North, Steps = 100_001 }] }, new Dictionary<string, string[]>
            {
                { nameof(Command.Steps), ["Step size must be 1 < Steps < 100 000."] }
            }
        ],
        // Two invalid steps and a valid step, only one error
        [
            new CleaningPath { Start = new Coordinate { X = 0, Y = 0 }, Commands = [new Command { Direction = Direction.North, Steps = 0 }, new Command { Direction = Direction.North, Steps = 100_001 }, new Command { Direction = Direction.North, Steps = 10 }] }, new Dictionary<string, string[]>
            {
                { nameof(Command.Steps), ["Step size must be 1 < Steps < 100 000."] }
            }
        ]
    ];

    [Theory]
    [MemberData(nameof(InvalidPaths))]
    public void IsValid_ReturnsError(CleaningPath inputPath, Dictionary<string, string[]> errors)
    {
        bool actualIsValid = CleaningPath.IsValid(inputPath, out Dictionary<string, string[]> actualErrors);

        Assert.False(actualIsValid);
        Assert.Equal(errors, actualErrors);
    }
}