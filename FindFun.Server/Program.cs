using Azure.Storage.Blobs;
using FindFun.Server.Features.Parks;
using FindFun.Server.Features.Parks.Create;
using FindFun.Server.Infrastructure;
using FindFun.Server.Shared;

using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults()
    .AddDatabase();

builder.Services.AddHttpContextAccessor();

builder.Services.AddSingleton(sp =>
{
    var config = sp.GetRequiredService<IConfiguration>();
    var env = sp.GetRequiredService<IWebHostEnvironment>();

    // Prefer connection string injected by Aspire/AppHost (ConnectionStrings:blobs)
    // Fallback to BlobStorage:ConnectionString or to the Azurite development connection string when in Development.
    var connectionString = config.GetConnectionString("blobs")
        ?? config["BlobStorage:ConnectionString"]
        ?? (env.IsDevelopment() ? "UseDevelopmentStorage=true" : null);

    if (string.IsNullOrWhiteSpace(connectionString))
        throw new InvalidOperationException("Connection string 'blobs' not found. When running locally either run via AppHost or set 'BlobStorage:ConnectionString' or start Azurite.");

    return new BlobServiceClient(connectionString);
});

builder.Services.AddScoped<FileUpLoad>();

// Register request handlers
builder.Services.AddScoped<CreateParkHandler>();

builder.Services.AddOpenApi()
    .AddEndpointsApiExplorer()
    .AddSwaggerGen()

    .AddValidation()
    .AddProblemDetails();


var app = builder.Build();

app.MapDefaultEndpoints();

app.UseDefaultFiles();
app.MapStaticAssets();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapParks();
app.MapFallbackToFile("/index.html");
await app.InitializeDbAsync();

app.Run();
