using Asp.Versioning;
using Fgs.Foundation.Api;
using Fgs.Foundation.Result;
using Fgs.Setup.Application.Features.Credentials.Commands.CreateCredential;
using Fgs.Setup.Application.Features.Credentials.Commands.DeleteCredential;
using Fgs.Setup.Application.Features.Credentials.Commands.RotateCredential;
using Fgs.Setup.Application.Features.Credentials.Commands.UpdateCredential;
using Fgs.Setup.Application.Features.Credentials.DTOs;
using Fgs.Setup.Application.Features.Credentials.Queries.GetCredential;
using Fgs.Setup.Application.Features.Credentials.Queries.ListCredentials;
using Fgs.Setup.Application.Common;
using Fgs.Setup.Application.Common.Options;
using Fgs.Setup.Application.Features.Credentials.Queries.ResolveCredentialSecret;
using Fgs.Setup.Domain.Enums;
using Fgs.Setup.Infrastructure.Credentials;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Fgs.Setup.API.Controllers;

/// <summary>
/// Global and tenant credential management with AWS KMS envelope encryption.
/// </summary>
//[Authorize]
[AllowAnonymous]
[ApiVersion(FgsApiVersions.V1)]
[FgsVersionedRoute("credentials")]
[Produces("application/json")]
public sealed class CredentialsController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<CredentialSummaryDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> List(
        [FromQuery] CredentialScope scope,
        [FromQuery] long? tenantId,
        [FromQuery] long? companyId,
        [FromQuery] bool activeOnly = true,
        CancellationToken cancellationToken = default)
    {
        var response = await mediator.Send(
            new ListCredentialsQuery(scope, tenantId, companyId, activeOnly),
            cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ApiResponse<CredentialDetailDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Get(
        string id,
        [FromQuery] CredentialScope scope,
        CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new GetCredentialQuery(scope, id), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<CredentialMutationResultDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create(
        [FromBody] CreateCredentialRequest request,
        CancellationToken cancellationToken)
    {
        var response = await mediator.Send(
            new CreateCredentialCommand(
                request.Scope,
                request.ProviderCode,
                request.CredentialName,
                request.Payload,
                request.Description,
                request.TenantId,
                request.CompanyId),
            cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(typeof(ApiResponse<CredentialMutationResultDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Update(
        string id,
        [FromQuery] CredentialScope scope,
        [FromBody] UpdateCredentialRequest request,
        CancellationToken cancellationToken)
    {
        var response = await mediator.Send(
            new UpdateCredentialCommand(
                scope,
                id,
                request.CredentialName,
                request.Description,
                request.Payload,
                request.IsActive),
            cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Delete(
        string id,
        [FromQuery] CredentialScope scope,
        CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new DeleteCredentialCommand(scope, id), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    [HttpPost("{id}/rotate")]
    [ProducesResponseType(typeof(ApiResponse<CredentialMutationResultDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Rotate(
        string id,
        [FromQuery] CredentialScope scope,
        [FromBody] RotateCredentialRequest? request,
        CancellationToken cancellationToken)
    {
        var response = await mediator.Send(
            new RotateCredentialCommand(
                scope,
                id,
                request?.RotationMode ?? CredentialRotationMode.Full),
            cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    /// <summary>
    /// Internal: full resolved configuration snapshot for peer services (Platform, etc.).
    /// Authenticated via <see cref="CredentialDistributionHeaders.InternalServiceKey"/>, not JWT.
    /// </summary>
    [HttpGet("resolved")]
    [ProducesResponseType(typeof(ApiResponse<ResolvedCredentialConfigurationDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    public IActionResult GetResolvedConfiguration(
        [FromServices] CredentialConfigurationHolder holder,
        [FromServices] IOptions<CredentialDistributionOptions> distributionOptions,
        [FromHeader(Name = CredentialDistributionHeaders.InternalServiceKey)] string? serviceKey)
    {
        if (!IsInternalServiceAuthorized(serviceKey, distributionOptions.Value))
        {
            return Unauthorized();
        }

        if (holder.Values.Count == 0)
        {
            return StatusCode(
                StatusCodes.Status503ServiceUnavailable,
                ApiResponse<ResolvedCredentialConfigurationDto>.Fail(
                    ["Resolved credential configuration is not loaded yet."],
                    StatusCodes.Status503ServiceUnavailable));
        }

        return Ok(ApiResponse<ResolvedCredentialConfigurationDto>.Ok(
            new ResolvedCredentialConfigurationDto(holder.Values)));
    }

    [HttpGet("{id}/resolve")]
    [ProducesResponseType(typeof(ApiResponse<CredentialSecretDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> ResolveSecret(
        string id,
        [FromQuery] CredentialScope scope,
        CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new ResolveCredentialSecretQuery(scope, id), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    private static bool IsInternalServiceAuthorized(
        string? providedKey,
        CredentialDistributionOptions options)
    {
        if (string.IsNullOrWhiteSpace(options.InternalServiceKey))
        {
            return false;
        }

        return string.Equals(providedKey, options.InternalServiceKey, StringComparison.Ordinal);
    }
}

public sealed record CreateCredentialRequest(
    CredentialScope Scope,
    string ProviderCode,
    string CredentialName,
    string Payload,
    string? Description = null,
    long? TenantId = null,
    long? CompanyId = null);

public sealed record UpdateCredentialRequest(
    string CredentialName,
    string? Description = null,
    string? Payload = null,
    bool? IsActive = null);

public sealed record RotateCredentialRequest(CredentialRotationMode RotationMode = CredentialRotationMode.Full);
