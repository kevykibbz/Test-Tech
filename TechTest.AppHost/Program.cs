var builder = DistributedApplication.CreateBuilder(args);

// Add SQL Server
var sqlServer = builder.AddSqlServer("sqlserver")
    .WithDataVolume();

var database = sqlServer.AddDatabase("LawVuTechTestDB");

// Add Ollama container with Mistral model
var ollama = builder.AddContainer("ollama", "ollama/ollama")
    .WithHttpEndpoint(port: 11434, targetPort: 11434, name: "ollama-api")
    .WithVolume("ollama-data", "/root/.ollama");

// Add a container to pull the llama3.2:1b model after Ollama is ready
// Using curl to make API call to pull the model
var ollamaModelPull = builder.AddContainer("ollama-model-pull", "curlimages/curl")
    .WithArgs("sh", "-c", "sleep 15 && curl -X POST http://ollama:11434/api/pull -d '{\"name\":\"llama3.2:1b\"}' && echo 'Model pull completed'")
    .WaitFor(ollama);

// Add WebAPI project
builder.AddProject<Projects.WebApi>("lawvu-techtest-webapi", launchProfileName: "WebApi")
    .WithReference(database)
    .WithEnvironment("OLLAMA_ENDPOINT", ollama.GetEndpoint("ollama-api"))
    .WaitFor(ollamaModelPull)
    .WaitFor(database)
    .WithHttpEndpoint(port: 9091, targetPort: 9091, isProxied: false);

// Add Angular WebApp as Docker container with volume mounting for development
var webApp = builder.AddDockerfile("lawvu-techtest-webapp", "../WebApp", "Dockerfile.dev")
    .WithHttpEndpoint(port: 4200, targetPort: 4200)
    .WithBindMount("../WebApp", "/app")
    .WithVolume("webapp-node-modules", "/app/node_modules") // Keep node_modules in named volume for performance
    .WithExternalHttpEndpoints();

var app = builder.Build();

await app.RunAsync();