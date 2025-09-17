
using System.Reflection;
using test_for_brett.Services;
using test_for_brett.Authentication;
using test_for_brett.Observability;
using test_for_brett.ErrorHandling;
using test_for_brett.Configuration;
using test_for_brett.Swagger;
using Microsoft.OpenApi.Models;


var builder = WebApplication.CreateBuilder(args);

// create basic console logging to provide visibility into configuration setup
var logger = ObservabilityExtension.GetBasicConsoleLogger<Program>();
builder.UseAzureAppConfiguration(logger);
var assembly = Assembly.GetExecutingAssembly();
var serviceName = assembly.GetName().Name ?? throw new InvalidProgramException("Assembly must have a valid name.");
var serviceVersion = assembly.GetName().Version?.ToString() ?? "0.0.1";
builder.AddOpenTelemetryTracing(serviceName, serviceVersion, builder.Configuration);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerWithAuthentication(builder.Configuration.GetSection("Swagger"), logger);

builder.Services.AddHttpClient();
builder.AddDependencyChecks();
builder.AddAuthentication(builder.Configuration.GetSection("Authentication"))
       .AddApiKeyAuthentication(builder.Configuration.GetSection("Authentication"))
       .AddBearer(builder.Configuration.GetSection("Authentication"));

// Add business services
builder.Services.AddSingleton<IInsightService, InsightService>();
builder.Services.Configure<InsightServiceConfig>(builder.Configuration.GetSection(InsightServiceConfig.Config));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwaggerWithOptions(builder.Configuration);
}

app.UseMiddleware<ExceptionMiddleware>();
app.UseRouting();
//Enable dev-certs and local HTTPS debugging first: ``app.UseHttpsRedirection();``
app.UseAuthorization();
app.MapControllers();
app.MapHealthCheckEndpoint();

await app.RunAsync();



