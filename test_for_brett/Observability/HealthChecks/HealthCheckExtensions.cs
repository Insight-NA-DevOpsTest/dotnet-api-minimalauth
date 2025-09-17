
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace test_for_brett.Observability;

public static class HealthcheckExtensions
{
    public static WebApplication MapHealthCheckEndpoint(this WebApplication app)
    {
        app.MapHealthChecks("health", new HealthCheckOptions { ResponseWriter = HealthCheckWriter.WriteResponse }).RequireAuthorization();
        return app;
    }

    public static WebApplicationBuilder AddDependencyChecks(this WebApplicationBuilder builder)
    {
        //Add Health checks for databases, queues, or other backing services...
        //  https://learn.microsoft.com/en-us/aspnet/core/host-and-deploy/health-checks?view=aspnetcore-8.0
        builder.Services.Configure<HttpConnectionCheck.HttpConnectionCheckOptions>(builder.Configuration.GetSection("HealthChecks"));
        builder.Services.AddHealthChecks()
                        .AddCheck<HttpConnectionCheck>("Network Connectivity", HealthStatus.Degraded);

        return builder;
    }


}
