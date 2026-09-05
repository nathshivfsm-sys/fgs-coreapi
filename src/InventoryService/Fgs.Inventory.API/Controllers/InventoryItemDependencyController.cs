using Asp.Versioning;
using Fgs.Contracts.Api;
using Fgs.Foundation.Api;
using Fgs.Foundation.Idempotency;
using Fgs.Inventory.Application.Features.InventoryItemDependencies.Commands.CreateFgsInventoryItemDependencies;
using Fgs.Inventory.Application.Features.InventoryItemDependencies.Commands.DeleteFgsInventoryItemDependency;
using Fgs.Inventory.Application.Features.InventoryItemDependencies.Commands.UpdateFgsInventoryItemDependencies;
using Fgs.Inventory.Application.Features.InventoryItemDependencies.Queries.GetFgsInventoryItemDependencyById;
using Fgs.Inventory.Application.Features.InventoryItemDependencies.Queries.ListFgsInventoryItemDependencies;
using Fgs.Inventory.Application.Features.InventoryItems.Dtos;
using MediatR;
using Fgs.Security.Authorization;
using Fgs.Security.Constants;
using Microsoft.AspNetCore.Mvc;

namespace Fgs.Inventory.API.Controllers;

/// <summary>
/// Inventory item dependency relationship management (full-replace on create/update).
/// </summary>
[ApiVersion(FgsApiVersions.V1)]
[FgsVersionedRoute("inventoryitemdependency")]
[ApiController]
[Produces("application/json")]
public sealed class InventoryItemDependencyController(IMediator mediator) : ControllerBase
{
    [HttpGet("{id:long}")]
    [ProducesResponseType(typeof(ApiResponse<FgsInventoryItemDependencyDetailDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(long id, CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new GetFgsInventoryItemDependencyByIdQuery(id), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<FgsInventoryItemDependencyDetailDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> List(
        [FromQuery] long inventoryItemId,
        CancellationToken cancellationToken = default)
    {
        var response = await mediator.Send(new ListFgsInventoryItemDependenciesQuery(inventoryItemId), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    [RequirePermission(FgsPermissionCodes.InventoryItemCreate)]
    [Idempotent]
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<FgsInventoryItemDependencyDetailDto>>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Create(
        [FromBody] FgsInventoryItemDependencyReplaceDto request,
        CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new CreateFgsInventoryItemDependenciesCommand(request), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    [RequirePermission(FgsPermissionCodes.InventoryItemEdit)]
    [HttpPut]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<FgsInventoryItemDependencyDetailDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Update(
        [FromBody] FgsInventoryItemDependencyReplaceDto request,
        CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new UpdateFgsInventoryItemDependenciesCommand(request), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    [RequirePermission(FgsPermissionCodes.InventoryItemDelete)]
    [HttpDelete("{id:long}")]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(long id, CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new DeleteFgsInventoryItemDependencyCommand(id), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }
}
