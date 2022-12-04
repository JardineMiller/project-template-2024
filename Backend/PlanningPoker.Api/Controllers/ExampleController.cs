using Microsoft.AspNetCore.Mvc;

namespace PlanningPoker.Api.Controllers;

public class ExampleController : ApiController
{
    [HttpGet]
    public IActionResult Index()
    {
        return Ok(Array.Empty<string>());
    }
}
