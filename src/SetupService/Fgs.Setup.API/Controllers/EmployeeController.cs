using Asp.Versioning;
using Fgs.Contracts.Api;
using Fgs.Foundation.Api;
using Fgs.Foundation.Paging;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.Employees.Commands.CreateFgsEmployee;
using Fgs.Setup.Application.Features.Employees.Commands.PatchFgsEmployee;
using Fgs.Setup.Application.Features.Employees.Commands.UpdateFgsEmployee;
using Fgs.Setup.Application.Features.Employees.Dtos;
using Fgs.Setup.Application.Features.Employees.Queries.GetFgsEmployeeById;
using Fgs.Setup.Application.Features.Employees.Queries.ListEmployees;
using Fgs.Setup.Application.Features.Employees.Queries.LookupEmployees;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Fgs.Setup.API.Controllers;

/// <summary>
/// Tenant-scoped employee management.
/// </summary>
[ApiVersion(FgsApiVersions.V1)]
[ApiController]
[FgsVersionedRoute("employee")]
[Produces("application/json")]
public sealed class EmployeeController(IMediator mediator) : ControllerBase
{
    [HttpGet("{id:long}")]
    [ProducesResponseType(typeof(ApiResponse<FgsEmployeeDetailDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(long id, CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new GetFgsEmployeeByIdQuery(id), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<FgsEmployeeSummaryDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> List(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 25,
        [FromQuery] string? sortBy = null,
        [FromQuery] SortDirection sortDirection = SortDirection.Asc,
        [FromQuery] string? search = null,
        [FromQuery] bool? isActive = null,
        [FromQuery] string? employeeNumber = null,
        [FromQuery] short? employeeTypeId = null,
        [FromQuery] short? statusId = null,
        CancellationToken cancellationToken = default)
    {
        var response = await mediator.Send(
            new ListEmployeesQuery(
                new SetupListQuery(page, pageSize, sortBy, sortDirection, search, isActive),
                new FgsEmployeeListFilters(employeeNumber, employeeTypeId, statusId)),
            cancellationToken);

        return StatusCode(response.StatusCode, response);
    }

    [HttpGet("lookup")]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<FgsEmployeeLookupDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Lookup(
        [FromQuery] bool activeOnly = true,
        CancellationToken cancellationToken = default)
    {
        var response = await mediator.Send(new LookupEmployeesQuery(activeOnly), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<FgsEmployeeDetailDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Create(
        [FromBody] FgsEmployeeCreateDto request,
        CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new CreateFgsEmployeeCommand(request), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    [HttpPut("{id:long}")]
    [ProducesResponseType(typeof(ApiResponse<FgsEmployeeDetailDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Update(
        long id,
        [FromBody] FgsEmployeeUpdateDto request,
        CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new UpdateFgsEmployeeCommand(id, request), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    [HttpPatch("{id:long}")]
    [ProducesResponseType(typeof(ApiResponse<FgsEmployeeDetailDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Patch(
        long id,
        [FromBody] FgsEmployeePatchDto request,
        CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new PatchFgsEmployeeCommand(id, request), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }
}
