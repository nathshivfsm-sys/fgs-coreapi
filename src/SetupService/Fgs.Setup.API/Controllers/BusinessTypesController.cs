using Asp.Versioning;
using Fgs.Contracts.Api;
using Fgs.Contracts.Clients;
using Fgs.Foundation.Api;
using Fgs.Setup.Application.Features.BusinessTypes.Commands.AddCompanyBusinessTypes;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Fgs.Setup.API.Controllers;

[ApiVersion(FgsApiVersions.V1)]
[FgsVersionedRoute("tenants")]
public sealed class BusinessTypesController(IMediator mediator) : ControllerBase
{
    [HttpPost("{tenantId:long}/companies/{companyId:long}/business-types")]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddCompanyBusinessTypes(
        long tenantId,
        long companyId,
        [FromBody] AddCompanyBusinessTypesRequest request,
        CancellationToken cancellationToken)
    {
        var response = await mediator.Send(
            new AddCompanyBusinessTypesCommand(tenantId, companyId, request),
            cancellationToken);

        return StatusCode(response.StatusCode, response);
    }
}
