namespace RobotService;

public class CleaningPathIsValidFilter : IEndpointFilter
{
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context,
        EndpointFilterDelegate next)
    {
        var path = context.GetArgument<CleaningPath>(0);

        if (!CleaningPath.IsValid(path!, out Dictionary<string, string[]> problems))
        {
            return Results.ValidationProblem(problems);
        }
        return await next(context);
    }
}