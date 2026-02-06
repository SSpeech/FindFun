using FindFun.Server.Features.Parks.Create;

namespace FindFun.Server.Features.Parks;

public static class ParksEndpoint
{ 
    public static void MapParks(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/parks");
        group.MapCreateParkEndpoint();
    }
}