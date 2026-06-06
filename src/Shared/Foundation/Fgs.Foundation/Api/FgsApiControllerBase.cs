using Asp.Versioning;
using Fgs.Contracts.Api;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Fgs.Foundation.Api;

/// <summary>
/// Base API controller with shared attributes and <see cref="ApiResponse{T}"/> mapping helpers.
/// </summary>
[ApiController]
[Produces("application/json")]
public abstract class FgsApiControllerBase : ControllerBase
{
    protected FgsApiControllerBase(IMediator mediator) => Mediator = mediator;

    protected IMediator Mediator { get; }

    protected IActionResult FromApiResponse<T>(ApiResponse<T> response) =>
        StatusCode(response.StatusCode, response);

    protected IActionResult CreatedFromApiResponse<T>(ApiResponse<T> response) =>
        response.StatusCode == ApiStatusCodes.Created
            ? StatusCode(ApiStatusCodes.Created, response)
            : FromApiResponse(response);

    protected IActionResult NoContentFromApiResponse(ApiResponse<object> response) =>
        response.Success && response.StatusCode is ApiStatusCodes.NoContent or ApiStatusCodes.Ok
            ? NoContent()
            : FromApiResponse(response);
}
