var builder = DistributedApplication.CreateBuilder(args);

builder.AddDockerComposeEnvironment("mi-red-hogarena");

var apiService = builder.AddProject<Projects.AgenteCoff_ApiService>("apiservice")
    .WithHttpHealthCheck("/health");

builder.AddProject<Projects.AgenteCoff_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithHttpHealthCheck("/health")
    .WithReference(apiService)
    .WaitFor(apiService);

builder.Build().Run();
