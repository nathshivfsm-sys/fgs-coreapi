using Asp.Versioning;
using Fgs.Contracts.Api;
using Fgs.Contracts.Clients;
using Fgs.Foundation.Api;
using Fgs.Setup.Application.Common;
using Fgs.Setup.Application.Features.BusinessTypes.Commands.AddCompanyBusinessTypes;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fgs.Setup.API.Controllers;

/// <summary>
/// Internal tenant company business type bootstrap used during signup and provisioning.
/// </summary>
[AllowAnonymous]
[ApiVersion(FgsApiVersions.V1)]
[FgsVersionedRoute("tenant/{tenantId:long}/companies/{companyId:long}/businesstype")]
[Produces("application/json")]
public sealed class TenantCompanyBusinessTypeController(IMediator mediator) : ControllerBase
{
    /// <summary>
    /// Seeds tenant-scoped business types from global catalog selections.
    /// Authenticated via <see cref="CredentialDistributionHeaders.InternalServiceKey"/>, not JWT.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> AddCompanyBusinessTypes(
        long tenantId,
        long companyId,
        [FromBody] AddCompanyBusinessTypesRequest request,
        [FromHeader(Name = CredentialDistributionHeaders.InternalServiceKey)] string? serviceKey,
        [FromHeader(Name = CredentialDistributionHeaders.ServiceName)] string? serviceName,
        CancellationToken cancellationToken)
    {
        var response = await mediator.Send(
            new AddCompanyBusinessTypesCommand(tenantId, companyId, request, serviceKey, serviceName),
            cancellationToken);

        return StatusCode(response.StatusCode, response);
    }
}
