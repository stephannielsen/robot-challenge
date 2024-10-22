using Microsoft.Extensions.Time.Testing;
using static CleaningServiceHelper;

namespace RobotService.Tests;

public class CleaningServiceHelperTests
{
    [Fact]
    public void GetUniqueVisitedPlacesReturnsUniquePlaces()
    {
        var input = new CleaningPath { Start = new Coordinate { X = 10, Y = 22 }, Commands = [new Command { Direction = Direction.East, Steps = 2 }, new Command { Direction = Direction.North, Steps = 1 }] };

        var actual = GetUniqueVisitedPlaces(input);

        Assert.Equal(4, actual);
    }
}