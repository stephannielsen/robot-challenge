using System.Diagnostics;
using static RobotService.CleaningServiceHelper;

namespace RobotService;

public class CleaningService(TimeProvider timeProvider) : ICleaningService
{
    public async Task<CleaningResult> CalculateResult(CleaningPath path, RobotDb db)
    {
        var stopwatch = Stopwatch.StartNew();
        int visitedPlaces = GetUniqueVisitedPlaces(path);
        stopwatch.Stop();
        CleaningResult result = new() { Timestamp = timeProvider.GetUtcNow().DateTime, Commands = path.Commands.Length, Result = visitedPlaces, Duration = stopwatch.Elapsed.TotalSeconds };
        db.CleaningResults.Add(result);
        await db.SaveChangesAsync();
        return result;
    }
}