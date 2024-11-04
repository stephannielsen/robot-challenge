using System;
using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RobotService.Tests.Helpers;

namespace RobotService.Tests;

[Collection("Sequential")]
public class RobotApiTests(TestWebApplicationFactory<Program> factory) : IClassFixture<TestWebApplicationFactory<Program>>
{
    private readonly HttpClient _httpClient = factory.CreateClient();

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
    public async Task PostCleaningPath_InvalidPaths(CleaningPath inputPath, Dictionary<string, string[]> expectedErrors)
    {
        var response = await _httpClient.PostAsJsonAsync("/tibber-developer-test/enter-path", inputPath);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        var actualError = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>();

        Assert.NotNull(actualError?.Errors);
        Assert.Equal(expectedErrors, actualError.Errors);
    }

    public static IEnumerable<object[]> ValidPaths =>
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
    [MemberData(nameof(ValidPaths))]
    public async Task PostCleaningPath_Valid(CleaningPath inputPath, int expectedResult)
    {
        var response = await _httpClient.PostAsJsonAsync("/tibber-developer-test/enter-path", inputPath);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        var actual = await response.Content.ReadFromJsonAsync<CleaningResult>();

        Assert.NotNull(actual);
        Assert.Equal(inputPath.Commands.Length, actual.Commands);
        Assert.Equal(expectedResult, actual.Result);
        Assert.IsType<int>(actual.ID);
        Assert.IsType<DateTime>(actual.Timestamp);
        Assert.IsType<double>(actual.Duration);
        Assert.True(actual.Duration > 0);
    }
}