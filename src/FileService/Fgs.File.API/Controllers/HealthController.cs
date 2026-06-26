using System.IdentityModel.Tokens.Jwt;
using Asp.Versioning;
using Fgs.Contracts.Api;
using Fgs.Contracts.Health;
using Fgs.Foundation.Api;
using Fgs.Foundation.Health;
using Fgs.Security.Options;
using Fgs.Security.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Fgs.File.API.Controllers;

[ApiVersion(FgsApiVersions.V1)]
[FgsVersionedRoute("[controller]")]
public sealed class HealthController(IMediator mediator, IConfiguration configuration)
    : FgsApiControllerBase(mediator)
{
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<ServiceHealthDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Get(CancellationToken cancellationToken) =>
        FromApiResponse(await Mediator.Send(new GetServiceHealthQuery(), cancellationToken));

    /// <summary>Development helper: validates the Bearer token and returns the validation error, if any.</summary>
    [HttpGet("auth-check")]
    public async Task<IActionResult> AuthCheck(CancellationToken cancellationToken)
    {
        var header = Request.Headers.Authorization.ToString();
        if (string.IsNullOrWhiteSpace(header)
            || !header.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
        {
            return Ok(new { valid = false, error = "Missing Bearer token in Authorization header." });
        }

        var token = header["Bearer ".Length..].Trim();
        token = FgsEntraGraphAccessTokenNormalizer.NormalizeIfRequired(token) ?? token;
        var entraOptions = configuration
                               .GetSection(EntraExternalIdAuthOptions.SectionName)
                               .Get<EntraExternalIdAuthOptions>()
                           ?? new EntraExternalIdAuthOptions();

        var signingKeys = await new FgsEntraSigningKeyResolver(entraOptions)
            .LoadSigningKeysAsync(cancellationToken);

        var validationParameters = FgsEntraTokenValidation.CreateValidationParameters(entraOptions);
        validationParameters.IssuerSigningKeys = signingKeys;

        try
        {
            new JwtSecurityTokenHandler().ValidateToken(token, validationParameters, out _);
            return Ok(new { valid = true });
        }
        catch (Exception ex)
        {
            return Ok(new { valid = false, error = ex.Message });
        }
    }
}
