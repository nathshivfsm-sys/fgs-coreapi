using Asp.Versioning;
using Fgs.Foundation.Api;
using Fgs.Contracts.Api;
using Fgs.User.Application.Features.Signup.Commands.CreateCompanySignup;
using Fgs.Contracts.Signup;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fgs.User.API.Controllers;

/// <summary>
/// Company signup and tenant onboarding.
/// </summary>
[ApiController]
[ApiVersion(FgsApiVersions.V1)]
[FgsVersionedRoute("signup")]
[Produces("application/json")]
public sealed class SignupController(IMediator mediator) : ControllerBase
{
    /// <summary>
    /// Creates a tenant, default company, admin user, physical location, verification invitation, and outbox message in a single transaction.
    /// </summary>
    /// <remarks>
    /// Prefer the BFF endpoint <c>POST /api/v1/bff/signup/company</c> for full onboarding (identity + business-type seeding).
    /// This User endpoint owns identity only; business types are seeded by the BFF via Setup.
    /// Request body maps to the onboarding questionnaire: <c>contact</c>, <c>company</c>, and <c>businessTypeIds</c>.
    /// Returns the standard JSON envelope with <c>tenantId</c>, <c>companyNumber</c>, <c>companyGuid</c>, <c>tenantCode</c>,
    /// user/invitation ids, and invite URL; email delivery uses the outbox.
    /// </remarks>
    [AllowAnonymous]
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

