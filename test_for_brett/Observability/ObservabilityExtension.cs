
using OpenTelemetry.Metrics;
using System.Diagnostics.Metrics;

namespace test_for_brett.Observability;

public static class ObservabilityExtension
{
    public static ILogger<T> GetBasicConsoleLogger<T>()
    {
        var serviceProvider = new ServiceCollection().AddLogging(config => config.AddConsole())
            .Configure<LoggerFilterOptions>(config => config.MinLevel = LogLevel.Trace)
            .BuildServiceProvider();

        return serviceProvider.GetService<ILogger<T>>() ?? throw new InvalidOperationException();
    }
    public static WebApplicationBuilder AddOpenTelemetryTracing(this WebApplicationBuilder builder, string serviceName, string serviceVersion, IConfiguration config)
    {
        builder.Services.AddOpenTelemetry()
                        .WithMetrics(metricsBuilder =>
                        {
                            var meter = new Meter(serviceName, serviceVersion);
                            builder.Services.AddSingleton<Meter>(meter);
                            //new custom meters can be added here and should be included in the AddMeter() call below:
                            //convention for meter names is to use dash (-) separators
                            //    meter.CreateCounter<int>("some-meter-name"); //create the counter for the metric
                            //    metricsBuilder.AddMeter("some-meter-name" [, other-meters...]); //add the meter to the output
                            //new meters should be added into the services DI container so they can be injected into services where needed.
                            // https://learn.microsoft.com/en-us/aspnet/core/log-mon/metrics/metrics?view=aspnetcore-8.0
                            metricsBuilder.AddMeter("Microsoft.AspNetCore.Hosting", "Microsoft.AspNetCore.Server.Kestrel");
                            metricsBuilder.AddMeter("current-requests", "failed-requests", "requests-per-second", "total-requests");
                        });
        return builder;
    }

}

