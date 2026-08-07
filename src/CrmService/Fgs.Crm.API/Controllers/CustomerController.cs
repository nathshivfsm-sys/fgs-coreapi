using Asp.Versioning;
using Fgs.Contracts.Api;
using Fgs.Crm.Application.Common.CrmCrud;
using Fgs.Crm.Application.Features.Customers.Commands.CreateCrmCustomer;
using Fgs.Crm.Application.Features.Customers.Commands.PatchCrmCustomer;
using Fgs.Crm.Application.Features.Customers.Commands.UpdateCrmCustomer;
using Fgs.Crm.Application.Features.Customers.Dtos;
using Fgs.Crm.Application.Features.Customers.Queries.GetCrmCustomerById;
using Fgs.Crm.Application.Features.Customers.Queries.ListCrmCustomers;
using Fgs.Crm.Application.Features.Customers.Queries.LookupCrmCustomers;
using Fgs.Foundation.Api;
using Fgs.Foundation.Paging;
using Fgs.Security.Authorization;
using Fgs.Security.Constants;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Fgs.Crm.API.Controllers;

/// <summary>
/// Tenant-scoped CRM customer management.
/// </summary>
[ApiVersion(FgsApiVersions.V1)]
[FgsVersionedRoute("customer")]
[ApiController]
[Produces("application/json")]
public sealed class CustomerController(IMediator mediator) : ControllerBase
{
    [RequirePermission(FgsPermissionCodes.CustomerView)]
    [HttpGet("{id:long}")]
    [ProducesResponseType(typeof(ApiResponse<CrmCustomerDetailDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(long id, CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new GetCrmCustomerByIdQuery(id), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    [RequirePermission(FgsPermissionCodes.CustomerView)]
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<CrmCustomerSummaryDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> List(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 25,
        [FromQuery] string? sortBy = null,
        [FromQuery] SortDirection sortDirection = SortDirection.Asc,
        [FromQuery] string? search = null,
        [FromQuery] bool? isActive = true,
        [FromQuery] string? customerNumber = null,
        [FromQuery] string? name = null,
        [FromQuery] string? displayName = null,
        CancellationToken cancellationToken = default)
    {
        var response = await mediator.Send(
            new ListCrmCustomersQuery(
                new CrmListQuery(page, pageSize, sortBy, sortDirection, search, isActive),
                new CrmCustomerListFilters(customerNumber, name, displayName)),
            cancellationToken);

        return StatusCode(response.StatusCode, response);
    }

    [RequirePermission(FgsPermissionCodes.CustomerView)]
    [HttpGet("lookup")]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<CrmCustomerLookupDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Lookup(
        [FromQuery] bool activeOnly = true,
        CancellationToken cancellationToken = default)
    {
        var response = await mediator.Send(new LookupCrmCustomersQuery(activeOnly), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    [RequirePermission(FgsPermissionCodes.CustomerCreate)]
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<CrmCustomerDetailDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Create(
        [FromBody] CrmCustomerCreateDto request,
        CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new CreateCrmCustomerCommand(request), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    [RequirePermission(FgsPermissionCodes.CustomerEdit)]
    [HttpPut("{id:long}")]
    [ProducesResponseType(typeof(ApiResponse<CrmCustomerDetailDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Update(
        long id,
        [FromBody] CrmCustomerUpdateDto request,
        CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new UpdateCrmCustomerCommand(id, request), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    [RequirePermission(FgsPermissionCodes.CustomerEdit)]
    [HttpPatch("{id:long}")]
    [ProducesResponseType(typeof(ApiResponse<CrmCustomerDetailDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Patch(
        long id,
        [FromBody] CrmCustomerPatchDto request,
        CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new PatchCrmCustomerCommand(id, request), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }
}
