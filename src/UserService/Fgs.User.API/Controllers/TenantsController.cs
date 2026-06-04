using Asp.Versioning;
using Fgs.Contracts.Api;
using Fgs.Contracts.Clients;
using Fgs.Foundation.Api;
using Fgs.User.Application.Features.Tenants.Commands.UpdateTenantStatus;
using Fgs.User.Application.Features.Tenants.Commands.UpdateTenantStorageBucket;
using Fgs.User.Application.Features.Tenants.Queries.GetTenant;
using Fgs.User.Application.Features.Tenants.Queries.ListTenantCompanies;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fgs.User.API.Controllers;

[AllowAnonymous]
[ApiVersion(FgsApiVersions.V1)]
[FgsVersionedRoute("tenants")]
public sealed class TenantsController(IMediator mediator) : FgsApiControllerBase(mediator)
{
    [HttpGet("{tenantId:long}")]
    [ProducesResponseType(typeof(ApiResponse<TenantDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetTenant(long tenantId, CancellationToken cancellationToken) =>
        FromApiResponse(await Mediator.Send(new GetTenantQuery(tenantId), cancellationToken));

    [HttpGet("{tenantId:long}/companies")]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<TenantCompanyDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCompanies(long tenantId, CancellationToken cancellationToken) =>
        FromApiResponse(await Mediator.Send(new ListTenantCompaniesQuery(tenantId), cancellationToken));

    [HttpPatch("{tenantId:long}/status")]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateStatus(
        long tenantId,
        [FromBody] UpdateTenantStatusRequest request,
        CancellationToken cancellationToken) =>
        NoContentFromApiResponse(await Mediator.Send(
            new UpdateTenantStatusCommand(tenantId, request),
            cancellationToken));

    [HttpPatch("{tenantId:long}/storage-bucket")]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateStorageBucket(
        long tenantId,
        [FromBody] UpdateTenantStorageBucketRequest request,
        CancellationToken cancellationToken) =>
        NoContentFromApiResponse(await Mediator.Send(
            new UpdateTenantStorageBucketCommand(tenantId, request),
            cancellationToken));
}
