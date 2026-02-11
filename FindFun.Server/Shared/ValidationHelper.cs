using FindFun.Server.Domain;
using FindFun.Server.Validations;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.ComponentModel.DataAnnotations;

namespace FindFun.Server.Shared;

public static class ValidationHelper
{
    public static Result<CoordinateDto> ParseCoordinate(string? coordinates)
    {
        var parts = coordinates?.Split(',');
        if (parts?.Length == 2
            && double.TryParse(parts[0], NumberStyles.Any, CultureInfo.InvariantCulture, out var longitude)
            && double.TryParse(parts[1], NumberStyles.Any, CultureInfo.InvariantCulture, out var latitude))
        {
            return Result<CoordinateDto>.Success(new CoordinateDto(longitude, latitude));
        }

        return Result<CoordinateDto>.Failure(new ValidationProblemDetails
        {
            Errors = new Dictionary<string, string[]> {{ "Coordinates",["Invalid coordinate format."] } }
        });
    }

    public static List<ClosingScheduleEntry> ParseClosingSchedule(string? closingSchedule)
    {
        var result = new List<ClosingScheduleEntry>();
        if (!string.IsNullOrWhiteSpace(closingSchedule))
        {
            var schedules = closingSchedule
                .Split(';', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

            foreach (var schedule in schedules)
            {
                var dayAndTimes = schedule.Split(':', 2, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                if (dayAndTimes.Length < 2)
                    continue;

                var times = dayAndTimes[1].Split('-', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

                result.Add(new ClosingScheduleEntry(dayAndTimes[0], times[0], times.Length < 2 ? string.Empty : times[1], false));
            }
        }
        return result;
    }

    public static Result<(string, string?)> ParseAmenityGroup(string amenities)
    {
        if (string.IsNullOrWhiteSpace(amenities))
        {
            return Result<(string, string?)>.Failure(new ValidationProblemDetails
            {
                Errors = new Dictionary<string, string[]> { { "Amenities", ["Amenities cannot be empty." ] } }
            });
        }

        var parts = amenities.Split(':', 2, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        var note = parts.Length > 1 ? parts[1] : null;
        return Result<(string, string?)>.Success((parts[0], note));
    }
    public static Result<decimal> ValidateEntrance(bool isFree, decimal entranceFee)
    {
        if (isFree)
            return Result<decimal>.Success(0m);

        if (entranceFee <= 0m)
        {
            return Result<decimal>.Failure(new ValidationProblemDetails
            {
                Errors = new Dictionary<string, string[]>
                {
                    { "EntraceFee", ["Entrance fee must be greater than 0." ]}
                }
            });
        }

        return Result<decimal>.Success(entranceFee);
    }
}

public record CoordinateDto([Required] double Longitude, [Required] double Latitude);
