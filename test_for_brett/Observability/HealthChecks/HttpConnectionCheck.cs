
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;

namespace test_for_brett.Observability;

public class HttpConnectionCheck : IHealthCheck
{
    public class HttpConnectionCheckOptions
    {
        public string? HealthCheckUrl { get; set; }
    }

    private readonly string? _healthCheckUrl;
    private readonly HttpClient _httpClient;
    private readonly ILogger _logger;

    public HttpConnectionCheck(HttpClient httpClient, IOptions<HttpConnectionCheckOptions> options, ILogger<HttpConnectionCheck> logger)
    {
        _healthCheckUrl = options.Value.HealthCheckUrl;
        _httpClient = httpClient;
        _logger = logger;
    }
    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        HealthCheckResult returnValue;

        try
        {
            _logger.LogDebug("Making test request to {Url}", _healthCheckUrl);
            await _httpClient.GetAsync(_healthCheckUrl, cancellationToken);
            _logger.LogDebug("Completed test request to {Url}", _healthCheckUrl);
            returnValue = HealthCheckResult.Healthy("HTTP connection successful");
        }
        catch (System.Net.Http.HttpRequestException httpEx)
        {
            _logger.LogWarning(httpEx, "Failed test request to {Url} with network error.", _healthCheckUrl);
            returnValue = HealthCheckResult.Unhealthy("Web request failed with network error.", httpEx);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed test request to {Url} with unknown error.", _healthCheckUrl);
            returnValue = HealthCheckResult.Unhealthy("Unknown error.", ex);
        }
        return returnValue;
    }
}