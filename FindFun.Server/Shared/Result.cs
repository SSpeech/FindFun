using Microsoft.AspNetCore.Mvc;

namespace FindFun.Server.Shared;

public class Result<T>
{
    public bool IsValid { get; set; }
    public ValidationProblemDetails? ProblemDetails { get; set; }
    public T? Data { get; set; }

    public static Result<T> Success(T data) => new() { IsValid = true, Data = data };
    public static Result<T> Success() => new() { IsValid = true };
    public static Result<T> Failure(ValidationProblemDetails problemDetails) => new() { IsValid = false, ProblemDetails = problemDetails };
    public static Result<T> Failure(T data) => new() { IsValid = false, Data = data };
}
