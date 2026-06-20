using Asp.Versioning;
using Fgs.Contracts.Api;
using Fgs.Contracts.Clients;
using Fgs.Foundation.Api;
using Fgs.User.Application.Features.Tenants.Commands.UpdateTenantCompanyDetails;
using Fgs.User.Application.Features.Tenants.Commands.UpdateTenantStatus;
using Fgs.User.Application.Features.Tenants.Commands.UpdateTenantStorageBucket;
using Fgs.User.Application.Features.Tenants.Dtos;
using Fgs.User.Application.Features.Tenants.Queries.GetTenant;
using Fgs.User.Application.Features.Tenants.Queries.GetTenantCompanyDetails;
using Fgs.User.Application.Features.Tenants.Queries.ListTenantCompanies;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Fgs.User.API.Controllers;
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

    [HttpGet("{tenantId:long}/companies/{companyId:long}/details")]
    [ProducesResponseType(typeof(ApiResponse<TenantCompanyDetailDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetDetails(
        long tenantId,
        long companyId,
        CancellationToken cancellationToken) =>
        FromApiResponse(await Mediator.Send(
            new GetTenantCompanyDetailsQuery(tenantId, companyId),
            cancellationToken));

    [HttpPost("{tenantId:long}/companies/{companyId:long}/details")]
    [ProducesResponseType(typeof(ApiResponse<TenantCompanyDetailDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateDetails(
        long tenantId,
        long companyId,
        [FromBody] UpdateTenantCompanyDetailsRequest request,
        CancellationToken cancellationToken) =>
        FromApiResponse(await Mediator.Send(
            new UpdateTenantCompanyDetailsCommand(tenantId, companyId, request),
            cancellationToken));

    [HttpPatch("{tenantId:long}/status")]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateStatus(
        long tenantId,
        [FromBody] UpdateTenantStatusRequest request,
        CancellationToken cancellationToken) =>
        FromApiResponse(await Mediator.Send(
            new UpdateTenantStatusCommand(tenantId, request),
            cancellationToken));

    [HttpPatch("{tenantId:long}/storage-bucket")]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateStorageBucket(
        long tenantId,
        [FromBody] UpdateTenantStorageBucketRequest request,
        CancellationToken cancellationToken) =>
        FromApiResponse(await Mediator.Send(
            new UpdateTenantStorageBucketCommand(tenantId, request),
            cancellationToken));
}
