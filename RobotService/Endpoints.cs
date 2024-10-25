using Microsoft.AspNetCore.Http.HttpResults;

namespace RobotService;

public static class Endpoints
{
    public static async Task<Results<Created<CleaningResult>, BadRequest<ValidateError>>> CleanPath(CleaningPath path, ICleaningService service, RobotDb db)
    {

        var error = Validate(path);
        if (error != null)
        {
            return TypedResults.BadRequest(error);
        }

        var result = await service.Calculate(path, db);
        return TypedResults.Created((string?)null, result);
    }

    static ValidateError? Validate(CleaningPath path)
    {
        if (path.Commands.Length > 10000)
        {
            return new ValidateError { Message = "Validation error: Too many commands (max: 100 000)." };
        }
        if (path.Start.X < -100_000 || path.Start.X > 100_000)
        {
            return new ValidateError { Message = "Validation error: Start.X must be -100 000 <= X <= 100 000." };
        }
        if (path.Start.Y < -100_000 || path.Start.Y > 100_000)
        {
            return new ValidateError { Message = "Validation error: Start.Y must be -100 000 <= Y <= 100 000." };
        }
        if (path.Commands.Any(c => c.Steps <= 0 || c.Steps >= 100_000))
        {
            return new ValidateError { Message = "Validation error: Step size must be 1 < Steps < 100 000." };
        }
        return null;
    }
}

public record ValidateError
{
    public required string Message { get; set; }
}