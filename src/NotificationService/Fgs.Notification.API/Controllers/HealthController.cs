using Asp.Versioning;
using Fgs.Foundation.Api;
using Microsoft.AspNetCore.Mvc;

namespace Fgs.Notification.API.Controllers;
[ApiController]
[ApiVersion(FgsApiVersions.V1)]
[FgsVersionedRoute("[controller]")]
public sealed class HealthController : ControllerBase
{
    [HttpGet]
    public IActionResult Get() =>
        Ok(new { status = "healthy", service = "Fgs.Notification", apiVersion = FgsApiVersions.V1 });
}
