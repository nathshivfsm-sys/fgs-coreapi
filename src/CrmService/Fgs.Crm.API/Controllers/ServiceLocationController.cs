using Asp.Versioning;
using Fgs.Contracts.Api;
using Fgs.Crm.Application.Common.CrmCrud;
using Fgs.Crm.Application.Features.Customers.Commands.UpsertCrmDefaultServiceLocation;
using Fgs.Crm.Application.Features.Customers.Dtos;
using Fgs.Crm.Application.Features.Customers.Queries.GetCrmDefaultServiceLocation;
using Fgs.Crm.Application.Features.Customers.Queries.ListCrmCompanyServiceLocations;
using Fgs.Crm.Domain.Enums;
using Fgs.Foundation.Api;
using Fgs.Foundation.Paging;
using Fgs.Security.Authorization;
using Fgs.Security.Constants;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Fgs.Crm.API.Controllers;

/// <summary>
/// Company-wide CRM service locations.
/// </summary>
[ApiVersion(FgsApiVersions.V1)]
[FgsVersionedRoute("servicelocation")]
[ApiController]
[Produces("application/json")]
public sealed class ServiceLocationController(IMediator mediator) : ControllerBase
{
    [RequirePermission(FgsPermissionCodes.CustomerView)]
    [HttpGet("default")]
    [ProducesResponseType(typeof(ApiResponse<CrmDefaultServiceLocationDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetDefault(CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new GetCrmDefaultServiceLocationQuery(), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    [RequirePermission(FgsPermissionCodes.CustomerEdit)]
    [HttpPut("default")]
    [ProducesResponseType(typeof(ApiResponse<CrmDefaultServiceLocationDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpsertDefault(
        [FromBody] CrmDefaultServiceLocationUpsertDto request,
        CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new UpsertCrmDefaultServiceLocationCommand(request), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    /// <summary>
    /// Lists service locations for the current company.
    /// Omitted <paramref name="isActive"/> returns all locations.
    /// Summary counts ignore search, isActive, customer, and customer type.
    /// </summary>
    [RequirePermission(FgsPermissionCodes.CustomerView)]
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<CrmCompanyServiceLocationListResultDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> List(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? sortBy = null,
        [FromQuery] SortDirection sortDirection = SortDirection.Asc,
        [FromQuery] string? search = null,
        [FromQuery] bool? isActive = null,
        [FromQuery] IReadOnlyList<long>? customerId = null,
        [FromQuery] IReadOnlyList<CustomerType>? customerType = null,
        [FromQuery] bool includeSummary = true,
        CancellationToken cancellationToken = default)
    {
        var response = await mediator.Send(
            new ListCrmCompanyServiceLocationsQuery(
                new CrmListQuery(page, pageSize, sortBy, sortDirection, search, IsActive: isActive),
                new CrmCompanyServiceLocationListFilters(customerId, customerType),
                includeSummary),
            cancellationToken);

        return StatusCode(response.StatusCode, response);
    }
}
