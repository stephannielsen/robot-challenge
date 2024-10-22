public record CleaningResult
{
    public int ID { get; init; }
    // Use local date time here to match requirements of using timestamps without time zone as column type
    // would prefer using DateTimeOffset.UtcNow.DateTime
    public DateTime Timestamp { get; init; } = DateTimeOffset.Now.DateTime;
    public required int Commands { get; init; }
    public required int Result { get; init; }
    public required double Duration { get; init; }
}