using Microsoft.OpenApi.Models;
using WebApi.DependencyInjection;
using WebApi.Extensions;
#pragma warning disable CA1812 //https://github.com/dotnet/roslyn-analyzers/issues/5628

// Configure Services
var builder = WebApplication.CreateBuilder(args);

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp", policy =>
    {
        policy.WithOrigins("http://localhost:4200", "https://localhost:4200")
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials();
    });
});

// Register services
builder.Services.AddWebApiServices(builder.Configuration);
builder.Services.AddControllers();
builder.Services.AddSwaggerGen(c => c.SwaggerDoc("v1", new OpenApiInfo { Title = "WebApi", Version = "v1" }));

// Configure Middleware
var app = builder.Build();

// Always enable Swagger for development and testing
app.UseDeveloperExceptionPage();
app.UseSwagger();
app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebApi v1"));

// Enable CORS
app.UseCors("AllowAngularApp");

// Enable WebSocket support
app.UseWebSockets();

app.UseRouting();

// Add WebSocket middleware before mapping controllers
app.Use(async (context, next) =>
{
    if (context.Request.Path == "/ws")
    {
        if (context.WebSockets.IsWebSocketRequest)
        {
            using var webSocket = await context.WebSockets.AcceptWebSocketAsync().ConfigureAwait(false);
            await HandleWebSocketConnection(webSocket).ConfigureAwait(false);
        }
        else
        {
            context.Response.StatusCode = 400;
            await context.Response.WriteAsync("WebSocket connection required");
        }
    }
    else
    {
        await next(context);
    }
});

app.MapControllers();

// Database Setup
app.ApplyDatabaseMigrations(redeployDatabase: app.Environment.IsDevelopment());

// WebSocket handler method
static async Task HandleWebSocketConnection(System.Net.WebSockets.WebSocket webSocket)
{
    var buffer = new byte[1024 * 4];
    
    while (webSocket.State == System.Net.WebSockets.WebSocketState.Open)
    {
        try
        {
            var result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None).ConfigureAwait(false);
            
            if (result.MessageType == System.Net.WebSockets.WebSocketMessageType.Text)
            {
                var message = System.Text.Encoding.UTF8.GetString(buffer, 0, result.Count);
                
                // Send back a JSON response
                var responseJson = System.Text.Json.JsonSerializer.Serialize(new { 
                    type = "echo", 
                    message = message,
                    timestamp = DateTime.UtcNow
                });
                var responseBuffer = System.Text.Encoding.UTF8.GetBytes(responseJson);
                
                await webSocket.SendAsync(
                    new ArraySegment<byte>(responseBuffer),
                    System.Net.WebSockets.WebSocketMessageType.Text,
                    true,
                    CancellationToken.None).ConfigureAwait(false);
            }
            else if (result.MessageType == System.Net.WebSockets.WebSocketMessageType.Close)
            {
                await webSocket.CloseAsync(
                    System.Net.WebSockets.WebSocketCloseStatus.NormalClosure,
                    "Connection closed",
                    CancellationToken.None).ConfigureAwait(false);
            }
        }
        catch (System.Net.WebSockets.WebSocketException)
        {
            // Connection closed by client
            break;
        }
    }
}

// Run application
await app.RunAsync().ConfigureAwait(false);
