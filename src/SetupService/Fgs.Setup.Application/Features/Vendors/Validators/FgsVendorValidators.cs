using Fgs.Setup.Application.Abstractions.Vendors;
using Fgs.Setup.Application.Features.Vendors.Commands.CreateFgsVendor;
using Fgs.Setup.Application.Features.Vendors.Commands.PatchFgsVendor;
using Fgs.Setup.Application.Features.Vendors.Commands.UpdateFgsVendor;
using FluentValidation;

namespace Fgs.Setup.Application.Features.Vendors.Validators;

public sealed class CreateFgsVendorCommandValidator : AbstractValidator<CreateFgsVendorCommand>
{
    public CreateFgsVendorCommandValidator(IFgsVendorReadRepository readRepository)
    {
        RuleFor(x => x.Dto.VendorCode).NotEmpty();
        RuleFor(x => x.Dto.VendorCode).MaximumLength(50);
        RuleFor(x => x.Dto.VendorCode).Must(code => string.Equals(code, code.Trim().ToUpperInvariant(), StringComparison.Ordinal)).WithMessage("VendorCode must be uppercase.");
        RuleFor(x => x.Dto.VendorCode).MustAsync(async (command, code, cancellationToken) =>
                !await readRepository.ExistsByVendorCodeAsync(code, null, cancellationToken))
            .WithMessage("A vendor with this code already exists.");
        RuleFor(x => x.Dto.Name).NotEmpty();
        RuleFor(x => x.Dto.Name).MaximumLength(200);
        RuleFor(x => x.Dto.LegalName).MaximumLength(200);
        RuleFor(x => x.Dto.VendorType).NotEmpty();
        RuleFor(x => x.Dto.VendorType).MaximumLength(50);
        RuleFor(x => x.Dto.PaymentTermId).MustAsync(async (command, value, cancellationToken) =>
                !value.HasValue || await readRepository.ExistsPaymentTermIdAsync(value.Value, cancellationToken))
            .WithMessage("The specified payment term was not found.");
        RuleFor(x => x.Dto.Email).MaximumLength(255);
        RuleFor(x => x.Dto.PhoneNumber).MaximumLength(50);
        RuleFor(x => x.Dto.MobileNumber).MaximumLength(50);
        RuleFor(x => x.Dto.Website).MaximumLength(255);
        RuleFor(x => x.Dto.TaxIdentificationNumber).MaximumLength(100);
        RuleFor(x => x.Dto.LicenseNumber).MaximumLength(100);
        RuleFor(x => x.Dto.InsurancePolicyNumber).MaximumLength(100);

        RuleFor(x => x.Dto.VendorCode).NotEmpty();
        RuleFor(x => x.Dto.VendorCode).MaximumLength(50);
        RuleFor(x => x.Dto.VendorCode).Must(code => string.Equals(code, code.Trim().ToUpperInvariant(), StringComparison.Ordinal)).WithMessage("VendorCode must be uppercase.");
        RuleFor(x => x.Dto.VendorCode).MustAsync(async (command, code, cancellationToken) =>
                !await readRepository.ExistsByVendorCodeAsync(code, null, cancellationToken))
            .WithMessage("A vendor with this code already exists.");
        RuleFor(x => x.Dto.Name).NotEmpty();
        RuleFor(x => x.Dto.Name).MaximumLength(200);
        RuleFor(x => x.Dto.LegalName).MaximumLength(200);
        RuleFor(x => x.Dto.VendorType).NotEmpty();
        RuleFor(x => x.Dto.VendorType).MaximumLength(50);
        RuleFor(x => x.Dto.PaymentTermId).MustAsync(async (command, value, cancellationToken) =>
                !value.HasValue || await readRepository.ExistsPaymentTermIdAsync(value.Value, cancellationToken))
            .WithMessage("The specified payment term was not found.");
        RuleFor(x => x.Dto.Email).MaximumLength(255);
        RuleFor(x => x.Dto.PhoneNumber).MaximumLength(50);
        RuleFor(x => x.Dto.MobileNumber).MaximumLength(50);
        RuleFor(x => x.Dto.Website).MaximumLength(255);
        RuleFor(x => x.Dto.TaxIdentificationNumber).MaximumLength(100);
        RuleFor(x => x.Dto.LicenseNumber).MaximumLength(100);
        RuleFor(x => x.Dto.InsurancePolicyNumber).MaximumLength(100);


    }
}

