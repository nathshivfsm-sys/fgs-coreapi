using Fgs.Setup.Application.Abstractions.VehicleMaintenances;
using Fgs.Setup.Application.Features.VehicleMaintenances.Commands.CreateFgsVehicleMaintenance;
using Fgs.Setup.Application.Features.VehicleMaintenances.Commands.PatchFgsVehicleMaintenance;
using Fgs.Setup.Application.Features.VehicleMaintenances.Commands.UpdateFgsVehicleMaintenance;
using FluentValidation;

namespace Fgs.Setup.Application.Features.VehicleMaintenances.Validators;

public sealed class CreateFgsVehicleMaintenanceCommandValidator : AbstractValidator<CreateFgsVehicleMaintenanceCommand>
{
    public CreateFgsVehicleMaintenanceCommandValidator(IFgsVehicleMaintenanceReadRepository readRepository)
    {
        RuleFor(x => x.Dto.VehicleId).MustAsync(async (command, value, cancellationToken) =>
                await readRepository.ExistsVehicleIdAsync(value, cancellationToken))
            .WithMessage("The specified vehicle was not found.");
        RuleFor(x => x.Dto.VehicleMaintenanceTypeId).MustAsync(async (command, value, cancellationToken) =>
                await readRepository.ExistsGloVehicleMaintenanceTypeIdAsync(value, cancellationToken))
            .WithMessage("The specified vehicle maintenance type was not found.");


        RuleFor(x => x.Dto.ServiceProvider).MaximumLength(200);
        RuleFor(x => x.Dto.InvoiceNumber).MaximumLength(100);




        RuleFor(x => x.Dto.Description).MaximumLength(500);
        RuleFor(x => x.Dto.VehicleId).MustAsync(async (command, value, cancellationToken) =>
                await readRepository.ExistsVehicleIdAsync(value, cancellationToken))
            .WithMessage("The specified vehicle was not found.");
        RuleFor(x => x.Dto.VehicleMaintenanceTypeId).MustAsync(async (command, value, cancellationToken) =>
                await readRepository.ExistsGloVehicleMaintenanceTypeIdAsync(value, cancellationToken))
            .WithMessage("The specified vehicle maintenance type was not found.");


        RuleFor(x => x.Dto.ServiceProvider).MaximumLength(200);
        RuleFor(x => x.Dto.InvoiceNumber).MaximumLength(100);




        RuleFor(x => x.Dto.Description).MaximumLength(500);

    }
}

public sealed class UpdateFgsVehicleMaintenanceCommandValidator : AbstractValidator<UpdateFgsVehicleMaintenanceCommand>
{
    public UpdateFgsVehicleMaintenanceCommandValidator(IFgsVehicleMaintenanceReadRepository readRepository)
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Dto.VehicleId).MustAsync(async (command, value, cancellationToken) =>
                await readRepository.ExistsVehicleIdAsync(value, cancellationToken))
            .WithMessage("The specified vehicle was not found.");
        RuleFor(x => x.Dto.VehicleMaintenanceTypeId).MustAsync(async (command, value, cancellationToken) =>
                await readRepository.ExistsGloVehicleMaintenanceTypeIdAsync(value, cancellationToken))
            .WithMessage("The specified vehicle maintenance type was not found.");


        RuleFor(x => x.Dto.ServiceProvider).MaximumLength(200);
        RuleFor(x => x.Dto.InvoiceNumber).MaximumLength(100);




        RuleFor(x => x.Dto.Description).MaximumLength(500);

    }
}

public sealed class PatchFgsVehicleMaintenanceCommandValidator : AbstractValidator<PatchFgsVehicleMaintenanceCommand>
{
    public PatchFgsVehicleMaintenanceCommandValidator(IFgsVehicleMaintenanceReadRepository readRepository)
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Dto.VehicleId).MustAsync(async (command, value, cancellationToken) =>
                !value.HasValue || await readRepository.ExistsVehicleIdAsync(value.Value, cancellationToken))
            .WithMessage("The specified vehicle was not found.").When(x => x.Dto.VehicleId.HasValue);
        RuleFor(x => x.Dto.VehicleMaintenanceTypeId).MustAsync(async (command, value, cancellationToken) =>
                !value.HasValue || await readRepository.ExistsGloVehicleMaintenanceTypeIdAsync(value.Value, cancellationToken))
            .WithMessage("The specified vehicle maintenance type was not found.").When(x => x.Dto.VehicleMaintenanceTypeId.HasValue);


        RuleFor(x => x.Dto.ServiceProvider).MaximumLength(200).When(x => x.Dto.ServiceProvider is not null);
        RuleFor(x => x.Dto.InvoiceNumber).MaximumLength(100).When(x => x.Dto.InvoiceNumber is not null);




        RuleFor(x => x.Dto.Description).MaximumLength(500).When(x => x.Dto.Description is not null);

    }
}
