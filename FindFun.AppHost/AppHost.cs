var builder = DistributedApplication.CreateBuilder(args);

var postgresPassword = builder.AddParameter("postgres-password", secret: true);

var postgres = builder.AddPostgres("postgres")
    .WithImage("postgis/postgis")
    .WithDataVolume()
    .WithPassword(postgresPassword)
    .WithPgAdmin(configureContainer =>
    {
        // need to check doc 
        configureContainer
            .WithEnvironment("PGADMIN_DEFAULT_EMAIL", "admin@findfun.local.com")
            .WithEnvironment("PGADMIN_DEFAULT_PASSWORD", postgresPassword);
    });

var db = postgres.AddDatabase("findfun");

var storage = builder.AddAzureStorage("storage")
    .RunAsEmulator(azurite =>
    {
        azurite.WithDataVolume();
    });

var blobs = storage.AddBlobs("blobs");

builder.AddProject<Projects.FindFun_Server>("findfun-server")
    .WithReference(db)
    .WithReference(blobs)
    .WaitFor(db);

builder.Build().Run();