public sealed class UpdateFgsVendorCommandValidator : AbstractValidator<UpdateFgsVendorCommand>
{
    public UpdateFgsVendorCommandValidator(IFgsVendorReadRepository readRepository)
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Dto.VendorCode).NotEmpty();
        RuleFor(x => x.Dto.VendorCode).MaximumLength(50);
        RuleFor(x => x.Dto.VendorCode).Must(code => string.Equals(code, code.Trim().ToUpperInvariant(), StringComparison.Ordinal)).WithMessage("VendorCode must be uppercase.");
        RuleFor(x => x.Dto.VendorCode).MustAsync(async (command, code, cancellationToken) =>
                !await readRepository.ExistsByVendorCodeAsync(code, command.Id, cancellationToken))
            .WithMessage("A vendor with this code already exists.");
        RuleFor(x => x.Dto.Name).NotEmpty();
        RuleFor(x => x.Dto.Name).MaximumLength(200);
        RuleFor(x => x.Dto.LegalName).MaximumLength(200);
        RuleFor(x => x.Dto.VendorType).NotEmpty();
        RuleFor(x => x.Dto.VendorType).MaximumLength(50);
        RuleFor(x => x.Dto.PaymentTermId).MustAsync(async (command, value, cancellationToken) =>
                !value.HasValue || await readRepository.ExistsPaymentTermIdAsync(value.Value, cancellationToken))
            .WithMessage("The specified payment term was not found.");
        RuleFor(x => x.Dto.Email).MaximumLength(255);
        RuleFor(x => x.Dto.PhoneNumber).MaximumLength(50);
        RuleFor(x => x.Dto.MobileNumber).MaximumLength(50);
        RuleFor(x => x.Dto.Website).MaximumLength(255);
        RuleFor(x => x.Dto.TaxIdentificationNumber).MaximumLength(100);
        RuleFor(x => x.Dto.LicenseNumber).MaximumLength(100);
        RuleFor(x => x.Dto.InsurancePolicyNumber).MaximumLength(100);


    }
}

public sealed class PatchFgsVendorCommandValidator : AbstractValidator<PatchFgsVendorCommand>
{
    public PatchFgsVendorCommandValidator(IFgsVendorReadRepository readRepository)
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Dto.VendorCode).NotEmpty();
        RuleFor(x => x.Dto.VendorCode).MaximumLength(50);
        RuleFor(x => x.Dto.VendorCode).Must(code => string.Equals(code!, code!.Trim().ToUpperInvariant(), StringComparison.Ordinal)).WithMessage("VendorCode must be uppercase.");
        RuleFor(x => x.Dto.VendorCode).MustAsync(async (command, code, cancellationToken) =>
                !await readRepository.ExistsByVendorCodeAsync(code!, command.Id, cancellationToken))
            .WithMessage("A vendor with this code already exists.");
        RuleFor(x => x.Dto.Name).NotEmpty();
        RuleFor(x => x.Dto.Name).MaximumLength(200);
        RuleFor(x => x.Dto.LegalName).MaximumLength(200).When(x => x.Dto.LegalName is not null);
        RuleFor(x => x.Dto.VendorType).NotEmpty();
        RuleFor(x => x.Dto.VendorType).MaximumLength(50);
        RuleFor(x => x.Dto.PaymentTermId).MustAsync(async (command, value, cancellationToken) =>
                !value.HasValue || await readRepository.ExistsPaymentTermIdAsync(value.Value, cancellationToken))
            .WithMessage("The specified payment term was not found.").When(x => x.Dto.PaymentTermId.HasValue);
        RuleFor(x => x.Dto.Email).MaximumLength(255).When(x => x.Dto.Email is not null);
        RuleFor(x => x.Dto.PhoneNumber).MaximumLength(50).When(x => x.Dto.PhoneNumber is not null);
        RuleFor(x => x.Dto.MobileNumber).MaximumLength(50).When(x => x.Dto.MobileNumber is not null);
        RuleFor(x => x.Dto.Website).MaximumLength(255).When(x => x.Dto.Website is not null);
        RuleFor(x => x.Dto.TaxIdentificationNumber).MaximumLength(100).When(x => x.Dto.TaxIdentificationNumber is not null);
        RuleFor(x => x.Dto.LicenseNumber).MaximumLength(100).When(x => x.Dto.LicenseNumber is not null);
        RuleFor(x => x.Dto.InsurancePolicyNumber).MaximumLength(100).When(x => x.Dto.InsurancePolicyNumber is not null);


    }
}
