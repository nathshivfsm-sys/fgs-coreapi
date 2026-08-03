using Fgs.User.Application.Features.ServiceAccountsSetups.Commands.PatchFgsTenantServiceAccountsSetup;
using Fgs.User.Application.Features.ServiceAccountsSetups.Commands.UpdateFgsTenantServiceAccountsSetup;
using FluentValidation;

namespace Fgs.User.Application.Features.ServiceAccountsSetups.Validators;

public sealed class UpdateFgsTenantServiceAccountsSetupCommandValidator
    : AbstractValidator<UpdateFgsTenantServiceAccountsSetupCommand>
{
    public UpdateFgsTenantServiceAccountsSetupCommandValidator()
    {
        RuleFor(x => x.Dto.BankAccountId).GreaterThan(0).When(x => x.Dto.BankAccountId.HasValue);
        RuleFor(x => x.Dto.AccountsReceivableAccountId).GreaterThan(0).When(x => x.Dto.AccountsReceivableAccountId.HasValue);
        RuleFor(x => x.Dto.RevenueAccountId).GreaterThan(0).When(x => x.Dto.RevenueAccountId.HasValue);
        RuleFor(x => x.Dto.DiscountAccountId).GreaterThan(0).When(x => x.Dto.DiscountAccountId.HasValue);
        RuleFor(x => x.Dto.SalesTaxPayableAccountId).GreaterThan(0).When(x => x.Dto.SalesTaxPayableAccountId.HasValue);
        RuleFor(x => x.Dto.InventoryAccountId).GreaterThan(0).When(x => x.Dto.InventoryAccountId.HasValue);
        RuleFor(x => x.Dto.COGSAccountId).GreaterThan(0).When(x => x.Dto.COGSAccountId.HasValue);
        RuleFor(x => x.Dto.UndepositedFundsAccountId).GreaterThan(0).When(x => x.Dto.UndepositedFundsAccountId.HasValue);
        RuleFor(x => x.Dto.ProcessingFeeAccountId).GreaterThan(0).When(x => x.Dto.ProcessingFeeAccountId.HasValue);
        RuleFor(x => x.Dto.AccountsPayableAccountId).GreaterThan(0).When(x => x.Dto.AccountsPayableAccountId.HasValue);
    }
}

public sealed class PatchFgsTenantServiceAccountsSetupCommandValidator
    : AbstractValidator<PatchFgsTenantServiceAccountsSetupCommand>
{
    public PatchFgsTenantServiceAccountsSetupCommandValidator()
    {
        RuleFor(x => x.Dto.BankAccountId).GreaterThan(0).When(x => x.Dto.BankAccountId.HasValue);
        RuleFor(x => x.Dto.AccountsReceivableAccountId).GreaterThan(0).When(x => x.Dto.AccountsReceivableAccountId.HasValue);
        RuleFor(x => x.Dto.RevenueAccountId).GreaterThan(0).When(x => x.Dto.RevenueAccountId.HasValue);
        RuleFor(x => x.Dto.DiscountAccountId).GreaterThan(0).When(x => x.Dto.DiscountAccountId.HasValue);
        RuleFor(x => x.Dto.SalesTaxPayableAccountId).GreaterThan(0).When(x => x.Dto.SalesTaxPayableAccountId.HasValue);
        RuleFor(x => x.Dto.InventoryAccountId).GreaterThan(0).When(x => x.Dto.InventoryAccountId.HasValue);
        RuleFor(x => x.Dto.COGSAccountId).GreaterThan(0).When(x => x.Dto.COGSAccountId.HasValue);
        RuleFor(x => x.Dto.UndepositedFundsAccountId).GreaterThan(0).When(x => x.Dto.UndepositedFundsAccountId.HasValue);
        RuleFor(x => x.Dto.ProcessingFeeAccountId).GreaterThan(0).When(x => x.Dto.ProcessingFeeAccountId.HasValue);
        RuleFor(x => x.Dto.AccountsPayableAccountId).GreaterThan(0).When(x => x.Dto.AccountsPayableAccountId.HasValue);
    }
}
