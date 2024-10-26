using Microsoft.Extensions.Time.Testing;

namespace RobotService.Tests;

public class CleaningPathTests
{
    [Fact]
    public void IsValidReturnsNoError()
    {
        var input = new CleaningPath { Start = new Coordinate { X = 10, Y = 22 }, Commands = [new Command { Direction = Direction.East, Steps = 2 }, new Command { Direction = Direction.North, Steps = 1 }] };
    
        var actual = CleaningPath.IsValid(input);
    
        Assert.Null(actual);
    }

    [Fact]
    public void IsValidReturnsError()
    {
        
        var input = new CleaningPath { Start = new Coordinate { X = -200_000, Y = 22 }, Commands = [new Command { Direction = Direction.East, Steps = 2 }, new Command { Direction = Direction.North, Steps = 1 }] };

        var actual = CleaningPath.IsValid(input);

        Assert.Equal("Start.X must be -100 000 <= X <= 100 000.", actual);
    }
}