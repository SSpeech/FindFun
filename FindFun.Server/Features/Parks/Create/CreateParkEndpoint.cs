using Microsoft.AspNetCore.Mvc;

namespace FindFun.Server.Features.Parks.Create;

public static class CreateParkEndpoint
{
    public static void MapCreateParkEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapPost("", async (CreateParkHandler handler, [FromForm] CreateParkCommand request, CancellationToken cancellationToken) =>
        {
            var result = await handler.HandleAsync(request, cancellationToken);
            return result.IsValid ? Results.Ok(result.Data) : Results.Problem(result.ProblemDetails);
        })
        .WithName("CreatePark")
        .WithTags("Parks").DisableAntiforgery();
    }
}
