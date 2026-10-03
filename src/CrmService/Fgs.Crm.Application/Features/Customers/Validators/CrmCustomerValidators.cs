using System.Linq.Expressions;
using Fgs.Crm.Application.Abstractions.Customers;
using Fgs.Crm.Application.Features.Customers.Commands.AddCrmCustomerServiceLocation;
using Fgs.Crm.Application.Features.Customers.Commands.CreateCrmCustomer;
using Fgs.Crm.Application.Features.Customers.Commands.PatchCrmCustomer;
using Fgs.Crm.Application.Features.Customers.Commands.UpdateCrmCustomer;
using Fgs.Crm.Application.Features.Customers.Dtos;
using Fgs.Foundation.Validation;
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
            RuleFor(x => x.Dto.DefaultPaymentTermId).NotNull().GreaterThan(0);
            ApplyAddressRules(this);
            RuleFor(x => x.Dto.TaxExemptNumber).MaximumLength(100);
            RuleFor(x => x.Dto.CustomerAccountNumber).MaximumLength(100);
            RuleFor(x => x.Dto.ExternalEntityId).MaximumLength(200);
            RuleFor(x => x.Dto.ExternalVersion).MaximumLength(100);
            RuleFor(x => x.Dto.PlaceId).MaximumLength(500);
            RuleFor(x => x.Dto.FormattedAddress).MaximumLength(1000);
            RuleFor(x => x.Dto.CreationOption).IsInEnum();
            CrmCustomerFieldValidation.ApplyWebsiteRule(this, x => x.Dto.Website);
            ApplyPrimaryContactRules(this);
            RuleFor(x => x.Dto.TagIds)
                .Must(CrmCustomerFieldValidation.TagIdsAreDistinct)
                .WithMessage("TagIds must not contain duplicates.");

            When(x => x.Dto.CreationOption == CrmCustomerCreationOption.BillToOnly, () =>
            {
                RuleFor(x => x.Dto.ServiceLocation)
                    .Null()
                    .WithMessage("ServiceLocation must be omitted when CreationOption is BillToOnly.");
            });

            When(x => x.Dto.CreationOption == CrmCustomerCreationOption.BillToAndServiceLocation, () =>
            {
                RuleFor(x => x.Dto.ServiceLocation)
                    .NotNull()
                    .WithMessage("ServiceLocation is required when CreationOption is BillToAndServiceLocation.");

                When(x => x.Dto.ServiceLocation is not null, () =>
                {
                    RuleFor(x => x.Dto.ServiceLocation!)
                        .SetValidator(new CrmServiceLocationCreateDtoValidator());
                });
            });
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

        validator.When(
            x => CrmCustomerFieldValidation.HasAddressInput(
                x.Dto.AddressLine1,
                x.Dto.AddressLine2,
                x.Dto.AddressLine3,
                x.Dto.AddressLine4,
                x.Dto.City,
                x.Dto.State,
                x.Dto.County,
                x.Dto.Country,
                x.Dto.PostalCode,
                x.Dto.FormattedAddress),
            () =>
            {
                validator.RuleFor(x => x.Dto.AddressLine1).NotEmpty();
                validator.RuleFor(x => x.Dto.PostalCode).NotEmpty();
                validator.RuleFor(x => x.Dto.State).NotEmpty();
                validator.RuleFor(x => x.Dto.Country).NotEmpty();
            });
    }

    private static void ApplyPrimaryContactRules(AbstractValidator<CreateCrmCustomerCommand> validator)
    {
        validator.RuleFor(x => x.Dto.PrimaryContactName)
            .MaximumLength(200)
            .When(x => !string.IsNullOrWhiteSpace(x.Dto.PrimaryContactName));

        validator.RuleFor(x => x.Dto.PrimaryContactName)
            .NotEmpty()
            .WithMessage("Primary contact name is required when email or phone is provided.")
            .When(x => !string.IsNullOrWhiteSpace(x.Dto.PrimaryContactEmail)
                || !string.IsNullOrWhiteSpace(x.Dto.PrimaryContactPhone));

        validator.RuleFor(x => x.Dto.PrimaryContactEmail)
            .MaximumLength(1000)
            .MustBeValidEmailAddress()
            .When(x => !string.IsNullOrWhiteSpace(x.Dto.PrimaryContactEmail));

        validator.RuleFor(x => x.Dto.PrimaryContactPhone)
            .MaximumLength(50)
            .Matches(@"^\+?[\d\s().-]+$")
            .WithMessage("Phone must contain only digits, spaces, and phone punctuation.")
            .When(x => !string.IsNullOrWhiteSpace(x.Dto.PrimaryContactPhone));
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
            CrmCustomerFieldValidation.ApplyWebsiteRule(this, x => x.Dto.Website);
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
            CrmCustomerFieldValidation.ApplyWebsiteRule(this, x => x.Dto.Website);
        });
    }
}

