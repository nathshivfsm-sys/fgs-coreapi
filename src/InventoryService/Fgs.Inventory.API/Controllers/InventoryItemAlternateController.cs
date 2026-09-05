using Asp.Versioning;
using Fgs.Contracts.Api;
using Fgs.Foundation.Api;
using Fgs.Foundation.Idempotency;
using Fgs.Inventory.Application.Features.InventoryItemAlternates.Commands.CreateFgsInventoryItemAlternates;
using Fgs.Inventory.Application.Features.InventoryItemAlternates.Commands.DeleteFgsInventoryItemAlternate;
using Fgs.Inventory.Application.Features.InventoryItemAlternates.Commands.UpdateFgsInventoryItemAlternates;
using Fgs.Inventory.Application.Features.InventoryItemAlternates.Queries.GetFgsInventoryItemAlternateById;
using Fgs.Inventory.Application.Features.InventoryItemAlternates.Queries.ListFgsInventoryItemAlternates;
using Fgs.Inventory.Application.Features.InventoryItems.Dtos;
using MediatR;
using Fgs.Security.Authorization;
using Fgs.Security.Constants;
using Microsoft.AspNetCore.Mvc;

namespace Fgs.Inventory.API.Controllers;

/// <summary>
/// Inventory item alternate relationship management (full-replace on create/update).
/// </summary>
[ApiVersion(FgsApiVersions.V1)]
[FgsVersionedRoute("inventoryitemalternate")]
[ApiController]
[Produces("application/json")]
public sealed class InventoryItemAlternateController(IMediator mediator) : ControllerBase
{
    [HttpGet("{id:long}")]
    [ProducesResponseType(typeof(ApiResponse<FgsInventoryItemAlternateDetailDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(long id, CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new GetFgsInventoryItemAlternateByIdQuery(id), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<FgsInventoryItemAlternateDetailDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> List(
        [FromQuery] long inventoryItemId,
        CancellationToken cancellationToken = default)
    {
        var response = await mediator.Send(new ListFgsInventoryItemAlternatesQuery(inventoryItemId), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    [RequirePermission(FgsPermissionCodes.InventoryItemCreate)]
    [Idempotent]
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<FgsInventoryItemAlternateDetailDto>>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Create(
        [FromBody] FgsInventoryItemAlternateReplaceDto request,
        CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new CreateFgsInventoryItemAlternatesCommand(request), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    [RequirePermission(FgsPermissionCodes.InventoryItemEdit)]
    [HttpPut]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<FgsInventoryItemAlternateDetailDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Update(
        [FromBody] FgsInventoryItemAlternateReplaceDto request,
        CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new UpdateFgsInventoryItemAlternatesCommand(request), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    [RequirePermission(FgsPermissionCodes.InventoryItemDelete)]
    [HttpDelete("{id:long}")]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(long id, CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new DeleteFgsInventoryItemAlternateCommand(id), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }
}
