using Asp.Versioning;
using Fgs.Audit.Application.Features.CredentialAudits.Commands.RecordCredentialAudit;
using Fgs.Contracts.Api;
using Fgs.Contracts.CredentialAudit;
using Fgs.Foundation.Api;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Fgs.Audit.API.Controllers;
[ApiVersion(FgsApiVersions.V1)]
[FgsVersionedRoute("credential-audits")]
public sealed class CredentialAuditsController(IMediator mediator) : FgsApiControllerBase(mediator)
{
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Record(
        [FromBody] RecordCredentialAuditRequest request,
        CancellationToken cancellationToken) =>
        CreatedFromApiResponse(await Mediator.Send(new RecordCredentialAuditCommand(request), cancellationToken));
}
