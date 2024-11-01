namespace RobotService;

public record CleaningPath
{
    public required Coordinate Start { get; init; }
    public required Command[] Commands { get; init; }

    // Simple validation logic, could be improved e.g. with Fluent Validation
    public static string? IsValid(CleaningPath path)
    {
        if (path.Commands.Length > 10000)
        {
            return "Too many commands (max: 100 000).";
        }
        if (path.Start.X < -100_000 || path.Start.X > 100_000)
        {
            return "Start.X must be -100 000 <= X <= 100 000.";
        }
        if (path.Start.Y < -100_000 || path.Start.Y > 100_000)
        {
            return "Validation error: Start.Y must be -100 000 <= Y <= 100 000.";
        }
        if (path.Commands.Any(c => c.Steps <= 0 || c.Steps >= 100_000))
        {
            return "Validation error: Step size must be 1 < Steps < 100 000.";
        }
        return null;
    }
}

public record Coordinate
{
    public required int X { get; init; }
    public required int Y { get; init; }
}

public enum Direction
{
    North, East, South, West
}

public record Command
{
    public required Direction Direction { get; init; }
    public required int Steps { get; init; }
}
