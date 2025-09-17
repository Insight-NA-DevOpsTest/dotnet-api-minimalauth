using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace test_for_brett.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class ErrController : ControllerBase
{
    private readonly ILogger _logger;

    public ErrController(ILogger<ErrController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public ActionResult Get()
    {
        _logger.LogInformation("This command will return an error.  This shows how error handling works using the ExceptionMiddleware class");
        throw new ArgumentException("this is a sample exception");
    }
}
