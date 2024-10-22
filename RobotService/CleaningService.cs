public class CleaningService : ICleaningService
{
    private readonly TimeProvider _timeProvider;

    public CleaningService(TimeProvider timeProvider)
    {
        _timeProvider = timeProvider;
    }

    public async Task<CleaningResult> Calculate(CleaningPath path)
    {
        return await Task.FromResult(new CleaningResult { ID = "1243", Timestamp = _timeProvider.GetUtcNow().DateTime, Commands = 2, Result = 4, Duration = 0.00123 });
    }
}