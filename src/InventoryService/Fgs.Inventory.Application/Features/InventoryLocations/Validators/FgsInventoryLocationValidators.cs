using Fgs.Inventory.Application.Abstractions.InventoryLocations;
using Fgs.Inventory.Application.Features.InventoryLocations.Commands.CreateFgsInventoryLocation;
using Fgs.Inventory.Application.Features.InventoryLocations.Commands.PatchFgsInventoryLocation;
using Fgs.Inventory.Application.Features.InventoryLocations.Commands.UpdateFgsInventoryLocation;
using Fgs.Inventory.Domain.Entities;
using FluentValidation;

namespace Fgs.Inventory.Application.Features.InventoryLocations.Validators;

public sealed class CreateFgsInventoryLocationCommandValidator : AbstractValidator<CreateFgsInventoryLocationCommand>
{
    private static readonly HashSet<string> AllowedTypes = new(StringComparer.OrdinalIgnoreCase)
    {
        InventoryLocationTypes.Warehouse,
        InventoryLocationTypes.Truck,
        InventoryLocationTypes.Trailer,
        InventoryLocationTypes.JobSite,
        InventoryLocationTypes.Consignment,
        InventoryLocationTypes.Vendor
    };

    public CreateFgsInventoryLocationCommandValidator(IFgsInventoryLocationReadRepository readRepository)
    {
        RuleFor(x => x.Dto.InventoryLocationCode).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Dto.InventoryLocationCode)
            .Must(code => string.Equals(code, code.Trim().ToUpperInvariant(), StringComparison.Ordinal))
            .WithMessage("InventoryLocationCode must be uppercase.");
        RuleFor(x => x.Dto.InventoryLocationCode)
            .MustAsync(async (command, code, cancellationToken) =>
                !await readRepository.ExistsByInventoryLocationCodeAsync(code, null, cancellationToken))
            .WithMessage("An inventory location with this code already exists.");
        RuleFor(x => x.Dto.Name).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Dto.InventoryLocationType).NotEmpty().MaximumLength(30);
        RuleFor(x => x.Dto.InventoryLocationType)
            .Must(type => AllowedTypes.Contains(type))
            .WithMessage("InventoryLocationType must be a valid inventory location type.");
        RuleFor(x => x.Dto.Address1).MaximumLength(200);
        RuleFor(x => x.Dto.Address2).MaximumLength(200);
        RuleFor(x => x.Dto.City).MaximumLength(100);
        RuleFor(x => x.Dto.StateProvince).MaximumLength(100);
        RuleFor(x => x.Dto.PostalCode).MaximumLength(20);
        RuleFor(x => x.Dto.Country).MaximumLength(100);
        RuleFor(x => x.Dto.ContactName).MaximumLength(150);
        RuleFor(x => x.Dto.PhoneNumber).MaximumLength(50);
        RuleFor(x => x.Dto.Email).MaximumLength(255);
    }
}

public sealed class UpdateFgsInventoryLocationCommandValidator : AbstractValidator<UpdateFgsInventoryLocationCommand>
{
    private static readonly HashSet<string> AllowedTypes = new(StringComparer.OrdinalIgnoreCase)
    {
        InventoryLocationTypes.Warehouse,
        InventoryLocationTypes.Truck,
        InventoryLocationTypes.Trailer,
        InventoryLocationTypes.JobSite,
        InventoryLocationTypes.Consignment,
        InventoryLocationTypes.Vendor
    };

