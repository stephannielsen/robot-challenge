using Microsoft.AspNetCore.Http.HttpResults;

namespace RobotService;

public static class RobotApiHandler
{
    public static async Task<Results<Created<CleaningResult>, BadRequest<ValidationProblem>>> CalculateCleaningPath(CleaningPath path, ICleaningService service, RobotDb db)
    {
        var result = service.CalculateResult(path);
        db.CleaningResults.Add(result);
        await db.SaveChangesAsync();
        // No API for accessing single results, so setting uri location to `null`
        return TypedResults.Created((string?)null, result);
    }
}