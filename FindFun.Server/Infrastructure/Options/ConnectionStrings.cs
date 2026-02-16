namespace FindFun.Server.Infrastructure.Options;

public class ConnectionStrings
{
    public string FindFun { get; set; } = default!;
    public string? Blobs { get; set; }
}