using Fgs.Setup.Application.Features.Vehicles.Commands.CreateFgsVehicle;
using Fgs.Setup.Application.Features.Vehicles.Commands.PatchFgsVehicle;
using Fgs.Setup.Application.Features.Vehicles.Commands.UpdateFgsVehicle;
using FluentValidation;

namespace Fgs.Setup.Application.Features.Vehicles.Validators;

public sealed class CreateFgsVehicleCommandValidator : AbstractValidator<CreateFgsVehicleCommand>
{
    public CreateFgsVehicleCommandValidator()
    {
        RuleFor(x => x.Dto.OwnershipType).NotEmpty();
        RuleFor(x => x.Dto.OwnershipType).MaximumLength(20);
        RuleFor(x => x.Dto.OwnershipCompany).MaximumLength(200);

        RuleFor(x => x.Dto.Make).MaximumLength(100);
        RuleFor(x => x.Dto.Model).MaximumLength(100);
        RuleFor(x => x.Dto.Color).MaximumLength(50);
        RuleFor(x => x.Dto.VIN).NotEmpty();
        RuleFor(x => x.Dto.VIN).MaximumLength(50);
        RuleFor(x => x.Dto.LicensePlate).MaximumLength(50);
        RuleFor(x => x.Dto.LicensePlateState).MaximumLength(50);

        RuleFor(x => x.Dto.PurchasedFrom).MaximumLength(200);
    }
}

public sealed class UpdateFgsVehicleCommandValidator : AbstractValidator<UpdateFgsVehicleCommand>
{
    public UpdateFgsVehicleCommandValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Dto.OwnershipType).NotEmpty();
        RuleFor(x => x.Dto.OwnershipType).MaximumLength(20);
        RuleFor(x => x.Dto.OwnershipCompany).MaximumLength(200);

        RuleFor(x => x.Dto.Make).MaximumLength(100);
        RuleFor(x => x.Dto.Model).MaximumLength(100);
        RuleFor(x => x.Dto.Color).MaximumLength(50);
        RuleFor(x => x.Dto.VIN).NotEmpty();
        RuleFor(x => x.Dto.VIN).MaximumLength(50);
        RuleFor(x => x.Dto.LicensePlate).MaximumLength(50);
        RuleFor(x => x.Dto.LicensePlateState).MaximumLength(50);

        RuleFor(x => x.Dto.PurchasedFrom).MaximumLength(200);
    }
}

public sealed class PatchFgsVehicleCommandValidator : AbstractValidator<PatchFgsVehicleCommand>
{
    public PatchFgsVehicleCommandValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Dto.OwnershipType).NotEmpty().When(x => x.Dto.OwnershipType is not null);
        RuleFor(x => x.Dto.OwnershipType).MaximumLength(20).When(x => x.Dto.OwnershipType is not null);
        RuleFor(x => x.Dto.OwnershipCompany).MaximumLength(200).When(x => x.Dto.OwnershipCompany is not null);

        RuleFor(x => x.Dto.Make).MaximumLength(100).When(x => x.Dto.Make is not null);
        RuleFor(x => x.Dto.Model).MaximumLength(100).When(x => x.Dto.Model is not null);
        RuleFor(x => x.Dto.Color).MaximumLength(50).When(x => x.Dto.Color is not null);
        RuleFor(x => x.Dto.VIN).NotEmpty().When(x => x.Dto.VIN is not null);
        RuleFor(x => x.Dto.VIN).MaximumLength(50).When(x => x.Dto.VIN is not null);
        RuleFor(x => x.Dto.LicensePlate).MaximumLength(50).When(x => x.Dto.LicensePlate is not null);
        RuleFor(x => x.Dto.LicensePlateState).MaximumLength(50).When(x => x.Dto.LicensePlateState is not null);

        RuleFor(x => x.Dto.PurchasedFrom).MaximumLength(200).When(x => x.Dto.PurchasedFrom is not null);
    }
}
