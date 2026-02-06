using System.ComponentModel.DataAnnotations;
using NetTopologySuite.Geometries;

namespace FindFun.Server.Features.Parks.Create;

public record CreateParkCommand(
    [Required]
    [StringLength(100, MinimumLength = 2)]
    string Name,
    [StringLength(500)]
    string? Description,
    [Required]
    string LocationName,
    [StringLength(100)]
    string? Organizer,
    bool IsFree,
    string? AgeRecommendation,
    [Required]
    List<DateTime>? Dates,
    [Required]
    string Amenities,
    [Required]
    string ParkType,
    [Required]
    IFormFileCollection ParkImages,

    string? ClosingSchedule,
        [Required]
     string Coordinates,

    [StringLength(200)]
     string? FormattedAddress,

    [StringLength(100, MinimumLength = 2)]
     string? Street,

    [StringLength(20)]
     string? StreetNumber,

    [StringLength(100)]
     string? Locality,

    [StringLength(5, MinimumLength = 5)]
     string? PostalCode
) 
{
    public CoordinateDto ParseCoordinate()
    {
        var parts = Coordinates.Split(',');
        if (parts.Length == 2 && double.TryParse(parts[0], out var longitude) && double.TryParse(parts[1], out var latitude))
        {
            return new CoordinateDto(longitude, latitude);
        }

        throw new FormatException("Invalid coordinate format.");
    }
}

public record AmenityGroup(
    [MinLength(1)]
    List<string> Items,
    string? Note);
public record ClosingSchedule(bool VenueCloses, List<string> Schedules) : IValidatableObject
{
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (VenueCloses && (Schedules == null || Schedules.Count == 0))
        {
            yield return new ValidationResult(
                "If 'VenunCloses' is true, 'Schedules' must not be null or empty.",
                [nameof(Schedules)]
            );
        }
    }
}
public record LocationDetails
(
    [Required]
     CoordinateDto Coordinates,

    [StringLength(200)]
     string? FormattedAddress,

    [StringLength(100, MinimumLength = 2)]
     string? Street,

    [StringLength(20)]
     string? StreetNumber,

    [StringLength(100)]
     string? Locality,

    [StringLength(5, MinimumLength = 5)]
     string? PostalCode 
);

public record CoordinateDto(
    [Required]
    double Longitude,
    [Required]
    double Latitude
);
