using System.Text.Json.Serialization;

public record CleaningPath
{
    public required Coordinate Start { get; set; }
    public required Command[] Commands { get; set; }
}

public record Coordinate
{
    public required int X { get; set; }
    public required int Y { get; set; }
}

public enum Direction
{
    North, East, South, West
}

public record Command
{
    public required Direction Direction { get; set; }
    public required int Steps { get; set; }
}