using FindFun.Server.Domain;
using FindFun.Server.Validations;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace FindFun.Server.Features.Parks.Create;

public record CreateParkRequest(
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
    public Result<CoordinateDto> ParseCoordinate()
    {
        var parts = Coordinates.Split(',');
        if (parts.Length == 2 && double.TryParse(parts[0],CultureInfo.InvariantCulture, out var longitude) && double.TryParse(parts[1], CultureInfo.InvariantCulture, out var latitude))
        {
            return Result<CoordinateDto>.Success(new CoordinateDto(longitude, latitude));
        }

        return Result<CoordinateDto>.Failure(new ValidationProblemDetails
        {
         Errors = new Dictionary<string, string[]> {{ "Coordinates", ["Invalid coordinate format."] } }
        });
    }
    public List<ClosingScheduleEntry> ParseClosingScheduleParse(string? closingSchedule)
    {
        var result = new List<ClosingScheduleEntry>();
        if (!string.IsNullOrWhiteSpace(closingSchedule))
        {
            var schedules = closingSchedule
                .Split(';', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

            foreach (var schedule in schedules)
            {
                var dayAndTimes = schedule.Split(':', 2, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                var times = dayAndTimes[1].Split('-', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

                result.Add(new ClosingScheduleEntry(dayAndTimes[0],times[0], times.Length < 2 ? string.Empty : times[1], false));
            }
        }
        return result;
    }
    public Result<(string, string?)> ParseAmenityGroup(string amenities)
    {
        if (string.IsNullOrWhiteSpace(amenities))
        {
            return Result<(string, string?)>.Failure(new ValidationProblemDetails
            {
                Errors = new Dictionary<string, string[]> { { "Amenities", ["Amenities cannot be empty."] } }
            });
        }

        var parts = amenities.Split(':', 2, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        var note = parts.Length > 1 ? parts[1] : null;
        return Result<(string, string?)>.Success((parts[0], note));
    }
}


public record CoordinateDto([Required] double Longitude,[Required] double Latitude);
