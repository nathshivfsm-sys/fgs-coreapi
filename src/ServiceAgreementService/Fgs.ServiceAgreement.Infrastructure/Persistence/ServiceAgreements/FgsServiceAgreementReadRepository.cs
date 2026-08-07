using Fgs.Foundation.Paging;
using Fgs.ServiceAgreement.Application.Abstractions.ServiceAgreements;
using Fgs.ServiceAgreement.Application.Common.ServiceAgreementCrud;
using Fgs.ServiceAgreement.Application.Features.ServiceAgreements.Dtos;
using Fgs.ServiceAgreement.Domain.Entities;
using Fgs.ServiceAgreement.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Fgs.ServiceAgreement.Infrastructure.Persistence.ServiceAgreements;

internal sealed class FgsServiceAgreementReadRepository(FgsServiceAgreementDbContext dbContext)
    : IFgsServiceAgreementReadRepository
{
    public async Task<FgsServiceAgreementDetailDto?> GetByIdAsync(
        long id,
        CancellationToken cancellationToken = default)
    {
        var entity = await dbContext.FgsServiceAgreements
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Id == id, cancellationToken);

        return entity is null ? null : MapToDetail(entity);
    }

    public async Task<PagedResult<FgsServiceAgreementSummaryDto>> ListAsync(
        ServiceAgreementListQuery query,
        FgsServiceAgreementListFilters filters,
        CancellationToken cancellationToken = default)
    {
        var paging = query.ToPagedQuery();
        var page = Math.Max(1, paging.Page);
        var pageSize = Math.Clamp(paging.PageSize, 1, 200);

        var dbQuery = dbContext.FgsServiceAgreements.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(filters.AgreementNumber))
        {
            var agreementNumber = filters.AgreementNumber.Trim().ToUpperInvariant();
            dbQuery = dbQuery.Where(e => e.AgreementNumber == agreementNumber);
        }

        if (filters.CustomerId.HasValue)
        {
            dbQuery = dbQuery.Where(e => e.CustomerId == filters.CustomerId.Value);
        }

        if (filters.ServiceAgreementStatusId.HasValue)
        {
            dbQuery = dbQuery.Where(e => e.ServiceAgreementStatusId == filters.ServiceAgreementStatusId.Value);
        }

        if (!string.IsNullOrWhiteSpace(paging.Search))
        {
            var pattern = $"%{paging.Search.Trim()}%";
            dbQuery = dbQuery.Where(e =>
                EF.Functions.ILike(e.AgreementNumber, pattern)
                || EF.Functions.ILike(e.Name, pattern)
                || (e.Description != null && EF.Functions.ILike(e.Description, pattern)));
        }

        dbQuery = ApplySort(dbQuery, paging.SortBy, paging.SortDirection);

        var totalCount = await dbQuery.CountAsync(cancellationToken);
        var items = await dbQuery
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(e => new FgsServiceAgreementSummaryDto(
                e.Id,
                e.AgreementNumber,
                e.Name,
                e.CustomerId,
                e.CustomerLocationId,
                e.StartDate,
                e.EndDate,
                e.ServiceAgreementStatusId,
                e.ContractAmount))
            .ToListAsync(cancellationToken);

        return new PagedResult<FgsServiceAgreementSummaryDto>(items, page, pageSize, totalCount);
    }

    public async Task<bool> ExistsByAgreementNumberAsync(
        string agreementNumber,
        long? excludeId = null,
        CancellationToken cancellationToken = default)
    {
        var normalized = agreementNumber.Trim().ToUpperInvariant();
        var dbQuery = dbContext.FgsServiceAgreements.AsNoTracking()
            .Where(e => e.AgreementNumber == normalized);

        if (excludeId.HasValue)
        {
            dbQuery = dbQuery.Where(e => e.Id != excludeId.Value);
        }

        return await dbQuery.AnyAsync(cancellationToken);
    }

    private static IQueryable<FgsServiceAgreement> ApplySort(
        IQueryable<FgsServiceAgreement> query,
        string? sortBy,
        SortDirection direction)
    {
        var desc = direction == SortDirection.Desc;
        return (sortBy?.Trim().ToLowerInvariant()) switch
        {
            "agreementnumber" => desc
                ? query.OrderByDescending(e => e.AgreementNumber)
                : query.OrderBy(e => e.AgreementNumber),
            "name" => desc ? query.OrderByDescending(e => e.Name) : query.OrderBy(e => e.Name),
            "startdate" => desc ? query.OrderByDescending(e => e.StartDate) : query.OrderBy(e => e.StartDate),
            "enddate" => desc ? query.OrderByDescending(e => e.EndDate) : query.OrderBy(e => e.EndDate),
            "serviceagreementstatusid" => desc
                ? query.OrderByDescending(e => e.ServiceAgreementStatusId)
                : query.OrderBy(e => e.ServiceAgreementStatusId),
            "contractamount" => desc
                ? query.OrderByDescending(e => e.ContractAmount)
                : query.OrderBy(e => e.ContractAmount),
            "createdon" => desc ? query.OrderByDescending(e => e.CreatedOn) : query.OrderBy(e => e.CreatedOn),
            _ => desc ? query.OrderByDescending(e => e.Id) : query.OrderBy(e => e.Id)
        };
    }

    internal static FgsServiceAgreementDetailDto MapToDetail(FgsServiceAgreement entity) =>
        new(
            entity.Id,
            entity.AgreementNumber,
            entity.CustomerId,
            entity.CustomerLocationId,
            entity.EstimateId,
            entity.Name,
            entity.Description,
            entity.Break1Id,
            entity.Break2Id,
            entity.JobTypeId,
            entity.StartDate,
            entity.EndDate,
            entity.ServiceAgreementStatusId,
            entity.VisitFrequencyId,
            entity.BillingFrequencyId,
            entity.ContractAmount,
            entity.LaborDiscountPercent,
            entity.MaterialDiscountPercent,
            entity.AutoRenew,
            entity.RenewedByServiceAgreementId,
            entity.SoldDate,
            entity.SoldByEmployeeId,
            entity.ActivatedOn,
            entity.CancelledOn,
            entity.ExternalEntityId,
            entity.ExternalVersion);
}
