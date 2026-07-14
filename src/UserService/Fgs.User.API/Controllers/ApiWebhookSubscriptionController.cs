using Asp.Versioning;
using Fgs.Contracts.Api;
using Fgs.Foundation.Api;
using Fgs.Foundation.Paging;
using Fgs.User.Application.Common.IdentityCrud;
using Fgs.User.Application.Features.ApiWebhookSubscriptions.Commands.CreateFgsApiWebhookSubscription;
using Fgs.User.Application.Features.ApiWebhookSubscriptions.Commands.DeleteFgsApiWebhookSubscription;
using Fgs.User.Application.Features.ApiWebhookSubscriptions.Dtos;
using Fgs.User.Application.Features.ApiWebhookSubscriptions.Queries.GetFgsApiWebhookSubscriptionById;
using Fgs.User.Application.Features.ApiWebhookSubscriptions.Queries.ListFgsApiWebhookSubscriptions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Fgs.User.API.Controllers;

/// <summary>
/// Webhook-to-event subscription management.
/// </summary>
[ApiVersion(FgsApiVersions.V1)]
[FgsVersionedRoute("apiwebhooksubscription")]
[Produces("application/json")]
public sealed class ApiWebhookSubscriptionController(IMediator mediator) : FgsApiControllerBase(mediator)
{
    [HttpGet("{id:long}")]
    [ProducesResponseType(typeof(ApiResponse<FgsApiWebhookSubscriptionDetailDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(long id, CancellationToken cancellationToken) =>
        FromApiResponse(await Mediator.Send(new GetFgsApiWebhookSubscriptionByIdQuery(id), cancellationToken));

    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<FgsApiWebhookSubscriptionSummaryDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> List(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 25,
        [FromQuery] string? sortBy = null,
        [FromQuery] SortDirection sortDirection = SortDirection.Asc,
        [FromQuery] long? fgsApiWebhookId = null,
        [FromQuery] long? fgsApiEventId = null,
        CancellationToken cancellationToken = default) =>
        FromApiResponse(await Mediator.Send(
            new ListFgsApiWebhookSubscriptionsQuery(
                new IdentityListQuery(page, pageSize, sortBy, sortDirection),
                new FgsApiWebhookSubscriptionListFilters(fgsApiWebhookId, fgsApiEventId)),
            cancellationToken));

    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<FgsApiWebhookSubscriptionDetailDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Create(
        [FromBody] FgsApiWebhookSubscriptionCreateDto request,
        CancellationToken cancellationToken) =>
        CreatedFromApiResponse(
            await Mediator.Send(new CreateFgsApiWebhookSubscriptionCommand(request), cancellationToken));

    [HttpDelete("{id:long}")]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(long id, CancellationToken cancellationToken) =>
        FromApiResponse(await Mediator.Send(new DeleteFgsApiWebhookSubscriptionCommand(id), cancellationToken));
}
