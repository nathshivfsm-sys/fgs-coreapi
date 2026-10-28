using Fgs.Billing.Application.Abstractions.CustomerPaymentInformation;
using Fgs.Billing.Application.Common.BillingCrud;
using Fgs.Billing.Application.Features.CustomerPaymentInformation;
using Fgs.Billing.Application.Features.CustomerPaymentInformation.Dtos;
using Fgs.Billing.Domain.Entities;
using Fgs.Billing.Infrastructure.Database;
using Fgs.Foundation.Paging;
using Fgs.MultiTenancy;
using Microsoft.EntityFrameworkCore;

namespace Fgs.Billing.Infrastructure.Entities.CustomerPaymentInformation;

public sealed class CustomerPaymentInformationReadRepository(FgsBillingDbContext dbContext)
    : ICustomerPaymentInformationReadRepository
{
    public async Task<CustomerPaymentInformationCard?> GetCardAsync(
        long id,
        CancellationToken cancellationToken = default)
    {
        var entity = await dbContext.FgsARCustomerPaymentInformations
            .AsNoTracking()
            .FirstOrDefaultAsync(
                e => e.Id == id && e.PaymentMethodType == CustomerPaymentMethods.Card,
                cancellationToken);

        if (entity is null)
        {
            return null;
        }

        return new CustomerPaymentInformationCard(
            entity.Id,
            entity.ARCustomerId,
            entity.CustomerProfileId,
            entity.CustomerPaymentProfileId,
            entity.CardBrand,
            entity.CardHolderName,
            entity.Last4,
            entity.ExpirationMonth,
            entity.ExpirationYear,
            entity.IsDefault,
            entity.IsActive);
    }

    public async Task<string?> FindCustomerProfileIdAsync(
        long arCustomerId,
        CancellationToken cancellationToken = default)
    {
        using (SoftDeleteFilterAccessor.BeginSuppress())
        {
            return await dbContext.FgsARCustomerPaymentInformations
                .AsNoTracking()
                .Where(e =>
                    e.ARCustomerId == arCustomerId
                    && e.CustomerProfileId != null
                    && e.CustomerProfileId != "")
                .Select(e => e.CustomerProfileId)
                .FirstOrDefaultAsync(cancellationToken);
        }
    }

    public async Task<CustomerPaymentInformationListDto> ListAsync(
        long arCustomerId,
        BillingListQuery query,
        bool isActive,
        CancellationToken cancellationToken = default)
    {
        var activeCount = await dbContext.FgsARCustomerPaymentInformations
            .AsNoTracking()
            .CountAsync(
                e => e.ARCustomerId == arCustomerId && e.PaymentMethodType == CustomerPaymentMethods.Card,
                cancellationToken);

        if (!isActive)
        {
            using (SoftDeleteFilterAccessor.BeginSuppress())
            {
                return await QueryPageAsync(arCustomerId, query, false, activeCount, cancellationToken);
            }
        }

        return await QueryPageAsync(arCustomerId, query, true, activeCount, cancellationToken);
    }

    private async Task<CustomerPaymentInformationListDto> QueryPageAsync(
        long arCustomerId,
        BillingListQuery query,
        bool isActive,
        int activeCount,
        CancellationToken cancellationToken)
    {
        var paging = query.ToPagedQuery();
        var page = Math.Max(1, paging.Page);
        var pageSize = Math.Clamp(paging.PageSize, 1, 200);

        var dbQuery = dbContext.FgsARCustomerPaymentInformations
            .AsNoTracking()
            .Where(e =>
                e.ARCustomerId == arCustomerId
                && e.PaymentMethodType == CustomerPaymentMethods.Card
                && e.IsActive == isActive);

        if (!string.IsNullOrWhiteSpace(paging.Search))
        {
            var pattern = $"%{paging.Search.Trim()}%";
            dbQuery = dbQuery.Where(e =>
                (e.CardHolderName != null && EF.Functions.ILike(e.CardHolderName, pattern))
                || (e.CardBrand != null && EF.Functions.ILike(e.CardBrand, pattern))
                || (e.Last4 != null && EF.Functions.ILike(e.Last4, pattern)));
        }

        dbQuery = ApplySort(dbQuery, paging.SortBy, paging.SortDirection);

        var totalCount = await dbQuery.CountAsync(cancellationToken);
        var rows = await dbQuery
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(e => new
            {
                e.Id,
                e.CardBrand,
                e.CardHolderName,
                e.Last4,
                e.ExpirationMonth,
                e.ExpirationYear,
                e.IsDefault,
                e.IsActive
            })
            .ToListAsync(cancellationToken);

        var items = rows
            .Select(e => CustomerPaymentInformationGrid.Create(
                e.Id,
                e.CardBrand,
                e.CardHolderName,
                e.Last4,
                e.ExpirationMonth,
                e.ExpirationYear,
                e.IsDefault,
                e.IsActive))
            .ToList();

        return new CustomerPaymentInformationListDto(items, page, pageSize, totalCount, activeCount);
    }

    private static IQueryable<FgsARCustomerPaymentInformation> ApplySort(
        IQueryable<FgsARCustomerPaymentInformation> query,
        string? sortBy,
        SortDirection direction)
    {
        if (string.IsNullOrWhiteSpace(sortBy))
        {
            return query
                .OrderByDescending(e => e.IsDefault)
                .ThenBy(e => e.CardHolderName);
        }

        var desc = direction == SortDirection.Desc;
        return sortBy.Trim().ToLowerInvariant() switch
        {
            "cardholdername" => desc
                ? query.OrderByDescending(e => e.CardHolderName)
                : query.OrderBy(e => e.CardHolderName),
            "cardbrand" => desc
                ? query.OrderByDescending(e => e.CardBrand)
                : query.OrderBy(e => e.CardBrand),
            "last4" => desc
                ? query.OrderByDescending(e => e.Last4)
                : query.OrderBy(e => e.Last4),
            "expiry" or "expiration" => desc
                ? query.OrderByDescending(e => e.ExpirationYear).ThenByDescending(e => e.ExpirationMonth)
                : query.OrderBy(e => e.ExpirationYear).ThenBy(e => e.ExpirationMonth),
            "isdefault" => desc
                ? query.OrderByDescending(e => e.IsDefault)
                : query.OrderBy(e => e.IsDefault),
            "isactive" => desc
                ? query.OrderByDescending(e => e.IsActive)
                : query.OrderBy(e => e.IsActive),
            "id" => desc
                ? query.OrderByDescending(e => e.Id)
                : query.OrderBy(e => e.Id),
            _ => query.OrderByDescending(e => e.IsDefault).ThenBy(e => e.CardHolderName)
        };
    }
}
