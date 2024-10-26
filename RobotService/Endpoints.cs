using Microsoft.AspNetCore.Http.HttpResults;

namespace RobotService;

public static class Endpoints
{
    public static async Task<Results<Created<CleaningResult>, BadRequest<ValidationProblem>>> CleanPath(CleaningPath path, ICleaningService service, RobotDb db)
    {
        var result = await service.Calculate(path, db);
        return TypedResults.Created((string?)null, result);
    }
}