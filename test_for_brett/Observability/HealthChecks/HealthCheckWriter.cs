using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace test_for_brett.Observability;

public class HealthCheckWriter
{
    protected HealthCheckWriter() { }

    public static async Task WriteResponse(HttpContext context, HealthReport report)
    {
        context.Response.ContentType = "application/json; charset=utf-8";

        var response = new JsonObject(new Dictionary<string, JsonNode?>()
        {
            ["status"] = report.Status.ToString(),
            ["details"] = new JsonArray(report.Entries.Select(entry =>
                new JsonObject(new Dictionary<string, JsonNode?>()
                {
                    ["item"] = entry.Key,
                    ["status"] = entry.Value.Status.ToString(),
                    ["description"] = entry.Value.Description,
                    ["duration"] = entry.Value.Duration.ToString("G"),
                    ["tags"] = new JsonArray(entry.Value.Tags.Select(tag => (JsonValue?)tag).ToArray()),
                    ["data"] = new JsonObject(entry.Value.Data.Select(data =>
                        new KeyValuePair<string, JsonNode?>(data.Key, (JsonValue)data.Value)
                    ))
                })).ToArray())
        });

        await context.Response.WriteAsJsonAsync(response);
    }
}