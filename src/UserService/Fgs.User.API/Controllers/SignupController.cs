using Fgs.User.Application.Common;
using Fgs.User.Application.Signup;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Fgs.User.API.Controllers;

/// <summary>
/// Company signup and tenant onboarding.
/// </summary>
[ApiController]
[Route("api/signup")]
[Produces("application/json")]
public sealed class SignupController(IMediator mediator) : ControllerBase
{
    /// <summary>
    /// Creates a tenant, default company, admin user, verification invitation, and outbox message in a single transaction.
    /// </summary>
    /// <remarks>
    /// Returns the standard JSON envelope (<c>success</c>, <c>statusCode</c>, <c>data</c>, <c>errors</c>) with invite URL; email delivery uses the outbox.
    /// </remarks>
    [HttpPost("company")]
    [ProducesResponseType(typeof(ApiResponse<CompanySignupResultDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> CompanySignup(
        [FromBody] CreateCompanySignupCommand command,
        CancellationToken cancellationToken)
    {
        var response = await mediator.Send(command, cancellationToken);
        return StatusCode(response.StatusCode, response);
    }
}
