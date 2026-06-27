using Fgs.Inventory.Application.Abstractions.Vendors;
using Fgs.Inventory.Application.Features.Vendors.Commands.CreateFgsVendor;
using Fgs.Inventory.Application.Features.Vendors.Commands.PatchFgsVendor;
using Fgs.Inventory.Application.Features.Vendors.Commands.UpdateFgsVendor;
using Fgs.Inventory.Domain.Entities;
using FluentValidation;

namespace Fgs.Inventory.Application.Features.Vendors.Validators;

public sealed class CreateFgsVendorCommandValidator : AbstractValidator<CreateFgsVendorCommand>
{
    private static readonly HashSet<string> AllowedVendorTypes = new(StringComparer.OrdinalIgnoreCase)
    {
        VendorTypes.Vendor,
        VendorTypes.Subcontractor
    };

    private static readonly HashSet<string> AllowedVendorStatuses = new(StringComparer.OrdinalIgnoreCase)
    {
        VendorStatuses.Active,
        VendorStatuses.Inactive,
        VendorStatuses.OnHold
    };

    public CreateFgsVendorCommandValidator(IFgsVendorReadRepository readRepository)
    {
        RuleFor(x => x.Dto.VendorCode).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Dto.VendorCode)
            .Must(code => string.Equals(code, code.Trim().ToUpperInvariant(), StringComparison.Ordinal))
            .WithMessage("VendorCode must be uppercase.");
        RuleFor(x => x.Dto.VendorCode)
            .MustAsync(async (command, code, cancellationToken) =>
                !await readRepository.ExistsByVendorCodeAsync(code, null, cancellationToken))
            .WithMessage("A vendor with this code already exists.");
        RuleFor(x => x.Dto.Name).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Dto.LegalName).MaximumLength(200);
        RuleFor(x => x.Dto.VendorType).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Dto.VendorType)
            .Must(type => AllowedVendorTypes.Contains(type))
            .WithMessage("VendorType must be VENDOR or SUBCONTRACTOR.");
        RuleFor(x => x.Dto.VendorStatus)
            .Must(status => status is null || AllowedVendorStatuses.Contains(status))
            .WithMessage("VendorStatus must be ACTIVE, INACTIVE, or ON_HOLD.");
        RuleFor(x => x.Dto.VendorAccountNumber).MaximumLength(100);
        RuleFor(x => x.Dto.PaymentTermId)
            .MustAsync(async (command, value, cancellationToken) =>
                !value.HasValue || await readRepository.ExistsPaymentTermIdAsync(value.Value, cancellationToken))
            .WithMessage("The specified payment term was not found.");
        RuleFor(x => x.Dto.ContactName).MaximumLength(150);
        RuleFor(x => x.Dto.ContactTitle).MaximumLength(100);
        RuleFor(x => x.Dto.Email).MaximumLength(255);
        RuleFor(x => x.Dto.PurchaseOrderEmail).MaximumLength(255);
        RuleFor(x => x.Dto.PhoneNumber).MaximumLength(50);
        RuleFor(x => x.Dto.MobileNumber).MaximumLength(50);
        RuleFor(x => x.Dto.FaxNumber).MaximumLength(50);
        RuleFor(x => x.Dto.Website).MaximumLength(255);
        RuleFor(x => x.Dto.Address1).MaximumLength(200);
        RuleFor(x => x.Dto.Address2).MaximumLength(200);
        RuleFor(x => x.Dto.City).MaximumLength(100);
        RuleFor(x => x.Dto.StateProvince).MaximumLength(100);
        RuleFor(x => x.Dto.PostalCode).MaximumLength(20);
        RuleFor(x => x.Dto.Country).MaximumLength(100);
        RuleFor(x => x.Dto.TaxIdNumber).MaximumLength(100);
        RuleFor(x => x.Dto.LicenseNumber).MaximumLength(100);
        RuleFor(x => x.Dto.InsurancePolicyNumber).MaximumLength(100);
    }
}

public sealed class UpdateFgsVendorCommandValidator : AbstractValidator<UpdateFgsVendorCommand>
{
    private static readonly HashSet<string> AllowedVendorTypes = new(StringComparer.OrdinalIgnoreCase)
    {
        VendorTypes.Vendor,
        VendorTypes.Subcontractor
    };

