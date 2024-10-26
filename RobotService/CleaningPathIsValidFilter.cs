namespace RobotService;

public class CleaningPathIsValidFilter : IEndpointFilter
{
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext efiContext, 
        EndpointFilterDelegate next)
    {
        var path = efiContext.GetArgument<CleaningPath>(0);

        var validationError = CleaningPath.IsValid(path!);

        if (validationError is not null)
        {
            return Results.Problem(validationError, statusCode: 400);
        }
        return await next(efiContext);
    }
}