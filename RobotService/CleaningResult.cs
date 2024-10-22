public record CleaningResult
{
    public required string ID { get; set; }
    public required DateTime Timestamp { get; set; }
    public required int Commands { get; set; }
    public required int Result { get; set; }
    public required double Duration { get; set; }
}