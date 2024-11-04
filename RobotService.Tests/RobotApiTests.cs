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
    public async Task PostCleaningPath_InvalidPaths(CleaningPath path, Dictionary<string, string[]> expectedErrors)
    {
        var response = await _httpClient.PostAsJsonAsync("/tibber-developer-test/enter-path", path);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        var actualError = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>();

        Assert.NotNull(actualError?.Errors);
        Assert.Equal(expectedErrors, actualError.Errors);
    }

    [Fact]
    public async Task PostCleaningPath_Valid()
    {
        var inputPath = new CleaningPath { Start = new Coordinate { X = 10, Y = 22 }, Commands = [new Command { Direction = Direction.East, Steps = 2 }, new Command { Direction = Direction.North, Steps = 1 }] };
        var response = await _httpClient.PostAsJsonAsync("/tibber-developer-test/enter-path", inputPath);
        var actual = await response.Content.ReadFromJsonAsync<CleaningResult>();

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        Assert.NotNull(actual);

        Assert.Equal(2, actual.Commands);
        Assert.Equal(4, actual.Result);
        Assert.IsType<int>(actual.ID);
        Assert.IsType<DateTime>(actual.Timestamp);
        Assert.IsType<double>(actual.Duration);
    }
}