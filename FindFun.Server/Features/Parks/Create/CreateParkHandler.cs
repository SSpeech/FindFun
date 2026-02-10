using FindFun.Server.Domain;
using FindFun.Server.Infrastructure;
using FindFun.Server.Validations;
using Microsoft.EntityFrameworkCore;

namespace FindFun.Server.Features.Parks.Create;

public class CreateParkHandler(FindFunDbContext dbContext)
{
    public async Task<Result<int>> HandleAsync(CreateParkRequest request, CancellationToken cancellationToken)
    {
        var coordinates = request.ParseCoordinate();
        if (!coordinates.IsValid)
            return Result<int>.Failure(coordinates.ProblemDetails!);

        var amenityGroup = request.ParseAmenityGroup(request.Amenities);
        if (!amenityGroup.IsValid)
            return Result<int>.Failure(amenityGroup.ProblemDetails!);

        var municipalityId = await dbContext.Municipalities.AsNoTracking()
            .Where(m => m.OfficialNa6 == request.Locality)
            .Select(m => m.Gid)
            .FirstOrDefaultAsync(cancellationToken);

        if (municipalityId == 0)
        {
            return StatusCodes.Status400BadRequest.CreateProblemResult<CreateParkRequest, int>("Locality", "Locality not found.");
        }

        var existingAddress = await dbContext.Addresses
            .Include(a => a.Street)
            .FirstOrDefaultAsync(a =>
                a.Line1 == request.FormattedAddress
                && a.Street!.MunicipioGid == municipalityId
                && a.Street.Name == request.Street, cancellationToken);

        if (existingAddress is not null)
        {
            var parkExists = await dbContext.Parks
                .AsNoTracking()
                .AnyAsync(p => p.AddressId == existingAddress.Id, cancellationToken);

            if (parkExists)
               return StatusCodes.Status409Conflict.CreateProblemResult<CreateParkRequest, int>("Address", "The address already exists.");    
        }

        Street? street = existingAddress?.Street;
        if (street is null)
        {
            street = await dbContext.Streets
                .FirstOrDefaultAsync(s => s.Name == request.Street && s.MunicipioGid == municipalityId, cancellationToken);

            if (street is null)
            {
                street = new Street(request.Street!, municipalityId);
                await dbContext.Streets.AddAsync(street, cancellationToken);
            }
        }

        var address = existingAddress ?? new Address(
            request.FormattedAddress!,
            request.PostalCode!,
            street!,
           coordinates!.Data!.Longitude,
           coordinates.Data.Latitude
        );

        if (existingAddress is null)
            await dbContext.Addresses.AddAsync(address, cancellationToken);

        var closingScheduleEntries = request.ParseClosingScheduleParse(request.ClosingSchedule);
        var park = new Park(
            request.Name,
            request.Description!,
            address
        );
        await dbContext.Parks.AddAsync(park, cancellationToken);

        if (closingScheduleEntries.Count > 0)
        {
            var closingSchedule = new ClosingSchedule(closingScheduleEntries);
            park.SetClosingSchedule(closingSchedule);
            await dbContext.AddAsync(closingSchedule, cancellationToken);
        }

        var amenity = new Amenity { Name = amenityGroup.Data.Item1, Description = amenityGroup.Data.Item2 ?? string.Empty };
        park.AddAmenity(amenity);


        await dbContext.SaveChangesAsync(cancellationToken);

        return Result<int>.Success(park.Id);
    }
}