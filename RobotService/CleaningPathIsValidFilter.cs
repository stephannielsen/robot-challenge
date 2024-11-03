using Microsoft.AspNetCore.Http.HttpResults;

namespace RobotService;

public class CleaningPathIsValidFilter : IEndpointFilter
{
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext efiContext,
        EndpointFilterDelegate next)
    {
        var path = efiContext.GetArgument<CleaningPath>(0);

        if (!CleaningPath.IsValid(path!, out Dictionary<string, string[]> problems))
        {
            return Results.ValidationProblem(problems);
        }
        return await next(efiContext);
    }
}