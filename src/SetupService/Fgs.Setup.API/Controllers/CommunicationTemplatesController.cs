using Asp.Versioning;
using Fgs.Contracts.Api;
using Fgs.Contracts.Clients;
using Fgs.Foundation.Api;
using Fgs.Setup.Application.Features.CommunicationTemplates.Queries.GetActiveCommunicationTemplate;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Fgs.Setup.API.Controllers;
[ApiVersion(FgsApiVersions.V1)]
[FgsVersionedRoute("communication-templates")]
public sealed class CommunicationTemplatesController(IMediator mediator) : FgsApiControllerBase(mediator)
{
    [HttpGet("active")]
    [ProducesResponseType(typeof(ApiResponse<CommunicationTemplateDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetActive(
        [FromQuery] long? tenantId,
        [FromQuery] long? companyId,
        [FromQuery] string templateType,
        [FromQuery] string code,
        CancellationToken cancellationToken) =>
        FromApiResponse(await Mediator.Send(
            new GetActiveCommunicationTemplateQuery(
                tenantId,
                companyId,
                templateType,
                code),
            cancellationToken));
}
