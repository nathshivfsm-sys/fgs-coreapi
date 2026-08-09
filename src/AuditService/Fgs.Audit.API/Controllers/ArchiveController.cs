using Asp.Versioning;
using Fgs.Audit.Application.Features.ArchiveCatalogs.Commands.UpsertArchiveCatalog;
using Fgs.Audit.Application.Features.ArchiveCatalogs.Dtos;
using Fgs.Audit.Application.Features.ArchiveCatalogs.Queries.GetArchiveCatalogById;
using Fgs.Audit.Application.Features.ArchiveCatalogs.Queries.ListArchiveCatalogs;
using Fgs.Contracts.Api;
using Fgs.Contracts.Audit;
using Fgs.Contracts.Clients;
using Fgs.Credentials;
using Fgs.Credentials.Options;
using Fgs.Foundation.Api;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Fgs.Audit.API.Controllers;

/// <summary>
/// Archive catalog inventory APIs. Upsert is S2S-only (internal service key).
/// Prefer not exposing these routes on the public NGINX gateway (deny like credentialaudit).
/// </summary>
[ApiVersion(FgsApiVersions.V1)]
[FgsVersionedRoute("archive")]
public sealed class ArchiveController(
    IMediator mediator,
    IOptions<CredentialDistributionOptions> distributionOptions) : FgsApiControllerBase(mediator)
{
    /// <summary>
    /// Creates or updates an archive catalog entry by archive month. S2S only.
    /// </summary>
    [AllowAnonymous]
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<ArchiveCatalogDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<ArchiveCatalogDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Upsert(
        [FromBody] UpsertArchiveCatalogRequest request,
        [FromHeader(Name = InternalServiceHeaders.ServiceKey)] string? serviceKey,
        CancellationToken cancellationToken)
    {
        var unauthorized = UnauthorizedIfNotInternal(serviceKey);
        if (unauthorized is not null)
        {
            return unauthorized;
        }

        var response = await Mediator.Send(new UpsertArchiveCatalogCommand(request), cancellationToken);
        return response.StatusCode == ApiStatusCodes.Created
            ? CreatedFromApiResponse(response)
            : FromApiResponse(response);
    }

    /// <summary>
    /// Gets an archive catalog entry by id. Accepts JWT or internal service key.
    /// </summary>
    [AllowAnonymous]
    [HttpGet("{id:long}")]
    [ProducesResponseType(typeof(ApiResponse<ArchiveCatalogDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(
        long id,
        [FromHeader(Name = InternalServiceHeaders.ServiceKey)] string? serviceKey,
        CancellationToken cancellationToken)
    {
        var unauthorized = UnauthorizedIfNotInternalOrAuthenticated(serviceKey);
        if (unauthorized is not null)
        {
            return unauthorized;
        }

        return FromApiResponse(
            await Mediator.Send(new GetArchiveCatalogByIdQuery(id), cancellationToken));
    }

    /// <summary>
    /// Lists archive catalog entries. Accepts JWT or internal service key.
    /// </summary>
    [AllowAnonymous]
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<ArchiveCatalogDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> List(
        [FromHeader(Name = InternalServiceHeaders.ServiceKey)] string? serviceKey,
        CancellationToken cancellationToken)
    {
        var unauthorized = UnauthorizedIfNotInternalOrAuthenticated(serviceKey);
        if (unauthorized is not null)
        {
            return unauthorized;
        }

        return FromApiResponse(
            await Mediator.Send(new ListArchiveCatalogsQuery(), cancellationToken));
    }

    private IActionResult? UnauthorizedIfNotInternal(string? serviceKey)
    {
        if (InternalServiceAuthorization.IsAuthorized(serviceKey, distributionOptions.Value))
        {
            return null;
        }

        return StatusCode(
            StatusCodes.Status401Unauthorized,
            ApiResponse<object>.Fail(
                ["Internal service key is missing or invalid."],
                ApiStatusCodes.Unauthorized));
    }

    private IActionResult? UnauthorizedIfNotInternalOrAuthenticated(string? serviceKey)
    {
        if (InternalServiceAuthorization.IsAuthorizedOrUserAuthenticated(
                serviceKey,
                distributionOptions.Value,
                User))
        {
            return null;
        }

        return StatusCode(
            StatusCodes.Status401Unauthorized,
            ApiResponse<object>.Fail(
                ["Authentication required. Provide a valid JWT or internal service key."],
                ApiStatusCodes.Unauthorized));
    }
}
