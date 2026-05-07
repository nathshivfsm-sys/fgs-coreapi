namespace UserService.API.Controllers;

using MediatR;
using Microsoft.AspNetCore.Mvc;
using UserService.API.Models;
using UserService.Application.Signup.CreateCompanySignup;

[ApiController]
[Route("api/signup")]
public sealed class CompanySignupController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpPost("company")]
    public async Task<IActionResult> CreateCompanySignup(
        [FromBody] CreateCompanySignupRequest request,
        CancellationToken cancellationToken)
    {
        var command = new CreateCompanySignupCommand(
            request.CompanyName,
            request.Email,
            request.DisplayName);

        var result = await _mediator.Send(command, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }
}
