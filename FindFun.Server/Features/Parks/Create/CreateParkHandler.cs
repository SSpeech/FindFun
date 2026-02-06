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
        var municipalityId = await dbContext.Municipalities
            .Where(m => m.OfficialNa6 == request.Locality)
            .Select(m => m.Gid)
            .FirstOrDefaultAsync(cancellationToken);

        if (municipalityId == 0)
        {

            return StatusCodes.Status400BadRequest.CreateError<CreateParkCommand, int>("Locality", "Municipality not found.");
        }
        var streetData = await dbContext.Streets
        .AsNoTracking()
        .Where(s => s.Name == request.Street && s.MunicipioGid == municipalityId)
        .Select(s => new
        {
            StreetId = s.Id,
            HasAddress = s.Addresses.Any(a => a.Line1 == request.FormattedAddress)
        })
        .FirstOrDefaultAsync(cancellationToken);

        if (streetData!.HasAddress)
        {
           return StatusCodes.Status409Conflict.CreateError<CreateParkCommand, int>("Address", "The address already Exisit");

        }

        var coordinates = request.ParseCoordinate();
        if (!coordinates.IsValid)
         return StatusCodes.Status400BadRequest.CreateError<CreateParkCommand, int>(nameof(coordinates), "Invalid coordinate format.");

        Point? point = new(coordinates!.Data!.Longitude, coordinates.Data.Latitude) { SRID = 4326 };

        var address = new Address(
            request.FormattedAddress!,
            request.PostalCode!,
            streetData!.StreetId,
            point
        );
        await dbContext.Addresses.AddAsync(address, cancellationToken);

        var park = new Park(
            request.Name,
            request.Description!,
            address
        );
        await dbContext.Parks.AddAsync(park, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        return Result<int>.Success(park.Id);
    }
}