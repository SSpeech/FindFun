using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.WebUtilities;
using System.ComponentModel.DataAnnotations;

namespace FindFun.Server.Validations;

public static class ProblemDetailsResultExtensions
{
    public static Result<T> ValidateWithProblemDetails<T>(this T obj, bool includeAllErrors = default)
    {
        var context = new ValidationContext(obj!);
        var results = new List<ValidationResult>();
        bool isValid = Validator.TryValidateObject(obj!, context, results, true);

        if (isValid)
            return  Result<T>.Success();

        return BuildValidationProblemResult<T>(includeAllErrors, results);
    }

    private static  Result<T> BuildValidationProblemResult<T>(bool includeAllErrors, List<ValidationResult> results)
    {
        var modelState = new ModelStateDictionary();
        var errorsToProcess = includeAllErrors ? results : results.Take(1);

        errorsToProcess
            .SelectMany(result => result.MemberNames.Select(member => (result, member)))
            .ToList()
            .ForEach(item => modelState.AddModelError(item.member, item.result?.ErrorMessage!));

        var problemDetails = new ValidationProblemDetails(modelState)
        {
            Title = "One or more validation errors occurred.",
            Status = StatusCodes.Status400BadRequest,
            Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1"
        };

        return  Result<T>.Failure(problemDetails);
    }

    public static  Result<TResult> CreateProblemResult<T, TResult>(string fieldName, string errorMessage, int statusCode = StatusCodes.Status404NotFound)
    {
        var (title, type) = GetProblemDetailsTitleAndType(statusCode);
        var problemDetails = new ValidationProblemDetails
        {
            Title = title,
            Status = statusCode,
            Type = type
        };
        problemDetails.Errors.Add(fieldName, [errorMessage]);

        return  Result<TResult>.Failure(problemDetails);
    }

    private static (string Title, string Type) GetProblemDetailsTitleAndType(int statusCode)
    {
        return (ReasonPhrases.GetReasonPhrase(statusCode), GetProblemDetailsType(statusCode));
    }

    private static string GetProblemDetailsType(int statusCode)
    {
        return statusCode switch
        {
            StatusCodes.Status404NotFound => ProblemDetailsConstants.NotFound,
            StatusCodes.Status409Conflict => ProblemDetailsConstants.Conflict,
            StatusCodes.Status400BadRequest => ProblemDetailsConstants.BadRequest,
            StatusCodes.Status422UnprocessableEntity => ProblemDetailsConstants.UnprocessableEntity,
            StatusCodes.Status403Forbidden => ProblemDetailsConstants.Forbidden,
            StatusCodes.Status401Unauthorized => ProblemDetailsConstants.Unauthorized,
            _ => ProblemDetailsConstants.Default
        };
    }

    public static Result<TResult> CreateProblemResult<T, TResult>(this int statusCode, string fieldName, string errorMessage)
        => CreateProblemResult<T, TResult>(fieldName, errorMessage, statusCode);
}
