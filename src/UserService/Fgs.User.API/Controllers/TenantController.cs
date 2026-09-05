using Asp.Versioning;
using Fgs.Contracts.Api;
using Fgs.Contracts.Clients;
using Fgs.Credentials;
using Fgs.Credentials.Options;
using Fgs.Foundation.Api;
using Fgs.Foundation.Paging;
using Fgs.Security.Authorization;
using Fgs.Security.Constants;
using Fgs.User.Application.Common.IdentityCrud;
using Fgs.User.Application.Features.Tenants.Commands.PatchTenant;
using Fgs.User.Application.Features.Tenants.Commands.UpdateTenant;
using Fgs.User.Application.Features.Tenants.Commands.UpdateTenantStatus;
using Fgs.User.Application.Features.Tenants.Commands.UpdateTenantStorageBucket;
using Fgs.User.Application.Features.Tenants.Dtos;
using Fgs.User.Application.Features.Tenants.Queries.GetTenant;
using Fgs.User.Application.Features.Tenants.Queries.ListTenants;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Fgs.User.API.Controllers;

[ApiVersion(FgsApiVersions.V1)]
[FgsVersionedRoute("tenant")]
public sealed class TenantController(
    IMediator mediator,
    IOptions<CredentialDistributionOptions> distributionOptions) : FgsApiControllerBase(mediator)
{
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<TenantSummaryDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> List(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 25,
        [FromQuery] string? sortBy = null,
        [FromQuery] SortDirection sortDirection = SortDirection.Asc,
        [FromQuery] string? search = null,
        [FromQuery] bool? isActive = null,
        CancellationToken cancellationToken = default) =>
        FromApiResponse(await Mediator.Send(
            new ListTenantsQuery(new IdentityListQuery(page, pageSize, sortBy, sortDirection, search, isActive)),
            cancellationToken));

    [AllowAnonymous]
    [HttpGet("{tenantId:long}")]
    [ProducesResponseType(typeof(ApiResponse<TenantDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetTenant(
        long tenantId,
        [FromHeader(Name = InternalServiceHeaders.ServiceKey)] string? serviceKey,
        CancellationToken cancellationToken)
    {
        var unauthorized = UnauthorizedIfNotInternalOrAuthenticated(serviceKey);
        if (unauthorized is not null)
        {
            return unauthorized;
        }

        return FromApiResponse(await Mediator.Send(new GetTenantQuery(tenantId), cancellationToken));
    }

    [RequirePermission(FgsPermissionCodes.UserEdit)]
    [HttpPut("{tenantId:long}")]
    [ProducesResponseType(typeof(ApiResponse<TenantDetailDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(
        long tenantId,
        [FromBody] TenantUpdateDto request,
        CancellationToken cancellationToken) =>
        FromApiResponse(await Mediator.Send(new UpdateTenantCommand(tenantId, request), cancellationToken));

    [RequirePermission(FgsPermissionCodes.UserEdit)]
    [HttpPatch("{tenantId:long}")]
    [ProducesResponseType(typeof(ApiResponse<TenantDetailDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Patch(
        long tenantId,
        [FromBody] TenantPatchDto request,
        CancellationToken cancellationToken) =>
        FromApiResponse(await Mediator.Send(new PatchTenantCommand(tenantId, request), cancellationToken));

    [AllowAnonymous]
    [HttpPatch("{tenantId:long}/status")]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateStatus(
        long tenantId,
        [FromBody] UpdateTenantStatusRequest request,
        [FromHeader(Name = InternalServiceHeaders.ServiceKey)] string? serviceKey,
        CancellationToken cancellationToken)
    {
        var unauthorized = UnauthorizedIfNotInternal(serviceKey);
        if (unauthorized is not null)
        {
            return unauthorized;
        }

        return FromApiResponse(await Mediator.Send(
            new UpdateTenantStatusCommand(tenantId, request),
            cancellationToken));
    }

    [AllowAnonymous]
    [HttpPatch("{tenantId:long}/storage-bucket")]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateStorageBucket(
        long tenantId,
        [FromBody] UpdateTenantStorageBucketRequest request,
        [FromHeader(Name = InternalServiceHeaders.ServiceKey)] string? serviceKey,
        CancellationToken cancellationToken)
    {
        var unauthorized = UnauthorizedIfNotInternal(serviceKey);
        if (unauthorized is not null)
        {
            return unauthorized;
        }

        return FromApiResponse(await Mediator.Send(
            new UpdateTenantStorageBucketCommand(tenantId, request),
            cancellationToken));
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
}
