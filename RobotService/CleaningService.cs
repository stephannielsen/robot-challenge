using System.Diagnostics;
using static RobotService.CleaningServiceHelper;

namespace RobotService;

public class CleaningService() : ICleaningService
{
    public CleaningResult CalculateResult(CleaningPath path)
    {
        var stopwatch = Stopwatch.StartNew();
        int visitedPlaces = GetUniqueVisitedPlaces(path);
        stopwatch.Stop();
        CleaningResult result = new() { Commands = path.Commands.Length, Result = visitedPlaces, Duration = stopwatch.Elapsed.TotalSeconds };
        return result;
    }
}