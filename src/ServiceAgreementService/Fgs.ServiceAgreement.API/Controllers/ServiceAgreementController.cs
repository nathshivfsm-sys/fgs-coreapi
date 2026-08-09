using Asp.Versioning;
using Fgs.Contracts.Api;
using Fgs.Foundation.Api;
using Fgs.Foundation.Paging;
using Fgs.Security.Authorization;
using Fgs.Security.Constants;
using Fgs.ServiceAgreement.Application.Common.ServiceAgreementCrud;
using Fgs.ServiceAgreement.Application.Features.ServiceAgreements.Commands.CreateFgsServiceAgreement;
using Fgs.ServiceAgreement.Application.Features.ServiceAgreements.Dtos;
using Fgs.ServiceAgreement.Application.Features.ServiceAgreements.Queries.GetFgsServiceAgreementById;
using Fgs.ServiceAgreement.Application.Features.ServiceAgreements.Queries.ListFgsServiceAgreements;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Fgs.ServiceAgreement.API.Controllers;

/// <summary>
/// Tenant-scoped service agreement management.
/// </summary>
[ApiVersion(FgsApiVersions.V1)]
[FgsVersionedRoute("serviceagreement")]
[ApiController]
[Produces("application/json")]
public sealed class ServiceAgreementController(IMediator mediator) : ControllerBase
{
    [RequirePermission(FgsPermissionCodes.ServiceAgreementView)]
    [HttpGet("{id:long}")]
    [ProducesResponseType(typeof(ApiResponse<FgsServiceAgreementDetailDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(long id, CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new GetFgsServiceAgreementByIdQuery(id), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    [RequirePermission(FgsPermissionCodes.ServiceAgreementView)]
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<FgsServiceAgreementSummaryDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> List(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 25,
        [FromQuery] string? sortBy = null,
        [FromQuery] SortDirection sortDirection = SortDirection.Asc,
        [FromQuery] string? search = null,
        [FromQuery] string? agreementNumber = null,
        [FromQuery] long? customerId = null,
        [FromQuery] short? serviceAgreementStatusId = null,
        CancellationToken cancellationToken = default)
    {
        var response = await mediator.Send(
            new ListFgsServiceAgreementsQuery(
                new ServiceAgreementListQuery(page, pageSize, sortBy, sortDirection, search),
                new FgsServiceAgreementListFilters(agreementNumber, customerId, serviceAgreementStatusId)),
            cancellationToken);

        return StatusCode(response.StatusCode, response);
    }

    [RequirePermission(FgsPermissionCodes.ServiceAgreementCreate)]
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<FgsServiceAgreementDetailDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Create(
        [FromBody] FgsServiceAgreementCreateDto request,
        CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new CreateFgsServiceAgreementCommand(request), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }
}
