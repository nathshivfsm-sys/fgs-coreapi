using Asp.Versioning;
using Fgs.Bff.Application.Features.Lookups.Dtos;
using Fgs.Bff.Application.Features.Lookups.Queries.ListLookupKeys;
using Fgs.Contracts.Api;
using Fgs.Foundation.Api;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Fgs.Bff.API.Controllers;

/// <summary>
/// REST discovery for BFF batch lookup catalog. Execute batches via GraphQL
/// <c>lookups(requests:)</c> at <c>/api/v1/bff/graphql</c>.
/// </summary>
[ApiController]
[ApiVersion(FgsApiVersions.V1)]
[FgsVersionedRoute("bff/lookups")]
[Produces("application/json")]
[Tags("Lookups")]
public sealed class LookupsController(IMediator mediator) : FgsApiControllerBase(mediator)
{
    /// <summary>
    /// Lists all lookup keys, owning services, paths, and filter metadata (Swagger discovery).
    /// Execute lookups with GraphQL <c>POST /api/v1/bff/graphql</c> — <c>lookups(requests:)</c>.
    /// </summary>
    [HttpGet("keys")]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<LookupKeyInfoDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> ListKeys(CancellationToken cancellationToken)
    {
        var response = await Mediator.Send(new ListLookupKeysQuery(), cancellationToken);
        return FromApiResponse(response);
    }
}
