using Asp.Versioning;
using Fgs.Contracts.Api;
using Fgs.Contracts.Clients;
using Fgs.Contracts.Requests;
using Fgs.Foundation.Api;
using Fgs.Foundation.Idempotency;
using Fgs.Security.Authorization;
using Fgs.Security.Constants;
using Fgs.Setup.Application.Common;
using Fgs.Setup.Application.Features.BusinessTypes.Commands.AddCompanyBusinessTypes;
using Fgs.Setup.Application.Features.TenantProvisioning.Commands.ProvisionTenant;
using Fgs.Setup.Application.Features.TenantProvisioning.Commands.SyncTenantCatalog;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fgs.Setup.API.Controllers;

/// <summary>
/// Tenant onboarding, catalog resync, and company business-type bootstrap.
/// </summary>
[ApiVersion(FgsApiVersions.V1)]
public sealed class TenantController(IMediator mediator) : FgsApiControllerBase(mediator)
{
    /// <summary>
    /// Internal tenant provisioning orchestration invoked by the consumer service.
    /// Authenticated via <see cref="CredentialDistributionHeaders.InternalServiceKey"/>, not JWT.
    /// </summary>
    [AllowAnonymous]
    [FgsVersionedRoute("tenantprovisioning")]
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ProvisionTenant(
        [FromBody] ProvisionTenantRequest request,
        [FromHeader(Name = CredentialDistributionHeaders.InternalServiceKey)] string? serviceKey,
        [FromHeader(Name = CredentialDistributionHeaders.ServiceName)] string? serviceName,
        CancellationToken cancellationToken)
    {
        var response = await Mediator.Send(
            new ProvisionTenantCommand(request, serviceKey, serviceName),
            cancellationToken);
        return FromApiResponse(response);
    }

    /// <summary>
    /// Seeds missing catalog rows for the current tenant and company.
    /// Requires an authenticated user with setup edit permission.
    /// </summary>
    [RequirePermission(FgsPermissionCodes.SetupEdit)]
    [Idempotent]
    [FgsVersionedRoute("tenantseed")]
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<TenantCatalogSyncResultDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<TenantCatalogSyncResultDto>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<TenantCatalogSyncResultDto>), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Sync(CancellationToken cancellationToken) =>
        FromApiResponse(await Mediator.Send(new SyncTenantCatalogCommand(), cancellationToken));

    /// <summary>
    /// Seeds tenant-scoped business types from global catalog selections.
    /// Authenticated via <see cref="CredentialDistributionHeaders.InternalServiceKey"/>, not JWT.
    /// </summary>
    [AllowAnonymous]
    [FgsVersionedRoute("tenant/{tenantId:long}/companies/{companyId:long}/businesstype")]
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> AddCompanyBusinessTypes(
        long tenantId,
        long companyId,
        [FromBody] AddCompanyBusinessTypesRequest request,
        [FromHeader(Name = CredentialDistributionHeaders.InternalServiceKey)] string? serviceKey,
        [FromHeader(Name = CredentialDistributionHeaders.ServiceName)] string? serviceName,
        CancellationToken cancellationToken)
    {
        var response = await Mediator.Send(
            new AddCompanyBusinessTypesCommand(tenantId, companyId, request, serviceKey, serviceName),
            cancellationToken);

        return FromApiResponse(response);
    }
}
