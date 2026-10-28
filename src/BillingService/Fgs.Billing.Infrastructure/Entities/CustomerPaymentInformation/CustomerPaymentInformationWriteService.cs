using Fgs.Billing.Application.Abstractions.CustomerPaymentInformation;
using Fgs.Billing.Application.Features.CustomerPaymentInformation;
using Fgs.Billing.Application.Features.CustomerPaymentInformation.Dtos;
using Fgs.Billing.Domain.Entities;
using Fgs.Billing.Infrastructure.Common;
using Fgs.Billing.Infrastructure.Database;
using Fgs.Persistence.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace Fgs.Billing.Infrastructure.Entities.CustomerPaymentInformation;

public sealed class CustomerPaymentInformationWriteService(
    FgsBillingDbContext context,
    IUnitOfWork unitOfWork,
    BillingEntityAuditHelper auditHelper) : ICustomerPaymentInformationWriteService
{
    public Task<CustomerPaymentInformationGridDto> CreateAsync(
        CustomerPaymentInformationCreateModel model,
        CancellationToken cancellationToken = default) =>
        unitOfWork.ExecuteInTransactionAsync(
            async ct =>
            {
                if (model.SetAsDefault)
                {
                    await ClearActiveDefaultsAsync(model.ArCustomerId, excludeId: null, ct);
                    await unitOfWork.SaveChangesAsync(ct);
                }

                var entity = new FgsARCustomerPaymentInformation
                {
                    ARCustomerId = model.ArCustomerId,
                    PaymentMethodType = CustomerPaymentMethods.Card,
                    CardType = CustomerPaymentMethods.Credit,
                    WalletType = null,
                    PaymentToken = model.CustomerPaymentProfileId,
                    CustomerProfileId = model.CustomerProfileId,
                    CustomerPaymentProfileId = model.CustomerPaymentProfileId,
                    CardHolderName = model.CardHolderName.Trim(),
                    CardBrand = TrimTo(model.CardBrand, 50),
                    Last4 = model.Last4,
                    ExpirationMonth = model.ExpirationMonth,
                    ExpirationYear = model.ExpirationYear,
                    BillingAddressId = null,
                    IsContractBilling = false,
                    IsDefault = model.SetAsDefault,
                    IsActive = true
                };

                auditHelper.StampForCreate(entity);
                await context.FgsARCustomerPaymentInformations.AddAsync(entity, ct);
                await unitOfWork.SaveChangesAsync(ct);
                return ToGrid(entity);
            },
            cancellationToken);

    public async Task<CustomerPaymentInformationGridDto?> UpdateAsync(
        long id,
        CustomerPaymentInformationUpdateModel model,
        CancellationToken cancellationToken = default)
    {
        var entity = await FindCardAsync(id, cancellationToken);
        if (entity is null)
        {
            return null;
        }

        return await unitOfWork.ExecuteInTransactionAsync(
            async ct =>
            {
                if (model.IsDefault)
                {
                    await ClearActiveDefaultsAsync(entity.ARCustomerId, entity.Id, ct);
                    await unitOfWork.SaveChangesAsync(ct);
                }

                entity.CardHolderName = model.CardHolderName.Trim();
                entity.ExpirationMonth = model.ExpirationMonth;
                entity.ExpirationYear = model.ExpirationYear;
                entity.IsDefault = model.IsDefault;
                if (model.CardBrand is not null)
                {
                    entity.CardBrand = TrimTo(model.CardBrand, 50);
                }

                if (!string.IsNullOrWhiteSpace(model.Last4))
                {
                    entity.Last4 = model.Last4;
                }

                auditHelper.StampForUpdate(entity);
                await unitOfWork.SaveChangesAsync(ct);
                return ToGrid(entity);
            },
            cancellationToken);
    }

    public async Task<CustomerPaymentInformationGridDto?> SoftDeleteAsync(
        long id,
        CancellationToken cancellationToken = default)
    {
        var entity = await FindCardAsync(id, cancellationToken);
        if (entity is null)
        {
            return null;
        }

        return await unitOfWork.ExecuteInTransactionAsync(
            async ct =>
            {
                entity.IsActive = false;
                auditHelper.StampForUpdate(entity);
                await unitOfWork.SaveChangesAsync(ct);
                return ToGrid(entity);
            },
            cancellationToken);
    }

    private async Task ClearActiveDefaultsAsync(
        long arCustomerId,
        long? excludeId,
        CancellationToken cancellationToken)
    {
        var defaults = await context.FgsARCustomerPaymentInformations
            .Where(e => e.ARCustomerId == arCustomerId && e.IsDefault && e.IsActive)
            .ToListAsync(cancellationToken);

        foreach (var row in defaults)
        {
            if (excludeId.HasValue && row.Id == excludeId.Value)
            {
                continue;
            }

            row.IsDefault = false;
            auditHelper.StampForUpdate(row);
        }
    }

    private Task<FgsARCustomerPaymentInformation?> FindCardAsync(long id, CancellationToken cancellationToken) =>
        context.FgsARCustomerPaymentInformations
            .FirstOrDefaultAsync(
                e => e.Id == id && e.PaymentMethodType == CustomerPaymentMethods.Card,
                cancellationToken);

    private static CustomerPaymentInformationGridDto ToGrid(FgsARCustomerPaymentInformation entity) =>
        CustomerPaymentInformationGrid.Create(
            entity.Id,
            entity.CardBrand,
            entity.CardHolderName,
            entity.Last4,
            entity.ExpirationMonth,
            entity.ExpirationYear,
            entity.IsDefault,
            entity.IsActive);

    private static string? TrimTo(string? value, int maxLength)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return null;
        }

        var trimmed = value.Trim();
        return trimmed.Length <= maxLength ? trimmed : trimmed[..maxLength];
    }
}
