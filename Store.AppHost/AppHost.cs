var builder = DistributedApplication.CreateBuilder(args);

var postgresPassword = builder
    .AddParameter("postgres-password", secret: true);

var postgres = builder
    .AddPostgres("database", password: postgresPassword)
    .WithPgWeb()
    .WithDataVolume("70c8de409db447077bf1ecd57fb4300d14492363f9a8b7fc761815be8e4337c2");

var database = postgres
    .AddDatabase("store", databaseName: "store");

builder
    .AddProject<Projects.Store_Api>("api")
    .WithReference(database)
    .WaitFor(database);

builder.Build().Run();