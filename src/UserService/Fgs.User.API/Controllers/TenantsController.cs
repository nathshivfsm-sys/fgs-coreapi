using Asp.Versioning;
using Fgs.Contracts.Clients;
using Fgs.Foundation.Api;
using Fgs.User.Infrastructure.Database;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Fgs.User.API.Controllers;

[AllowAnonymous]
[ApiController]
[ApiVersion(FgsApiVersions.V1)]
[FgsVersionedRoute("tenants")]
public sealed class TenantsController(FgsUserDbContext dbContext) : ControllerBase
{
    [HttpGet("{tenantId:long}")]
    public async Task<ActionResult<TenantDto>> GetTenant(long tenantId, CancellationToken cancellationToken)
    {
        var tenant = await dbContext.FgsTenants
            .AsNoTracking()
            .FirstOrDefaultAsync(t => t.Id == tenantId, cancellationToken);

        return tenant is null
            ? NotFound()
            : new TenantDto(tenant.Id, tenant.TenantCode, tenant.Name, tenant.FgsTenantStatusId, tenant.StorageBucketName);
    }

    [HttpGet("{tenantId:long}/companies")]
    public async Task<ActionResult<IReadOnlyList<TenantCompanyDto>>> GetCompanies(
        long tenantId,
        CancellationToken cancellationToken)
    {
        var companies = await dbContext.FgsTenantCompanies
            .AsNoTracking()
            .Where(c => c.TenantId == tenantId)
            .Select(c => new TenantCompanyDto(c.Id, c.TenantId, c.CompanyNumber, c.Code, c.Name))
            .ToListAsync(cancellationToken);

        return Ok(companies);
    }

    [HttpPatch("{tenantId:long}/status")]
    public async Task<IActionResult> UpdateStatus(
        long tenantId,
        [FromBody] UpdateTenantStatusRequest request,
        CancellationToken cancellationToken)
    {
        var tenant = await dbContext.FgsTenants.FirstOrDefaultAsync(t => t.Id == tenantId, cancellationToken);
        if (tenant is null)
        {
            return NotFound();
        }

        tenant.FgsTenantStatusId = request.FgsTenantStatusId;
        tenant.UpdatedOn = DateTimeOffset.UtcNow;
        if (request.FgsTenantStatusId == TenantStatusIds.Active)
        {
            tenant.IsActive = true;
        }

        await dbContext.SaveChangesAsync(cancellationToken);
        return NoContent();
    }

    [HttpPatch("{tenantId:long}/storage-bucket")]
    public async Task<IActionResult> UpdateStorageBucket(
        long tenantId,
        [FromBody] UpdateTenantStorageBucketRequest request,
        CancellationToken cancellationToken)
    {
        var tenant = await dbContext.FgsTenants.FirstOrDefaultAsync(t => t.Id == tenantId, cancellationToken);
        if (tenant is null)
        {
            return NotFound();
        }

        tenant.StorageBucketName = request.StorageBucketName;
        tenant.UpdatedOn = DateTimeOffset.UtcNow;
        await dbContext.SaveChangesAsync(cancellationToken);
        return NoContent();
    }
}
