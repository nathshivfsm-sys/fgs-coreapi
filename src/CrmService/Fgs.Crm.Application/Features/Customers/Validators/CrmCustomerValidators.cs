using Fgs.Crm.Application.Abstractions.Customers;
using Fgs.Crm.Application.Features.Customers.Commands.CreateCrmCustomer;
using Fgs.Crm.Application.Features.Customers.Commands.PatchCrmCustomer;
using Fgs.Crm.Application.Features.Customers.Commands.UpdateCrmCustomer;
using FluentValidation;

namespace Fgs.Crm.Application.Features.Customers.Validators;

public sealed class CreateCrmCustomerCommandValidator : AbstractValidator<CreateCrmCustomerCommand>
{
    public CreateCrmCustomerCommandValidator(ICrmCustomerReadRepository readRepository)
    {
        RuleFor(x => x.Dto)
            .NotNull()
            .WithMessage(
                "Request body is required. Ensure the JSON is valid (unresolved Postman variables produce invalid JSON).");

        When(x => x.Dto is not null, () =>
        {
            RuleFor(x => x.Dto.CustomerNumber).NotEmpty().MaximumLength(30);
            RuleFor(x => x.Dto.CustomerNumber)
                .Must(code => string.Equals(code, code.Trim().ToUpperInvariant(), StringComparison.Ordinal))
                .WithMessage("CustomerNumber must be uppercase.");
            RuleFor(x => x.Dto.CustomerNumber)
                .MustAsync(async (command, number, cancellationToken) =>
                    !await readRepository.ExistsByCustomerNumberAsync(number, null, cancellationToken))
                .WithMessage("A customer with this number already exists.");
            RuleFor(x => x.Dto.Name).NotEmpty().MaximumLength(200);
            RuleFor(x => x.Dto.DisplayName).NotEmpty().MaximumLength(200);
            ApplyAddressRules(this);
            RuleFor(x => x.Dto.TaxExemptNumber).MaximumLength(100);
            RuleFor(x => x.Dto.CustomerAccountNumber).MaximumLength(100);
            RuleFor(x => x.Dto.ExternalEntityId).MaximumLength(200);
            RuleFor(x => x.Dto.ExternalVersion).MaximumLength(100);
            RuleFor(x => x.Dto.PlaceId).MaximumLength(500);
            RuleFor(x => x.Dto.FormattedAddress).MaximumLength(1000);
        });
    }

    private static void ApplyAddressRules(AbstractValidator<CreateCrmCustomerCommand> validator)
    {
        validator.RuleFor(x => x.Dto.AddressLine1).MaximumLength(200);
        validator.RuleFor(x => x.Dto.AddressLine2).MaximumLength(200);
        validator.RuleFor(x => x.Dto.AddressLine3).MaximumLength(200);
        validator.RuleFor(x => x.Dto.AddressLine4).MaximumLength(200);
        validator.RuleFor(x => x.Dto.City).MaximumLength(100);
        validator.RuleFor(x => x.Dto.State).MaximumLength(100);
        validator.RuleFor(x => x.Dto.County).MaximumLength(100);
        validator.RuleFor(x => x.Dto.Country).MaximumLength(100);
        validator.RuleFor(x => x.Dto.PostalCode).MaximumLength(20);
    }
}

public sealed class UpdateCrmCustomerCommandValidator : AbstractValidator<UpdateCrmCustomerCommand>
{
    public UpdateCrmCustomerCommandValidator(ICrmCustomerReadRepository readRepository)
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Dto)
            .NotNull()
            .WithMessage(
                "Request body is required. Ensure the JSON is valid (unresolved Postman variables produce invalid JSON).");

        When(x => x.Dto is not null, () =>
        {
            RuleFor(x => x.Dto.CustomerNumber).NotEmpty().MaximumLength(30);
            RuleFor(x => x.Dto.CustomerNumber)
                .Must(code => string.Equals(code, code.Trim().ToUpperInvariant(), StringComparison.Ordinal))
                .WithMessage("CustomerNumber must be uppercase.");
            RuleFor(x => x.Dto.CustomerNumber)
                .MustAsync(async (command, number, cancellationToken) =>
                    !await readRepository.ExistsByCustomerNumberAsync(number, command.Id, cancellationToken))
                .WithMessage("A customer with this number already exists.");
            RuleFor(x => x.Dto.Name).NotEmpty().MaximumLength(200);
            RuleFor(x => x.Dto.DisplayName).NotEmpty().MaximumLength(200);
            RuleFor(x => x.Dto.AddressLine1).MaximumLength(200);
            RuleFor(x => x.Dto.AddressLine2).MaximumLength(200);
            RuleFor(x => x.Dto.AddressLine3).MaximumLength(200);
            RuleFor(x => x.Dto.AddressLine4).MaximumLength(200);
            RuleFor(x => x.Dto.City).MaximumLength(100);
            RuleFor(x => x.Dto.State).MaximumLength(100);
            RuleFor(x => x.Dto.County).MaximumLength(100);
            RuleFor(x => x.Dto.Country).MaximumLength(100);
            RuleFor(x => x.Dto.PostalCode).MaximumLength(20);
            RuleFor(x => x.Dto.FormattedAddress).MaximumLength(1000);
            RuleFor(x => x.Dto.PlaceId).MaximumLength(500);
            RuleFor(x => x.Dto.TaxExemptNumber).MaximumLength(100);
            RuleFor(x => x.Dto.CustomerAccountNumber).MaximumLength(100);
            RuleFor(x => x.Dto.ExternalEntityId).MaximumLength(200);
            RuleFor(x => x.Dto.ExternalVersion).MaximumLength(100);
        });
    }
}

