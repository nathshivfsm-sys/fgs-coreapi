using Asp.Versioning;
using Fgs.Foundation.Api;
using Fgs.Foundation.Result;
using Fgs.User.Application.Features.Credentials.DTOs;
using Fgs.User.Application.Features.Credentials.Queries.GetCredentialSecretForTest;
using Fgs.User.Infrastructure.Common.Options;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Fgs.User.API.Controllers;

/// <summary>
/// Development-only endpoints that return decrypted credentials for integration testing.
/// Disabled unless ASPNETCORE_ENVIRONMENT=Development and AwsCredentials:EnableTestSecretEndpoint=true.
/// </summary>
[ApiController]
[ApiVersion(FgsApiVersions.V1)]
[FgsVersionedRoute("credentials/test")]
[Produces("application/json")]
[Tags("Credentials (Test - Development Only)")]
public sealed class CredentialSecretsTestController(
    IMediator mediator,
    IWebHostEnvironment environment,
    IOptions<AwsCredentialsOptions> awsCredentialsOptions) : ControllerBase
{
    /// <summary>
    /// Fetches decrypted secret JSON from AWS Secrets Manager (testing only).
    /// </summary>
    [HttpGet("{secretId:guid}/resolve")]
    [ProducesResponseType(typeof(ApiResponse<CredentialSecretTestDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ResolveSecret(
        Guid secretId,
        [FromQuery] long tenantId,
        [FromQuery] long companyId,
        [FromQuery] string? accessedBy,
        CancellationToken cancellationToken)
    {
        if (!IsTestEndpointEnabled())
        {
            return NotFound();
        }

        var response = await mediator.Send(
            new GetCredentialSecretForTestQuery
            {
                SecretId = secretId,
                TenantId = tenantId,
                CompanyId = companyId,
                AccessedBy = accessedBy
            },
            cancellationToken);

        return StatusCode(response.StatusCode, response);
    }

    private bool IsTestEndpointEnabled() =>
        environment.IsDevelopment()
        && awsCredentialsOptions.Value.EnableTestSecretEndpoint;
}
