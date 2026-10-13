using Asp.Versioning;
using Fgs.Contracts.Api;
using Fgs.Foundation.Api;
using Fgs.User.Application.Features.GridPreferences.Commands.DeleteFgsUserGridPreference;
using Fgs.User.Application.Features.GridPreferences.Commands.SaveFgsUserGridPreference;
using Fgs.User.Application.Features.GridPreferences.Dtos;
using Fgs.User.Application.Features.GridPreferences.Queries.GetFgsUserGridPreference;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Fgs.User.API.Controllers;

/// <summary>
/// Signed-in user's grid column preferences. The row is always the caller in the current tenant and company.
/// </summary>
[ApiVersion(FgsApiVersions.V1)]
[FgsVersionedRoute("gridpreference")]
[Produces("application/json")]
public sealed class GridPreferenceController(IMediator mediator) : FgsApiControllerBase(mediator)
{
    [HttpGet("{gridCode}")]
    [ProducesResponseType(typeof(ApiResponse<FgsUserGridPreferenceDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Get(string gridCode, CancellationToken cancellationToken) =>
        FromApiResponse(await Mediator.Send(new GetFgsUserGridPreferenceQuery(gridCode), cancellationToken));

    [HttpPut("{gridCode}")]
    [ProducesResponseType(typeof(ApiResponse<FgsUserGridPreferenceDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Save(
        string gridCode,
        [FromBody] FgsUserGridPreferenceWriteDto request,
        CancellationToken cancellationToken) =>
        FromApiResponse(await Mediator.Send(
            new SaveFgsUserGridPreferenceCommand(gridCode, request.Columns),
            cancellationToken));

    [HttpDelete("{gridCode}")]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Delete(string gridCode, CancellationToken cancellationToken) =>
        FromApiResponse(await Mediator.Send(new DeleteFgsUserGridPreferenceCommand(gridCode), cancellationToken));
}
