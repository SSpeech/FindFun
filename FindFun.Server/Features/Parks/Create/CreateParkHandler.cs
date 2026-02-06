using FindFun.Server.Domain;
using FindFun.Server.Infrastructure;
using FindFun.Server.Validations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;

namespace FindFun.Server.Features.Parks.Create;

public class CreateParkHandler(FindFunDbContext dbContext)
{
    public async Task<Result<int>> HandleAsync(CreateParkCommand request, CancellationToken cancellationToken)
    {
        var municipality = await dbContext.Municipalities
            .FirstOrDefaultAsync(m => m.OfficialNa == request.Locality, cancellationToken);

        if (municipality is null)
        {
            var problemDetails = new ValidationProblemDetails
            {
                Errors = new Dictionary<string, string[]>
                {
                    { "LocationDetails.Locality", ["Municipality not found."] }
                }
            };
            return Result<int>.Failure(problemDetails);
        }

        var street = await dbContext.Streets
            .FirstOrDefaultAsync(s => s.Name == request.Street && s.MunicipioGid == municipality.Gid, cancellationToken);

        if (street is null && request.Street is not null)
        {
            street = new Street(request.Street, municipality);
            dbContext.Streets.Add(street);
        }
        var coordinates = request.ParseCoordinate();
        Point? point = new(coordinates.Longitude, coordinates.Latitude) { SRID = 4326 };


        var address = new Address(
            request.FormattedAddress!,
            request.PostalCode!,
            street,
            point
        );
        await dbContext.Addresses.AddAsync(address,cancellationToken);

        var park = new Park(
            request.Name,
            request.Description!,
            address
        );
        await dbContext.Parks.AddAsync(park,cancellationToken);

        await dbContext.SaveChangesAsync(cancellationToken);

        return Result<int>.Success(park.Id);
    }
}