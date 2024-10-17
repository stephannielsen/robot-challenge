using System.Text.Json.Serialization;

public class CleaningPath
{
    public required Coordinate Start { get; set; }
    public required Command[] Commands { get; set; }
}

public record struct Coordinate
{
    public required int X { get; set; }
    public required int Y { get; set; }
}

public enum Direction
{
    North, East, South, West
}

public struct Command
{
    public required Direction Direction { get; set; }
    public required int Steps { get; set; }
}