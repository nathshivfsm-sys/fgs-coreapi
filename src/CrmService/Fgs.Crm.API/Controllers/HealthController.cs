using Asp.Versioning;
using Fgs.Contracts.Api;
using Fgs.Contracts.Health;
using Fgs.Crm.Application.Features.Health.Queries.GetServiceHealth;
using Fgs.Foundation.Api;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Fgs.Crm.API.Controllers;
[ApiVersion(FgsApiVersions.V1)]
[FgsVersionedRoute("[controller]")]
public sealed class HealthController(IMediator mediator) : FgsApiControllerBase(mediator)
{
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<ServiceHealthDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Get(CancellationToken cancellationToken) =>
        FromApiResponse(await Mediator.Send(new GetServiceHealthQuery(), cancellationToken));
}
