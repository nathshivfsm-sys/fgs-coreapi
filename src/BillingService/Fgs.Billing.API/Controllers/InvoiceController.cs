using Asp.Versioning;
using Fgs.Contracts.Api;
using Fgs.Billing.Application.Common.BillingCrud;
using Fgs.Billing.Application.Features.Invoices.Commands.CreateFgsInvoice;
using Fgs.Billing.Application.Features.Invoices.Commands.PatchFgsInvoice;
using Fgs.Billing.Application.Features.Invoices.Commands.UpdateFgsInvoice;
using Fgs.Billing.Application.Features.Invoices.Dtos;
using Fgs.Billing.Application.Features.Invoices.Queries.GetFgsInvoiceById;
using Fgs.Billing.Application.Features.Invoices.Queries.ListFgsInvoices;
using Fgs.Billing.Application.Features.Invoices.Queries.LookupFgsInvoices;
using Fgs.Foundation.Api;
using Fgs.Foundation.Paging;
using Fgs.Security.Authorization;
using Fgs.Security.Constants;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Fgs.Billing.API.Controllers;

/// <summary>
/// Tenant-scoped billing invoice management.
/// </summary>
[ApiVersion(FgsApiVersions.V1)]
[FgsVersionedRoute("invoice")]
[ApiController]
[Produces("application/json")]
public sealed class InvoiceController(IMediator mediator) : ControllerBase
{
    [RequirePermission(FgsPermissionCodes.InvoiceView)]
    [HttpGet("{id:long}")]
    [ProducesResponseType(typeof(ApiResponse<FgsInvoiceDetailDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(long id, CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new GetFgsInvoiceByIdQuery(id), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    [RequirePermission(FgsPermissionCodes.InvoiceView)]
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<FgsInvoiceSummaryDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> List(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 25,
        [FromQuery] string? sortBy = null,
        [FromQuery] SortDirection sortDirection = SortDirection.Asc,
        [FromQuery] string? search = null,
        [FromQuery] string? invoiceNumber = null,
        [FromQuery] long? customerId = null,
        [FromQuery] bool? isPosted = null,
        [FromQuery] bool? isApproved = null,
        CancellationToken cancellationToken = default)
    {
        var response = await mediator.Send(
            new ListFgsInvoicesQuery(
                new BillingListQuery(page, pageSize, sortBy, sortDirection, search),
                new FgsInvoiceListFilters(invoiceNumber, customerId, isPosted, isApproved)),
            cancellationToken);

        return StatusCode(response.StatusCode, response);
    }

    [RequirePermission(FgsPermissionCodes.InvoiceView)]
    [HttpGet("lookup")]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<FgsInvoiceLookupDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Lookup(CancellationToken cancellationToken = default)
    {
        var response = await mediator.Send(new LookupFgsInvoicesQuery(), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    [RequirePermission(FgsPermissionCodes.InvoiceCreate)]
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<FgsInvoiceDetailDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Create(
        [FromBody] FgsInvoiceCreateDto request,
        CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new CreateFgsInvoiceCommand(request), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    [RequirePermission(FgsPermissionCodes.InvoiceEdit)]
    [HttpPut("{id:long}")]
    [ProducesResponseType(typeof(ApiResponse<FgsInvoiceDetailDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Update(
        long id,
        [FromBody] FgsInvoiceUpdateDto request,
        CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new UpdateFgsInvoiceCommand(id, request), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    [RequirePermission(FgsPermissionCodes.InvoiceEdit)]
    [HttpPatch("{id:long}")]
    [ProducesResponseType(typeof(ApiResponse<FgsInvoiceDetailDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Patch(
        long id,
        [FromBody] FgsInvoicePatchDto request,
        CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new PatchFgsInvoiceCommand(id, request), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }
}
