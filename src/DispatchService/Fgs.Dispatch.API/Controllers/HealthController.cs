using Asp.Versioning;
using Fgs.Foundation.Api;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fgs.Dispatch.API.Controllers;

[AllowAnonymous]
[ApiController]
[ApiVersion(FgsApiVersions.V1)]
[FgsVersionedRoute("[controller]")]
public sealed class HealthController : ControllerBase
{
    [HttpGet]
    public IActionResult Get() =>
        Ok(new { status = "healthy", service = "Fgs.Dispatch", apiVersion = FgsApiVersions.V1 });
}
