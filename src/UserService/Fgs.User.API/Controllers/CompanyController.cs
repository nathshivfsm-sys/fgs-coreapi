using Asp.Versioning;
using Fgs.Contracts.Api;
using Fgs.Contracts.Clients;
using Fgs.Credentials;
using Fgs.Credentials.Options;
using Fgs.Foundation.Api;
using Fgs.MultiTenancy;
using Fgs.Security.Authorization;
using Fgs.Security.Constants;
using Fgs.User.Application.Features.Companies.Commands.CreateCompany;
using Fgs.User.Application.Features.Companies.Commands.PatchCompany;
using Fgs.User.Application.Features.Companies.Commands.UpdateCompany;
using Fgs.User.Application.Features.Companies.Dtos;
using Fgs.User.Application.Features.Companies.Queries.GetCompany;
using Fgs.User.Application.Features.Companies.Queries.GetCompanyAggregate;
using Fgs.User.Application.Features.Companies.Queries.ListCompanies;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Fgs.User.API.Controllers;

/// <summary>
/// Per-tenant company management.
/// </summary>
[ApiVersion(FgsApiVersions.V1)]
[FgsVersionedRoute("company")]
[Produces("application/json")]
public sealed class CompanyController(
    IMediator mediator,
    ITenantContextAccessor tenantContextAccessor,
    IOptions<CredentialDistributionOptions> distributionOptions) : FgsApiControllerBase(mediator)
{
    [AllowAnonymous]
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<TenantCompanyDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> List(
        [FromQuery] long? tenantId,
        [FromHeader(Name = InternalServiceHeaders.ServiceKey)] string? serviceKey,
        CancellationToken cancellationToken)
    {
        var unauthorized = UnauthorizedIfNotInternalOrAuthenticated(serviceKey);
        if (unauthorized is not null)
        {
            return unauthorized;
        }

        if (!TryResolveTenantId(tenantId, out var resolvedTenantId, out var error))
        {
            return BadRequest(ApiResponse<object>.Fail([error!], ApiStatusCodes.BadRequest));
        }

        return FromApiResponse(await Mediator.Send(new ListCompaniesQuery(resolvedTenantId), cancellationToken));
    }

    [HttpGet("{companyId:long}")]
    [ProducesResponseType(typeof(ApiResponse<CompanyDetailDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(
        long companyId,
        [FromQuery] long? tenantId,
        CancellationToken cancellationToken)
    {
        if (!TryResolveTenantId(tenantId, out var resolvedTenantId, out var error))
        {
            return BadRequest(ApiResponse<object>.Fail([error!], ApiStatusCodes.BadRequest));
        }

        return FromApiResponse(await Mediator.Send(
            new GetCompanyQuery(resolvedTenantId, companyId),
            cancellationToken));
    }

    [HttpGet("{companyId:long}/aggregate")]
    [ProducesResponseType(typeof(ApiResponse<CompanyAggregateDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAggregate(
        long companyId,
        [FromQuery] long? tenantId,
        CancellationToken cancellationToken)
    {
        if (!TryResolveTenantId(tenantId, out var resolvedTenantId, out var error))
        {
            return BadRequest(ApiResponse<object>.Fail([error!], ApiStatusCodes.BadRequest));
        }

        return FromApiResponse(await Mediator.Send(
            new GetCompanyAggregateQuery(resolvedTenantId, companyId),
            cancellationToken));
    }

    [RequirePermission(FgsPermissionCodes.UserCreate)]
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<CompanyDetailDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Create(
        [FromBody] CompanyCreateDto request,
        [FromQuery] long? tenantId,
        CancellationToken cancellationToken)
    {
        if (!TryResolveTenantId(tenantId, out var resolvedTenantId, out var error))
        {
            return BadRequest(ApiResponse<object>.Fail([error!], ApiStatusCodes.BadRequest));
        }

        return CreatedFromApiResponse(await Mediator.Send(
            new CreateCompanyCommand(resolvedTenantId, request),
            cancellationToken));
    }

    [RequirePermission(FgsPermissionCodes.UserEdit)]
    [HttpPut("{companyId:long}")]
    [ProducesResponseType(typeof(ApiResponse<CompanyDetailDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(
        long companyId,
        [FromBody] CompanyUpdateDto request,
        [FromQuery] long? tenantId,
        CancellationToken cancellationToken)
    {
        if (!TryResolveTenantId(tenantId, out var resolvedTenantId, out var error))
        {
            return BadRequest(ApiResponse<object>.Fail([error!], ApiStatusCodes.BadRequest));
        }

        return FromApiResponse(await Mediator.Send(
            new UpdateCompanyCommand(resolvedTenantId, companyId, request),
            cancellationToken));
    }

    [RequirePermission(FgsPermissionCodes.UserEdit)]
    [HttpPatch("{companyId:long}")]
    [ProducesResponseType(typeof(ApiResponse<CompanyDetailDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Patch(
        long companyId,
        [FromBody] CompanyPatchDto request,
        [FromQuery] long? tenantId,
        CancellationToken cancellationToken)
    {
        if (!TryResolveTenantId(tenantId, out var resolvedTenantId, out var error))
        {
            return BadRequest(ApiResponse<object>.Fail([error!], ApiStatusCodes.BadRequest));
        }

        return FromApiResponse(await Mediator.Send(
            new PatchCompanyCommand(resolvedTenantId, companyId, request),
            cancellationToken));
    }

    private bool TryResolveTenantId(long? tenantId, out long resolvedTenantId, out string? error)
    {
        if (tenantId is > 0)
        {
            resolvedTenantId = tenantId.Value;
            error = null;
            return true;
        }

        if (tenantContextAccessor.Current is ITenantContext context && context.TenantId > 0)
        {
            resolvedTenantId = context.TenantId;
            error = null;
            return true;
        }

        resolvedTenantId = 0;
        error = "tenantId is required when tenant context is not available.";
        return false;
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