public sealed class PatchCrmCustomerCommandValidator : AbstractValidator<PatchCrmCustomerCommand>
{
    public PatchCrmCustomerCommandValidator(ICrmCustomerReadRepository readRepository)
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Dto)
            .NotNull()
            .WithMessage(
                "Request body is required. Ensure the JSON is valid (unresolved Postman variables produce invalid JSON).");

        When(x => x.Dto is not null, () =>
        {
            RuleFor(x => x.Dto.CustomerNumber).NotEmpty().MaximumLength(30).When(x => x.Dto.CustomerNumber is not null);
            RuleFor(x => x.Dto.CustomerNumber)
                .Must(code => string.Equals(code!, code!.Trim().ToUpperInvariant(), StringComparison.Ordinal))
                .WithMessage("CustomerNumber must be uppercase.")
                .When(x => x.Dto.CustomerNumber is not null);
            RuleFor(x => x.Dto.CustomerNumber)
                .MustAsync(async (command, number, cancellationToken) =>
                    !await readRepository.ExistsByCustomerNumberAsync(number!, command.Id, cancellationToken))
                .WithMessage("A customer with this number already exists.")
                .When(x => x.Dto.CustomerNumber is not null);
            RuleFor(x => x.Dto.Name).NotEmpty().MaximumLength(200).When(x => x.Dto.Name is not null);
            RuleFor(x => x.Dto.DisplayName).NotEmpty().MaximumLength(200).When(x => x.Dto.DisplayName is not null);
            RuleFor(x => x.Dto.AddressLine1).MaximumLength(200).When(x => x.Dto.AddressLine1 is not null);
            RuleFor(x => x.Dto.AddressLine2).MaximumLength(200).When(x => x.Dto.AddressLine2 is not null);
            RuleFor(x => x.Dto.AddressLine3).MaximumLength(200).When(x => x.Dto.AddressLine3 is not null);
            RuleFor(x => x.Dto.AddressLine4).MaximumLength(200).When(x => x.Dto.AddressLine4 is not null);
            RuleFor(x => x.Dto.City).MaximumLength(100).When(x => x.Dto.City is not null);
            RuleFor(x => x.Dto.State).MaximumLength(100).When(x => x.Dto.State is not null);
            RuleFor(x => x.Dto.County).MaximumLength(100).When(x => x.Dto.County is not null);
            RuleFor(x => x.Dto.Country).MaximumLength(100).When(x => x.Dto.Country is not null);
            RuleFor(x => x.Dto.PostalCode).MaximumLength(20).When(x => x.Dto.PostalCode is not null);
            RuleFor(x => x.Dto.FormattedAddress).MaximumLength(1000).When(x => x.Dto.FormattedAddress is not null);
            RuleFor(x => x.Dto.PlaceId).MaximumLength(500).When(x => x.Dto.PlaceId is not null);
            RuleFor(x => x.Dto.TaxExemptNumber).MaximumLength(100).When(x => x.Dto.TaxExemptNumber is not null);
            RuleFor(x => x.Dto.CustomerAccountNumber).MaximumLength(100).When(x => x.Dto.CustomerAccountNumber is not null);
            RuleFor(x => x.Dto.ExternalEntityId).MaximumLength(200).When(x => x.Dto.ExternalEntityId is not null);
            RuleFor(x => x.Dto.ExternalVersion).MaximumLength(100).When(x => x.Dto.ExternalVersion is not null);
        });
    }
}