internal sealed class CrmServiceLocationCreateDtoValidator : AbstractValidator<CrmServiceLocationCreateDto>
{
    public CrmServiceLocationCreateDtoValidator()
    {
        RuleFor(x => x.ServiceLocationType).IsInEnum();
        RuleFor(x => x.CustomerType)
            .IsInEnum()
            .When(x => x.CustomerType.HasValue);
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
        RuleFor(x => x.DisplayName).NotEmpty().MaximumLength(200);
        RuleFor(x => x.AddressLine1).MaximumLength(200);
        RuleFor(x => x.AddressLine2).MaximumLength(200);
        RuleFor(x => x.AddressLine3).MaximumLength(200);
        RuleFor(x => x.AddressLine4).MaximumLength(200);
        RuleFor(x => x.City).MaximumLength(100);
        RuleFor(x => x.State).MaximumLength(100);
        RuleFor(x => x.County).MaximumLength(100);
        RuleFor(x => x.Country).MaximumLength(100);
        RuleFor(x => x.PostalCode).MaximumLength(20);
        RuleFor(x => x.FormattedAddress).MaximumLength(1000);
        RuleFor(x => x.PlaceId).MaximumLength(500);

        When(
            x => CrmCustomerFieldValidation.HasAddressInput(
                x.AddressLine1,
                x.AddressLine2,
                x.AddressLine3,
                x.AddressLine4,
                x.City,
                x.State,
                x.County,
                x.Country,
                x.PostalCode,
                x.FormattedAddress),
            () =>
            {
                RuleFor(x => x.AddressLine1).NotEmpty();
                RuleFor(x => x.PostalCode).NotEmpty();
                RuleFor(x => x.State).NotEmpty();
                RuleFor(x => x.Country).NotEmpty();
            });

        RuleFor(x => x.PrimaryContactName)
            .MaximumLength(200)
            .When(x => !string.IsNullOrWhiteSpace(x.PrimaryContactName));

        RuleFor(x => x.PrimaryContactName)
            .NotEmpty()
            .WithMessage("Primary contact name is required when email or phone is provided.")
            .When(x => !string.IsNullOrWhiteSpace(x.PrimaryContactEmail)
                || !string.IsNullOrWhiteSpace(x.PrimaryContactPhone));

        RuleFor(x => x.PrimaryContactEmail)
            .MaximumLength(1000)
            .MustBeValidEmailAddress()
            .When(x => !string.IsNullOrWhiteSpace(x.PrimaryContactEmail));

        RuleFor(x => x.PrimaryContactPhone)
            .MaximumLength(50)
            .Matches(@"^\+?[\d\s().-]+$")
            .WithMessage("Phone must contain only digits, spaces, and phone punctuation.")
            .When(x => !string.IsNullOrWhiteSpace(x.PrimaryContactPhone));

        RuleFor(x => x.TagIds)
            .Must(CrmCustomerFieldValidation.TagIdsAreDistinct)
            .WithMessage("TagIds must not contain duplicates.");
    }
}

public sealed class AddCrmCustomerServiceLocationCommandValidator : AbstractValidator<AddCrmCustomerServiceLocationCommand>
{
    public AddCrmCustomerServiceLocationCommandValidator()
    {
        RuleFor(x => x.CustomerId).GreaterThan(0);
        RuleFor(x => x.Dto)
            .NotNull()
            .WithMessage(
                "Request body is required. Ensure the JSON is valid (unresolved Postman variables produce invalid JSON).");

        When(x => x.Dto is not null, () =>
        {
            RuleFor(x => x.Dto).SetValidator(new CrmServiceLocationCreateDtoValidator());
        });
    }
}

internal static class CrmCustomerFieldValidation
{
    public static void ApplyWebsiteRule<T>(
        AbstractValidator<T> validator,
        Expression<Func<T, string?>> website)
    {
        var read = website.Compile();
        validator.RuleFor(website)
            .MaximumLength(500)
            .Must(url => IsAbsoluteHttpUrl(url))
            .WithMessage("Website must be an absolute HTTP or HTTPS URL.")
            .When(model => !string.IsNullOrWhiteSpace(read(model)));
    }

    private static bool IsAbsoluteHttpUrl(string? url) =>
        !string.IsNullOrWhiteSpace(url)
        && Uri.TryCreate(url.Trim(), UriKind.Absolute, out var uri)
        && (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps);

    public static bool TagIdsAreDistinct(IReadOnlyList<long>? tagIds)
    {
        if (tagIds is not { Count: > 0 })
        {
            return true;
        }

        return tagIds.Distinct().Count() == tagIds.Count;
    }

    public static bool HasAddressInput(
        string? addressLine1,
        string? addressLine2,
        string? addressLine3,
        string? addressLine4,
        string? city,
        string? state,
        string? county,
        string? country,
        string? postalCode,
        string? formattedAddress) =>
        HasText(addressLine1)
        || HasText(addressLine2)
        || HasText(addressLine3)
        || HasText(addressLine4)
        || HasText(city)
        || HasText(state)
        || HasText(county)
        || HasText(country)
        || HasText(postalCode)
        || HasText(formattedAddress);

    private static bool HasText(string? value) => !string.IsNullOrWhiteSpace(value);
}
