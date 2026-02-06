using Microsoft.EntityFrameworkCore;

namespace FindFun.Server.Infrastructure;

public static class DataExtension
{
    public static async Task InitializeDbAsync(this WebApplication app)
    {
        await using var scope = app.Services.CreateAsyncScope();
        var context = scope.ServiceProvider.GetRequiredService<FindFunDbContext>();
        await context.Database.MigrateAsync();
    }

    public static void  AddDatabase(this WebApplicationBuilder builder)
    {
        var connectionString = builder.Configuration.GetConnectionString("findfun");
        if (string.IsNullOrWhiteSpace(connectionString))
            throw new InvalidOperationException("Connection string 'findfun' not found.");

        builder.Services.AddDbContext<FindFunDbContext>(options =>
            options.UseNpgsql(connectionString, npgsql => npgsql.UseNetTopologySuite()));
    }
}
