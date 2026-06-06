using Fgs.Contracts.Clients;
using Fgs.Setup.Application.Abstractions.Tenants;
using Fgs.Setup.Application.Abstractions.Time;
using Fgs.Setup.Domain.Entities;
using Fgs.Setup.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Fgs.Setup.Infrastructure.Tenants;

public sealed class CompanyBusinessTypeService(
    FgsSetupDbContext dbContext,
    IDateTimeProvider dateTime) : ICompanyBusinessTypeService
{
    public async Task AddCompanyBusinessTypesAsync(
        long tenantId,
        long companyId,
        AddCompanyBusinessTypesRequest request,
        CancellationToken cancellationToken = default)
    {
        var now = dateTime.UtcNow;

        var cache = await dbContext.FgsTenantCompanyCaches
            .FirstOrDefaultAsync(c => c.TenantId == tenantId && c.CompanyId == companyId, cancellationToken);

        if (cache is null)
        {
            cache = new FgsTenantCompanyCache
            {
                TenantId = tenantId,
                CompanyId = companyId,
                CompanyGuid = request.CompanyGuid,
                Code = request.Code.Trim(),
                Name = request.Name.Trim(),
                IsActive = request.IsActive,
                UpdatedOn = now
            };
            await dbContext.FgsTenantCompanyCaches.AddAsync(cache, cancellationToken);
        }
        else
        {
            cache.CompanyGuid = request.CompanyGuid;
            cache.Code = request.Code.Trim();
            cache.Name = request.Name.Trim();
            cache.IsActive = request.IsActive;
            cache.UpdatedOn = now;
            dbContext.FgsTenantCompanyCaches.Update(cache);
        }

        var requestedIds = request.BusinessTypeIds
            .Where(id => id > 0)
            .Distinct()
            .ToList();

        if (requestedIds.Count == 0)
        {
            await dbContext.SaveChangesAsync(cancellationToken);
            return;
        }

        var gloTypes = await dbContext.GloBusinessTypes
            .AsNoTracking()
            .Where(g => requestedIds.Contains(g.Id) && g.IsActive)
            .OrderBy(g => g.Id)
            .ToListAsync(cancellationToken);

        var existingCodes = await dbContext.FgsBusinessTypes
            .AsNoTracking()
            .Where(b => b.TenantId == tenantId && b.CompanyId == companyId)
            .Select(b => b.Code)
            .ToListAsync(cancellationToken);

        var existingCodeSet = existingCodes.ToHashSet(StringComparer.OrdinalIgnoreCase);
        short displayOrder = (short)(existingCodes.Count + 1);

        foreach (var gloType in gloTypes)
        {
            if (existingCodeSet.Contains(gloType.Code))
            {
                continue;
            }

            await dbContext.FgsBusinessTypes.AddAsync(
                new FgsBusinessType
                {
                    TenantId = tenantId,
                    CompanyId = companyId,
                    Code = gloType.Code,
                    Name = gloType.Name,
                    DisplayOrder = displayOrder++,
                    IsActive = true,
                    CreatedOn = now
                },
                cancellationToken);

            existingCodeSet.Add(gloType.Code);
        }

        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
