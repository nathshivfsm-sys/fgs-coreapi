using Asp.Versioning;
using Fgs.Bff.Application.Features.Signup.Commands.CreateCompanySignup;
using Fgs.Contracts.Api;
using Fgs.Contracts.Signup;
using Fgs.Foundation.Api;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fgs.Bff.API.Controllers;

/// <summary>
/// Cross-domain workflows owned by the BFF (orchestration, aggregation, DTO mapping).
/// </summary>
[ApiController]
[ApiVersion(FgsApiVersions.V1)]
[FgsVersionedRoute("bff/signup")]
[Produces("application/json")]
public sealed class SignupController(IMediator mediator) : FgsApiControllerBase(mediator)
{
    /// <summary>
    /// Company self-serve signup: User identity + Setup business types (orchestrated on BFF).
    /// </summary>
    [AllowAnonymous]
    [HttpPost("company")]
    [ProducesResponseType(typeof(ApiResponse<CompanySignupResultDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> CompanySignup(
        [FromBody] CreateCompanySignupCommand command,
        CancellationToken cancellationToken)
    {
        var response = await Mediator.Send(command, cancellationToken);
        return FromApiResponse(response);
    }
}
