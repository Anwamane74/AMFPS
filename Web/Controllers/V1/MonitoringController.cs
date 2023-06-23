using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers.V1;

[ApiController]
[ApiVersion("1")]
[Route("api/v{version:apiVersion}/[controller]")]
public class MonitoringController : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<string>> IsAlive()
    {
        return await Task.FromResult(Ok("I am alive !"));
    }
}