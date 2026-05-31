using Asp.Versioning;
using Fgs.Foundation.Api;
using Fgs.Security.Authorization;
using Fgs.Foundation.Result;
using Fgs.User.Application.Features.Credentials.Commands.CreateCredential;
using Fgs.User.Application.Features.Credentials.Commands.RevokeCredential;
using Fgs.User.Application.Features.Credentials.Commands.RotateCredential;
using Fgs.User.Application.Features.Credentials.Commands.UpdateCredential;
using Fgs.User.Application.Features.Credentials.DTOs;
using Fgs.User.Application.Features.Credentials.Queries.GetCredentialMetadata;
using Fgs.User.Application.Features.Credentials.Queries.ListCredentialProviders;
using Fgs.User.Application.Features.Credentials.Queries.ListCredentialSecrets;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fgs.User.API.Controllers;

/// <summary>
/// Credential provider and secret metadata management. Secret values are write-only and never returned.
/// </summary>
[ApiController]
[ApiVersion(FgsApiVersions.V1)]
[FgsVersionedRoute("credentials")]
[Produces("application/json")]
[Authorize(Policy = FgsAuthorizationPolicies.RequireTenantAdmin)]
public sealed class CredentialsController(IMediator mediator) : ControllerBase
{
    /// <summary>Creates a credential provider (if needed), stores the secret in AWS Secrets Manager, and persists metadata.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<CredentialSecretMetadataDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create(
        [FromBody] CreateCredentialCommand command,
        CancellationToken cancellationToken)
    {
        var response = await mediator.Send(command, cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    [HttpPut("{secretId:guid}")]
    [ProducesResponseType(typeof(ApiResponse<CredentialSecretMetadataDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(
        Guid secretId,
        [FromBody] UpdateCredentialCommand command,
        CancellationToken cancellationToken)
    {
        var request = command with { SecretId = secretId };
        var response = await mediator.Send(request, cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    [HttpPost("{secretId:guid}/rotate")]
    [ProducesResponseType(typeof(ApiResponse<CredentialSecretMetadataDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Rotate(
        Guid secretId,
        [FromBody] RotateCredentialCommand command,
        CancellationToken cancellationToken)
    {
        var request = command with { SecretId = secretId };
        var response = await mediator.Send(request, cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    [HttpPost("{secretId:guid}/revoke")]
    [ProducesResponseType(typeof(ApiResponse<CredentialSecretMetadataDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Revoke(
        Guid secretId,
        [FromBody] RevokeCredentialCommand command,
        CancellationToken cancellationToken)
    {
        var request = command with { SecretId = secretId };
        var response = await mediator.Send(request, cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<CredentialSecretMetadataDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> ListSecrets(
        [FromQuery] long tenantId,
        [FromQuery] long companyId,
        [FromQuery] Guid? providerId,
        [FromQuery] bool activeOnly = true,
        CancellationToken cancellationToken = default)
    {
        var response = await mediator.Send(
            new ListCredentialSecretsQuery
            {
                TenantId = tenantId,
                CompanyId = companyId,
                ProviderId = providerId,
                ActiveOnly = activeOnly
            },
            cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    [HttpGet("{secretId:guid}")]
    [ProducesResponseType(typeof(ApiResponse<CredentialSecretMetadataDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetMetadata(
        Guid secretId,
        [FromQuery] long tenantId,
        [FromQuery] long companyId,
        CancellationToken cancellationToken)
    {
        var response = await mediator.Send(
            new GetCredentialMetadataQuery
            {
                SecretId = secretId,
                TenantId = tenantId,
                CompanyId = companyId
            },
            cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    [HttpGet("providers")]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<CredentialProviderMetadataDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> ListProviders(
        [FromQuery] long tenantId,
        [FromQuery] long companyId,
        CancellationToken cancellationToken)
    {
        var response = await mediator.Send(
            new ListCredentialProvidersQuery { TenantId = tenantId, CompanyId = companyId },
            cancellationToken);
        return StatusCode(response.StatusCode, response);
    }
}
