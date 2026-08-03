using Asp.Versioning;
using Fgs.Audit.Application.Features.CredentialAudits.Commands.RecordCredentialAudit;
using Fgs.Contracts.Api;
using Fgs.Contracts.Clients;
using Fgs.Contracts.CredentialAudit;
using Fgs.Credentials;
using Fgs.Credentials.Options;
using Fgs.Foundation.Api;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Fgs.Audit.API.Controllers;

[ApiVersion(FgsApiVersions.V1)]
[FgsVersionedRoute("credentialaudit")]
public sealed class CredentialAuditController(
    IMediator mediator,
    IOptions<CredentialDistributionOptions> distributionOptions) : FgsApiControllerBase(mediator)
{
    [AllowAnonymous]
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Record(
        [FromBody] RecordCredentialAuditRequest request,
        [FromHeader(Name = InternalServiceHeaders.ServiceKey)] string? serviceKey,
        CancellationToken cancellationToken)
    {
        if (!InternalServiceAuthorization.IsAuthorized(serviceKey, distributionOptions.Value))
        {
            return StatusCode(
                StatusCodes.Status401Unauthorized,
                ApiResponse<object>.Fail(
                    ["Internal service key is missing or invalid."],
                    ApiStatusCodes.Unauthorized));
        }

        return CreatedFromApiResponse(
            await Mediator.Send(new RecordCredentialAuditCommand(request), cancellationToken));
    }
}
