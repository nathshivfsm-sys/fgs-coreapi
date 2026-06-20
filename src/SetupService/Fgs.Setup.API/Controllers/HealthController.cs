using Asp.Versioning;
using Fgs.Foundation.Api;
using Microsoft.AspNetCore.Mvc;

namespace Fgs.Setup.API.Controllers;
[ApiController]
[ApiVersion(FgsApiVersions.V1)]
[FgsVersionedRoute("[controller]")]
public sealed class HealthController : ControllerBase
{
    [HttpGet]
    public IActionResult Get() =>
        Ok(new { status = "healthy", service = "Fgs.Setup", apiVersion = FgsApiVersions.V1 });
}
