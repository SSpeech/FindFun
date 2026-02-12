var builder = DistributedApplication.CreateBuilder(args);

var postgresPassword = builder.AddParameter("postgres-password", secret: true);

var postgres = builder.AddPostgres("postgres")
    .WithImage("postgis/postgis")
    .WithDataVolume()
    .WithPassword(postgresPassword)
    .WithPgAdmin();

var db = postgres.AddDatabase("findfun");

var storage = builder.AddAzureStorage("storage")
    .RunAsEmulator(azurite =>
    {
        azurite.WithDataVolume();
    });

var blobs = storage.AddBlobs("blobs");

var server = builder.AddProject<Projects.FindFun_Server>("findfun-server")
    .WithReference(db)
    .WithReference(blobs)
    .WaitFor(db);

builder.AddViteApp("frontend", "../findfun.client")
    .WithHttpsEndpoint()
    .WithReference(server)
    .WaitFor(server);


builder.Build().Run();