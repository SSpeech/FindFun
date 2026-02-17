using FindFun.Server;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection.Extensions;
using FindFun.Server.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Testcontainers.PostgreSql;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.Azurite;
using Azure.Storage.Blobs;
using NetTopologySuite.Geometries;
using FindFun.Server.Domain;
using Bogus;

namespace FindFund.Server.IntegrationTest;

public class WebAplicationCustomFactory : WebApplicationFactory<IServerMaker>, IAsyncLifetime
{
    public string MunicipalityName { get; private set; } = null!;
    private readonly Faker<Municipality> _faker;
    private IServiceScope? _scope;
    private FindFunDbContext? _dbContext;
    private readonly PostgreSqlContainer postgresContainer = new PostgreSqlBuilder("postgres:16")
              .WithImage("postgis/postgis:15-3.4")
              .Build();
    private readonly AzuriteContainer azuriteContainer = new AzuriteBuilder("mcr.microsoft.com/azure-storage/azurite").Build();
    public WebAplicationCustomFactory()
    {
        var geometryFactory = NetTopologySuite.NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326);

        _faker = new Faker<Municipality>()
               .CustomInstantiator(f => Municipality.Create(
                   f.Random.Int(1, 1000),
                   f.Address.ZipCode(),
                   f.Address.CountryCode(),
                   f.Address.StateAbbr(),
                   f.Address.City(),
                   f.Address.StreetAddress(),
                   f.Address.SecondaryAddress(),
                   f.Random.AlphaNumeric(10),
                   f.Address.CountryCode(),
                   "type",
                   "local",
                   geometryFactory.CreateMultiPolygon(
                   [
                       geometryFactory.CreatePolygon(
                           geometryFactory.CreateLinearRing(
                           [
                               new Coordinate(-1, -1),
                               new Coordinate(1, -1),
                               new Coordinate(1, 1),
                               new Coordinate(-1, 1),
                               new Coordinate(-1, -1),
                           ])
                       )
                   ])
               ));
    }
    public async Task AddMunicipality()
    {
        var municipality = _faker.Generate();
        _dbContext?.Municipalities.AddAsync(municipality);
        await _dbContext?.SaveChangesAsync()!;
        MunicipalityName = municipality.OfficialNa6;
    }

    public async Task InitializeAsync()
    {
        await postgresContainer.StartAsync();
        await azuriteContainer.StartAsync();
        _scope = Services.CreateScope();
        _dbContext = _scope.ServiceProvider.GetRequiredService<FindFunDbContext>();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureLogging(logging =>
        {
            logging.ClearProviders();
        });

        builder.ConfigureTestServices(services =>
        {
            services.RemoveAll(typeof(FindFunDbContext));
            services.AddDbContext<FindFunDbContext>(options =>
            {
                options.UseNpgsql(postgresContainer.GetConnectionString(), npgsqlOptions => npgsqlOptions.UseNetTopologySuite());
            });
            services.RemoveAll(typeof(BlobServiceClient));
            services.AddSingleton(sp => new BlobServiceClient(azuriteContainer.GetConnectionString()));
        });
    }

    async Task IAsyncLifetime.DisposeAsync()
    {
        _scope?.Dispose();
        await azuriteContainer.DisposeAsync();
        await postgresContainer.DisposeAsync();
    }
}
