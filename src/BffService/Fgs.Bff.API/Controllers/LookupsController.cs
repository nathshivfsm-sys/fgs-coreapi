using Asp.Versioning;
using Fgs.Bff.Application.Features.Lookups.Dtos;
using Fgs.Bff.Application.Features.Lookups.Queries.ListLookupKeys;
using Fgs.Contracts.Api;
using Fgs.Foundation.Api;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Fgs.Bff.API.Controllers;

/// <summary>
/// REST discovery for BFF batch lookup catalog (GraphQL is the runtime batch API).
/// </summary>
[ApiController]
[ApiVersion(FgsApiVersions.V1)]
[FgsVersionedRoute("bff/lookups")]
[Produces("application/json")]
public sealed class LookupsController(IMediator mediator) : FgsApiControllerBase(mediator)
{
    /// <summary>
    /// Lists all lookup keys, owning services, paths, and filter metadata.
    /// </summary>
    [HttpGet("keys")]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<LookupKeyInfoDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> ListKeys(CancellationToken cancellationToken)
    {
        var response = await Mediator.Send(new ListLookupKeysQuery(), cancellationToken);
        return FromApiResponse(response);
    }
}