    private static readonly HashSet<string> AllowedVendorStatuses = new(StringComparer.OrdinalIgnoreCase)
    {
        VendorStatuses.Active,
        VendorStatuses.Inactive,
        VendorStatuses.OnHold
    };

    public UpdateFgsVendorCommandValidator(IFgsVendorReadRepository readRepository)
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Dto.VendorCode).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Dto.VendorCode)
            .Must(code => string.Equals(code, code.Trim().ToUpperInvariant(), StringComparison.Ordinal))
            .WithMessage("VendorCode must be uppercase.");
        RuleFor(x => x.Dto.VendorCode)
            .MustAsync(async (command, code, cancellationToken) =>
                !await readRepository.ExistsByVendorCodeAsync(code, command.Id, cancellationToken))
            .WithMessage("A vendor with this code already exists.");
        RuleFor(x => x.Dto.Name).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Dto.LegalName).MaximumLength(200);
        RuleFor(x => x.Dto.VendorType).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Dto.VendorType)
            .Must(type => AllowedVendorTypes.Contains(type))
            .WithMessage("VendorType must be VENDOR or SUBCONTRACTOR.");
        RuleFor(x => x.Dto.VendorStatus).NotEmpty().MaximumLength(30);
        RuleFor(x => x.Dto.VendorStatus)
            .Must(status => AllowedVendorStatuses.Contains(status))
            .WithMessage("VendorStatus must be ACTIVE, INACTIVE, or ON_HOLD.");
        RuleFor(x => x.Dto.VendorAccountNumber).MaximumLength(100);
        RuleFor(x => x.Dto.PaymentTermId)
            .MustAsync(async (command, value, cancellationToken) =>
                !value.HasValue || await readRepository.ExistsPaymentTermIdAsync(value.Value, cancellationToken))
            .WithMessage("The specified payment term was not found.");
        RuleFor(x => x.Dto.ContactName).MaximumLength(150);
        RuleFor(x => x.Dto.ContactTitle).MaximumLength(100);
        RuleFor(x => x.Dto.Email).MaximumLength(255);
        RuleFor(x => x.Dto.PurchaseOrderEmail).MaximumLength(255);
        RuleFor(x => x.Dto.PhoneNumber).MaximumLength(50);
        RuleFor(x => x.Dto.MobileNumber).MaximumLength(50);
        RuleFor(x => x.Dto.FaxNumber).MaximumLength(50);
        RuleFor(x => x.Dto.Website).MaximumLength(255);
        RuleFor(x => x.Dto.Address1).MaximumLength(200);
        RuleFor(x => x.Dto.Address2).MaximumLength(200);
        RuleFor(x => x.Dto.City).MaximumLength(100);
        RuleFor(x => x.Dto.StateProvince).MaximumLength(100);
        RuleFor(x => x.Dto.PostalCode).MaximumLength(20);
        RuleFor(x => x.Dto.Country).MaximumLength(100);
        RuleFor(x => x.Dto.TaxIdNumber).MaximumLength(100);
        RuleFor(x => x.Dto.LicenseNumber).MaximumLength(100);
        RuleFor(x => x.Dto.InsurancePolicyNumber).MaximumLength(100);
    }
}

public sealed class PatchFgsVendorCommandValidator : AbstractValidator<PatchFgsVendorCommand>
{
    private static readonly HashSet<string> AllowedVendorTypes = new(StringComparer.OrdinalIgnoreCase)
    {
        VendorTypes.Vendor,
        VendorTypes.Subcontractor
    };

    private static readonly HashSet<string> AllowedVendorStatuses = new(StringComparer.OrdinalIgnoreCase)
    {
        VendorStatuses.Active,
        VendorStatuses.Inactive,
        VendorStatuses.OnHold
    };

