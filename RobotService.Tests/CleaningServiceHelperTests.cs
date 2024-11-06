using System.Text.Json;
using System.Text.Json.Serialization;
using static RobotService.CleaningServiceHelper;

namespace RobotService.Tests;

public class CleaningServiceHelperTests
{
    private JsonSerializerOptions _options;
    public CleaningServiceHelperTests()
    {
        _options = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
        _options.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
    }

    public static IEnumerable<object[]> CleaningPathData =>
    [
        [new CleaningPath { Start = new Coordinate { X = 10, Y = 22 }, Commands = [new Command { Direction = Direction.East, Steps = 2 }, new Command { Direction = Direction.North, Steps = 1 }] }, 4],
        [new CleaningPath { Start = new Coordinate { X = -43695, Y = -52014 }, Commands = [
            new Command { Direction = Direction.West, Steps = 55301 },
            new Command { Direction = Direction.South, Steps = 39162 },
            new Command { Direction = Direction.East, Steps = 89135 },
            new Command { Direction = Direction.North, Steps = 80438 }
        ] }, 264037 ],
        [new CleaningPath { Start = new Coordinate { X = 44743, Y = 74509 }, Commands = [
            new Command { Direction = Direction.East, Steps = 7104 },
            new Command { Direction = Direction.West, Steps = 95949 },
            new Command { Direction = Direction.West, Steps = 10470 },
            new Command { Direction = Direction.East, Steps = 21889 },
            new Command { Direction = Direction.West, Steps = 17842 },
            new Command { Direction = Direction.East, Steps = 15013 },
            new Command { Direction = Direction.East, Steps = 4711 }
        ] }, 106420 ],
    ];

    [Theory]
    [MemberData(nameof(CleaningPathData))]
    public void GetUniqueVisitedPlaces_ReturnsUniquePlaces(CleaningPath inputPath, int expectedVisited)
    {
        var actualVisited = GetUniqueVisitedPlaces(inputPath);

        Assert.Equal(expectedVisited, actualVisited);
    }

    [Fact]
    public async Task GetUniqueVisitedPlaces_HeavyPath_ReturnsUniquePlaces()
    {
        var file = Path.Combine(Directory.GetCurrentDirectory(), Path.Combine("paths", "robotcleanerpathheavy.json"));
        var json = await File.ReadAllTextAsync(file);

        CleaningPath? path = JsonSerializer.Deserialize<CleaningPath>(json, _options);

        Assert.NotNull(path);

        var actualVisited = GetUniqueVisitedPlaces(path);
        Assert.Equal(993737501, actualVisited);
    }
}