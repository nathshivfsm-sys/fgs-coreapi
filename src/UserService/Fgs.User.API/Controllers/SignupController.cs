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
    /// Creates a tenant, default company, admin user, physical location, verification invitation, and outbox message in a single transaction.
    /// </summary>
    /// <remarks>
    /// Request body maps to the onboarding questionnaire: <c>contact</c> (name, phone, email),
    /// <c>company</c> (name, website, structured <c>address</c>, companySize), and <c>businessTypeId</c> (industry from <c>GloBusinessType</c>).
    /// Tenant code is derived from the company name; timezone and currency are inferred from <c>company.address</c> (override with optional <c>timeZone</c> / <c>defaultCurrency</c>).
    /// Returns the standard JSON envelope with invite URL; email delivery uses the outbox.
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
