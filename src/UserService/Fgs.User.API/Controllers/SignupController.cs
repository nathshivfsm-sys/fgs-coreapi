using Asp.Versioning;
using Fgs.Foundation.Api;
using Fgs.Foundation.Result;
using Fgs.User.Application.Features.Signup.Commands.CreateCompanySignup;
using Fgs.User.Application.Features.Signup.DTOs;
using MediatR;
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
    /// Request body maps to the onboarding questionnaire: <c>contact</c> (name, phone, email),
    /// <c>company</c> (name, website, structured <c>address</c>, companySize), and <c>businessTypeIds</c> (one or more industries from <c>GloBusinessType</c>; the first is the primary).
    /// Tenant code is derived from the company name; timezone and currency are inferred from <c>company.address</c> (override with optional <c>timeZone</c> / <c>defaultCurrency</c>).
    /// Returns the standard JSON envelope with <c>tenantId</c>, <c>companyNumber</c>, <c>companyGuid</c>, user/invitation ids, and invite URL; email delivery uses the outbox.
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
