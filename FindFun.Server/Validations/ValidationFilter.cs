namespace FindFun.Server.Validations;

public class RequestValidationFilter<T> : IEndpointFilter
{
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var dto = context.Arguments.OfType<T>().FirstOrDefault();
        if (dto is null)
            return Results.BadRequest("Invalid request payload.");

        var validationResult = dto.ValidateWithProblemDetails(true);
        if (!validationResult.IsValid)
            return Results.Problem(validationResult.ProblemDetails!);

        return await next(context);
    }
}