    public PatchFgsVendorCommandValidator(IFgsVendorReadRepository readRepository)
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Dto.VendorCode).NotEmpty().MaximumLength(50).When(x => x.Dto.VendorCode is not null);
        RuleFor(x => x.Dto.VendorCode)
            .Must(code => string.Equals(code!, code!.Trim().ToUpperInvariant(), StringComparison.Ordinal))
            .WithMessage("VendorCode must be uppercase.")
            .When(x => x.Dto.VendorCode is not null);
        RuleFor(x => x.Dto.VendorCode)
            .MustAsync(async (command, code, cancellationToken) =>
                !await readRepository.ExistsByVendorCodeAsync(code!, command.Id, cancellationToken))
            .WithMessage("A vendor with this code already exists.")
            .When(x => x.Dto.VendorCode is not null);
        RuleFor(x => x.Dto.Name).NotEmpty().MaximumLength(200).When(x => x.Dto.Name is not null);
        RuleFor(x => x.Dto.LegalName).MaximumLength(200).When(x => x.Dto.LegalName is not null);
        RuleFor(x => x.Dto.VendorType).NotEmpty().MaximumLength(50).When(x => x.Dto.VendorType is not null);
        RuleFor(x => x.Dto.VendorType)
            .Must(type => AllowedVendorTypes.Contains(type!))
            .WithMessage("VendorType must be VENDOR or SUBCONTRACTOR.")
            .When(x => x.Dto.VendorType is not null);
        RuleFor(x => x.Dto.VendorStatus)
            .Must(status => AllowedVendorStatuses.Contains(status!))
            .WithMessage("VendorStatus must be ACTIVE, INACTIVE, or ON_HOLD.")
            .When(x => x.Dto.VendorStatus is not null);
        RuleFor(x => x.Dto.VendorAccountNumber).MaximumLength(100).When(x => x.Dto.VendorAccountNumber is not null);
        RuleFor(x => x.Dto.PaymentTermId)
            .MustAsync(async (command, value, cancellationToken) =>
                !value.HasValue || await readRepository.ExistsPaymentTermIdAsync(value.Value, cancellationToken))
            .WithMessage("The specified payment term was not found.")
            .When(x => x.Dto.PaymentTermId.HasValue);
        RuleFor(x => x.Dto.ContactName).MaximumLength(150).When(x => x.Dto.ContactName is not null);
        RuleFor(x => x.Dto.ContactTitle).MaximumLength(100).When(x => x.Dto.ContactTitle is not null);
        RuleFor(x => x.Dto.Email).MaximumLength(255).When(x => x.Dto.Email is not null);
        RuleFor(x => x.Dto.PurchaseOrderEmail).MaximumLength(255).When(x => x.Dto.PurchaseOrderEmail is not null);
        RuleFor(x => x.Dto.PhoneNumber).MaximumLength(50).When(x => x.Dto.PhoneNumber is not null);
        RuleFor(x => x.Dto.MobileNumber).MaximumLength(50).When(x => x.Dto.MobileNumber is not null);
        RuleFor(x => x.Dto.FaxNumber).MaximumLength(50).When(x => x.Dto.FaxNumber is not null);
        RuleFor(x => x.Dto.Website).MaximumLength(255).When(x => x.Dto.Website is not null);
        RuleFor(x => x.Dto.Address1).MaximumLength(200).When(x => x.Dto.Address1 is not null);
        RuleFor(x => x.Dto.Address2).MaximumLength(200).When(x => x.Dto.Address2 is not null);
        RuleFor(x => x.Dto.City).MaximumLength(100).When(x => x.Dto.City is not null);
        RuleFor(x => x.Dto.StateProvince).MaximumLength(100).When(x => x.Dto.StateProvince is not null);
        RuleFor(x => x.Dto.PostalCode).MaximumLength(20).When(x => x.Dto.PostalCode is not null);
        RuleFor(x => x.Dto.Country).MaximumLength(100).When(x => x.Dto.Country is not null);
        RuleFor(x => x.Dto.TaxIdNumber).MaximumLength(100).When(x => x.Dto.TaxIdNumber is not null);
        RuleFor(x => x.Dto.LicenseNumber).MaximumLength(100).When(x => x.Dto.LicenseNumber is not null);
        RuleFor(x => x.Dto.InsurancePolicyNumber).MaximumLength(100).When(x => x.Dto.InsurancePolicyNumber is not null);
    }
}
