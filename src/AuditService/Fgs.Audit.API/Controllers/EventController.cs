using Asp.Versioning;
using Fgs.Audit.Application.Features.Events.Commands.RecordAuditEvent;
using Fgs.Audit.Application.Features.Events.Dtos;
using Fgs.Audit.Application.Features.Events.Queries.GetAuditEventById;
using Fgs.Audit.Application.Features.Events.Queries.ListAuditEventsByEntity;
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
/// Product audit event APIs. Write/record is S2S-only (internal service key).
/// Prefer not exposing these routes on the public NGINX gateway (deny like credentialaudit).
/// </summary>
[ApiVersion(FgsApiVersions.V1)]
[FgsVersionedRoute("event")]
public sealed class EventController(
    IMediator mediator,
    IOptions<CredentialDistributionOptions> distributionOptions) : FgsApiControllerBase(mediator)
{
    /// <summary>
    /// Records an audit event. S2S only — not intended for public gateway exposure.
    /// </summary>
    [AllowAnonymous]
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<AuditEventDetailDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Record(
        [FromBody] RecordAuditEventRequest request,
        [FromHeader(Name = InternalServiceHeaders.ServiceKey)] string? serviceKey,
        CancellationToken cancellationToken)
    {
        var unauthorized = UnauthorizedIfNotInternal(serviceKey);
        if (unauthorized is not null)
        {
            return unauthorized;
        }

        return CreatedFromApiResponse(
            await Mediator.Send(new RecordAuditEventCommand(request), cancellationToken));
    }

    /// <summary>
    /// Gets an audit event by id. Accepts JWT or internal service key.
    /// Tenant/company filter applies when tenant context is present.
    /// </summary>
    [AllowAnonymous]
    [HttpGet("{id:long}")]
    [ProducesResponseType(typeof(ApiResponse<AuditEventDetailDto>), StatusCodes.Status200OK)]
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
            await Mediator.Send(new GetAuditEventByIdQuery(id), cancellationToken));
    }

    /// <summary>
    /// Lists audit events for a business entity. Accepts JWT or internal service key.
    /// Optional tenantId/companyId refine results for S2S calls without tenant context.
    /// </summary>
    [AllowAnonymous]
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<AuditEventSummaryDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> ListByEntity(
        [FromQuery] string recordType,
        [FromQuery] long entityId,
        [FromQuery] long? tenantId,
        [FromQuery] long? companyId,
        [FromHeader(Name = InternalServiceHeaders.ServiceKey)] string? serviceKey,
        CancellationToken cancellationToken)
    {
        var unauthorized = UnauthorizedIfNotInternalOrAuthenticated(serviceKey);
        if (unauthorized is not null)
        {
            return unauthorized;
        }

        return FromApiResponse(
            await Mediator.Send(
                new ListAuditEventsByEntityQuery(recordType, entityId, tenantId, companyId),
                cancellationToken));
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
