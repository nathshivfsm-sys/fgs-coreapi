using Asp.Versioning;
using Fgs.Contracts.Api;
using Fgs.Foundation.Api;
using Fgs.Foundation.Paging;
using Fgs.Scheduling.Application.Common.SchedulingCrud;
using Fgs.Scheduling.Application.Features.Appointments.Commands.CreateFgsAppointment;
using Fgs.Scheduling.Application.Features.Appointments.Dtos;
using Fgs.Scheduling.Application.Features.Appointments.Queries.GetFgsAppointmentById;
using Fgs.Scheduling.Application.Features.Appointments.Queries.ListFgsAppointments;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Fgs.Scheduling.API.Controllers;

/// <summary>
/// Tenant-scoped scheduling appointment management.
/// </summary>
[ApiVersion(FgsApiVersions.V1)]
[FgsVersionedRoute("appointment")]
[ApiController]
[Produces("application/json")]
public sealed class AppointmentController(IMediator mediator) : ControllerBase
{
    [HttpGet("{id:long}")]
    [ProducesResponseType(typeof(ApiResponse<FgsAppointmentDetailDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(long id, CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new GetFgsAppointmentByIdQuery(id), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<FgsAppointmentSummaryDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> List(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 25,
        [FromQuery] string? sortBy = null,
        [FromQuery] SortDirection sortDirection = SortDirection.Asc,
        [FromQuery] string? search = null,
        [FromQuery] DateOnly? serviceDate = null,
        [FromQuery] short? appointmentStatusId = null,
        [FromQuery] short? sourceTypeId = null,
        [FromQuery] long? sourceId = null,
        [FromQuery] long? crewId = null,
        CancellationToken cancellationToken = default)
    {
        var response = await mediator.Send(
            new ListFgsAppointmentsQuery(
                new SchedulingListQuery(page, pageSize, sortBy, sortDirection, search),
                new FgsAppointmentListFilters(serviceDate, appointmentStatusId, sourceTypeId, sourceId, crewId)),
            cancellationToken);

        return StatusCode(response.StatusCode, response);
    }

    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<FgsAppointmentDetailDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create(
        [FromBody] FgsAppointmentCreateDto request,
        CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new CreateFgsAppointmentCommand(request), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }
}
