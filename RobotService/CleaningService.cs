using System.Diagnostics;
using static RobotService.CleaningServiceHelper;

namespace RobotService;

public class CleaningService : ICleaningService
{
    private readonly TimeProvider _timeProvider;

    public CleaningService(TimeProvider timeProvider)
    {
        _timeProvider = timeProvider;
    }

    public async Task<CleaningResult> Calculate(CleaningPath path, RobotDb db)
    {
        var stopwatch = Stopwatch.StartNew();
        var visitedPlaces = GetUniqueVisitedPlaces(path);
        stopwatch.Stop();
        var result = new CleaningResult { Timestamp = _timeProvider.GetUtcNow().DateTime, Commands = path.Commands.Length, Result = visitedPlaces, Duration = stopwatch.Elapsed.TotalSeconds };
        db.CleaningResults.Add(result);
        await db.SaveChangesAsync();
        return result;
    }
}