    public UpdateFgsInventoryLocationCommandValidator(IFgsInventoryLocationReadRepository readRepository)
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Dto.InventoryLocationCode).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Dto.InventoryLocationCode)
            .Must(code => string.Equals(code, code.Trim().ToUpperInvariant(), StringComparison.Ordinal))
            .WithMessage("InventoryLocationCode must be uppercase.");
        RuleFor(x => x.Dto.InventoryLocationCode)
            .MustAsync(async (command, code, cancellationToken) =>
                !await readRepository.ExistsByInventoryLocationCodeAsync(code, command.Id, cancellationToken))
            .WithMessage("An inventory location with this code already exists.");
        RuleFor(x => x.Dto.Name).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Dto.InventoryLocationType).NotEmpty().MaximumLength(30);
        RuleFor(x => x.Dto.InventoryLocationType)
            .Must(type => AllowedTypes.Contains(type))
            .WithMessage("InventoryLocationType must be a valid inventory location type.");
        RuleFor(x => x.Dto.Address1).MaximumLength(200);
        RuleFor(x => x.Dto.Address2).MaximumLength(200);
        RuleFor(x => x.Dto.City).MaximumLength(100);
        RuleFor(x => x.Dto.StateProvince).MaximumLength(100);
        RuleFor(x => x.Dto.PostalCode).MaximumLength(20);
        RuleFor(x => x.Dto.Country).MaximumLength(100);
        RuleFor(x => x.Dto.ContactName).MaximumLength(150);
        RuleFor(x => x.Dto.PhoneNumber).MaximumLength(50);
        RuleFor(x => x.Dto.Email).MaximumLength(255);
    }
}

public sealed class PatchFgsInventoryLocationCommandValidator : AbstractValidator<PatchFgsInventoryLocationCommand>
{
    private static readonly HashSet<string> AllowedTypes = new(StringComparer.OrdinalIgnoreCase)
    {
        InventoryLocationTypes.Warehouse,
        InventoryLocationTypes.Truck,
        InventoryLocationTypes.Trailer,
        InventoryLocationTypes.JobSite,
        InventoryLocationTypes.Consignment,
        InventoryLocationTypes.Vendor
    };

    public PatchFgsInventoryLocationCommandValidator(IFgsInventoryLocationReadRepository readRepository)
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Dto.InventoryLocationCode).NotEmpty().MaximumLength(50).When(x => x.Dto.InventoryLocationCode is not null);
        RuleFor(x => x.Dto.InventoryLocationCode)
            .Must(code => string.Equals(code!, code!.Trim().ToUpperInvariant(), StringComparison.Ordinal))
            .WithMessage("InventoryLocationCode must be uppercase.")
            .When(x => x.Dto.InventoryLocationCode is not null);
        RuleFor(x => x.Dto.InventoryLocationCode)
            .MustAsync(async (command, code, cancellationToken) =>
                !await readRepository.ExistsByInventoryLocationCodeAsync(code!, command.Id, cancellationToken))
            .WithMessage("An inventory location with this code already exists.")
            .When(x => x.Dto.InventoryLocationCode is not null);
        RuleFor(x => x.Dto.Name).NotEmpty().MaximumLength(200).When(x => x.Dto.Name is not null);
        RuleFor(x => x.Dto.InventoryLocationType).NotEmpty().MaximumLength(30).When(x => x.Dto.InventoryLocationType is not null);
        RuleFor(x => x.Dto.InventoryLocationType)
            .Must(type => AllowedTypes.Contains(type!))
            .WithMessage("InventoryLocationType must be a valid inventory location type.")
            .When(x => x.Dto.InventoryLocationType is not null);
        RuleFor(x => x.Dto.Address1).MaximumLength(200).When(x => x.Dto.Address1 is not null);
        RuleFor(x => x.Dto.Address2).MaximumLength(200).When(x => x.Dto.Address2 is not null);
        RuleFor(x => x.Dto.City).MaximumLength(100).When(x => x.Dto.City is not null);
        RuleFor(x => x.Dto.StateProvince).MaximumLength(100).When(x => x.Dto.StateProvince is not null);
        RuleFor(x => x.Dto.PostalCode).MaximumLength(20).When(x => x.Dto.PostalCode is not null);
        RuleFor(x => x.Dto.Country).MaximumLength(100).When(x => x.Dto.Country is not null);
        RuleFor(x => x.Dto.ContactName).MaximumLength(150).When(x => x.Dto.ContactName is not null);
        RuleFor(x => x.Dto.PhoneNumber).MaximumLength(50).When(x => x.Dto.PhoneNumber is not null);
        RuleFor(x => x.Dto.Email).MaximumLength(255).When(x => x.Dto.Email is not null);
    }
}
