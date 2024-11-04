namespace RobotService;

public record CleaningResult
{
    public int ID { get; init; }
    public DateTime Timestamp { get; set; }
    public required int Commands { get; init; }
    public required int Result { get; init; }
    public required double Duration { get; init; }
}