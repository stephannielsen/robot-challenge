namespace RobotService;

public record CleaningResult
{
    public int ID { get; init; }
    public required DateTime Timestamp { get; init; }
    public required int Commands { get; init; }
    public required int Result { get; init; }
    public required double Duration { get; init; }
}