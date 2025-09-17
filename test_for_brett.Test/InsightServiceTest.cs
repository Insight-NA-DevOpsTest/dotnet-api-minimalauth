using test_for_brett.Services;
using Moq;


namespace test_for_brett.Test;

[TestClass]
public class InsightServiceTest
{

    private Mock<ILogger<InsightService>>? _mockLogger;
    private InsightService? _service;

    [TestInitialize]
    public void InitializeTests() {
        _mockLogger = new Mock<ILogger<InsightService>>();
        _service = new InsightService(_mockLogger.Object);
    }

    [TestMethod]
    public void GivenNoInputs_WhenInsightServiceGetDescriptions_ThenReturns5Strings()
    {
        var result = _service?.GetDescriptions();
        Assert.AreEqual(5, result?.Count(), "GetDescriptions() result should have returned 5 values");
    }
}