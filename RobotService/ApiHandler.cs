using Microsoft.AspNetCore.Http.HttpResults;

namespace RobotService;

public static class ApiHandler
{
    public static async Task<Results<Created<CleaningResult>, BadRequest<ValidationProblem>>> CalculateCleaningPath(CleaningPath path, ICleaningService service, RobotDb db)
    {
        var result = await service.CalculateResult(path, db);
        // No API for accessing single results, so setting uri location to `null`
        return TypedResults.Created((string?)null, result);
    }
}