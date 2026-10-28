using Asp.Versioning;
using Fgs.Billing.Application.Common.BillingCrud;
using Fgs.Billing.Application.Features.CustomerPaymentInformation.Commands.CreateCustomerPaymentInformation;
using Fgs.Billing.Application.Features.CustomerPaymentInformation.Commands.DeleteCustomerPaymentInformation;
using Fgs.Billing.Application.Features.CustomerPaymentInformation.Commands.UpdateCustomerPaymentInformation;
using Fgs.Billing.Application.Features.CustomerPaymentInformation.Dtos;
using Fgs.Billing.Application.Features.CustomerPaymentInformation.Queries.GetCustomerPaymentInformationAcceptConfig;
using Fgs.Billing.Application.Features.CustomerPaymentInformation.Queries.GetCustomerPaymentInformationById;
using Fgs.Billing.Application.Features.CustomerPaymentInformation.Queries.ListCustomerPaymentInformation;
using Fgs.Contracts.Api;
using Fgs.Foundation.Api;
using Fgs.Foundation.Idempotency;
using Fgs.Foundation.Paging;
using Fgs.Security.Authorization;
using Fgs.Security.Constants;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Fgs.Billing.API.Controllers;

/// <summary>
/// Saved card payment methods for an AR customer. ACH rows are not returned by this controller.
/// </summary>
[ApiVersion(FgsApiVersions.V1)]
[FgsVersionedRoute("customerpaymentinformation")]
[ApiController]
[Produces("application/json")]
public sealed class CustomerPaymentInformationController(IMediator mediator) : ControllerBase
{
    [RequirePermission(FgsPermissionCodes.PaymentCreate)]
    [HttpGet("accept-config")]
    [ProducesResponseType(typeof(ApiResponse<CustomerPaymentInformationAcceptConfigDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAcceptConfig(CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new GetCustomerPaymentInformationAcceptConfigQuery(), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    [RequirePermission(FgsPermissionCodes.PaymentView)]
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<CustomerPaymentInformationListDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> List(
        [FromQuery] long arCustomerId,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 25,
        [FromQuery] string? sortBy = null,
        [FromQuery] SortDirection sortDirection = SortDirection.Asc,
        [FromQuery] string? search = null,
        [FromQuery] bool isActive = true,
        CancellationToken cancellationToken = default)
    {
        var response = await mediator.Send(
            new ListCustomerPaymentInformationQuery(
                arCustomerId,
                new BillingListQuery(page, pageSize, sortBy, sortDirection, search),
                isActive),
            cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    [RequirePermission(FgsPermissionCodes.PaymentView)]
    [HttpGet("{id:long}")]
    [ProducesResponseType(typeof(ApiResponse<CustomerPaymentInformationGridDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(long id, CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new GetCustomerPaymentInformationByIdQuery(id), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    [RequirePermission(FgsPermissionCodes.PaymentCreate)]
    [Idempotent]
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<CustomerPaymentInformationGridDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Create(
        [FromBody] CustomerPaymentInformationCreateDto request,
        CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new CreateCustomerPaymentInformationCommand(request), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    [RequirePermission(FgsPermissionCodes.PaymentEdit)]
    [HttpPut("{id:long}")]
    [ProducesResponseType(typeof(ApiResponse<CustomerPaymentInformationGridDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(
        long id,
        [FromBody] CustomerPaymentInformationUpdateDto request,
        CancellationToken cancellationToken)
    {
        var response = await mediator.Send(
            new UpdateCustomerPaymentInformationCommand(id, request),
            cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    [RequirePermission(FgsPermissionCodes.PaymentDelete)]
    [HttpDelete("{id:long}")]
    [ProducesResponseType(typeof(ApiResponse<CustomerPaymentInformationGridDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(long id, CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new DeleteCustomerPaymentInformationCommand(id), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }
}
