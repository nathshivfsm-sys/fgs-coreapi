using Asp.Versioning;
using Fgs.Foundation.Api;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fgs.WorkOrder.API.Controllers;

[AllowAnonymous]
[ApiController]
[ApiVersion(FgsApiVersions.V1)]
[FgsVersionedRoute("[controller]")]
public sealed class HealthController : ControllerBase
{
    [HttpGet]
    public IActionResult Get() =>
        Ok(new { status = "healthy", service = "Fgs.WorkOrder", apiVersion = FgsApiVersions.V1 });
}
