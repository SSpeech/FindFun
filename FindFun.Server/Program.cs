using FindFun.Server.Features.Parks;
using FindFun.Server.Features.Parks.Create;
using FindFun.Server.Infrastructure;

using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults()
    .AddDatabase();

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
