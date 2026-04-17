var builder = DistributedApplication.CreateBuilder(args);

var api = builder.AddProject<Projects.TableTennisAPI>("api")
    .WithHttpHealthCheck("/health");

builder.AddProject<Projects.TableTennisFrontEnd>("web")
    .WithReference(api);

builder.Build().Run();
