

using System.Security.Cryptography;

namespace test_for_brett.Services;

public class InsightService : IInsightService
{
    private static readonly string[] _descriptions = new[]
    {
        "Insight", "Apps & Integration", "Reference Implementation", "Cool", "Thank you", "Data", "Modern Apps"
    };

    private readonly ILogger<InsightService> _logger;

    public InsightService(ILogger<InsightService> logger)
    {
        _logger = logger;
    }

    public IEnumerable<string> GetDescriptions()
    {
        _logger.LogInformation("There was a farmer who had a dog");
        List<string> returnValue;
        var buffer = RandomNumberGenerator.GetBytes(5);

        returnValue = buffer.Select(i =>
            {
                _logger.LogTrace("the value: {Item}", i);
                var index = i % _descriptions.Length;
                return _descriptions[index];
            }).ToList();

        _logger.LogInformation("and bingo was his name-o");
        return returnValue;
    }
}
