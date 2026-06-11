using FluentValidation;
using Fgs.Foundation.CatalogCrud.Abstractions;
using Fgs.Foundation.CatalogCrud.Commands;
using Fgs.Foundation.CatalogCrud.Validation;
using Fgs.Setup.Application.Features.Generated.Descriptors;
using Fgs.Setup.Application.Features.Generated.Dtos;

namespace Fgs.Setup.Application.Features.Generated.Validators;

public sealed class CreateFgsVehicleMaintenanceCommandValidator : AbstractValidator<CreateCatalogEntityCommand<FgsVehicleMaintenanceCreateDto, FgsVehicleMaintenanceDetailDto>>
{
    public CreateFgsVehicleMaintenanceCommandValidator(IEntityReadRepository readRepository)
    {
        var descriptor = FgsVehicleMaintenanceDescriptor.Create();
        RuleFor(x => x.EntityKey).Equal(descriptor.Key);
        CatalogEntityValidatorFactory.ApplyCreateRules(this, descriptor, readRepository, x => x.Payload);
    }
}

public sealed class UpdateFgsVehicleMaintenanceCommandValidator : AbstractValidator<UpdateCatalogEntityCommand<FgsVehicleMaintenanceUpdateDto, FgsVehicleMaintenanceDetailDto>>
{
    public UpdateFgsVehicleMaintenanceCommandValidator(IEntityReadRepository readRepository)
    {
        var descriptor = FgsVehicleMaintenanceDescriptor.Create();
        RuleFor(x => x.EntityKey).Equal(descriptor.Key);
        RuleFor(x => x.Id).NotEmpty();
        CatalogEntityValidatorFactory.ApplyUpdateRules(this, descriptor, readRepository, x => x.Payload, x => x.Id);
    }
}

public sealed class PatchFgsVehicleMaintenanceCommandValidator : AbstractValidator<PatchCatalogEntityCommand<FgsVehicleMaintenancePatchDto, FgsVehicleMaintenanceDetailDto>>
{
    public PatchFgsVehicleMaintenanceCommandValidator()
    {
        var descriptor = FgsVehicleMaintenanceDescriptor.Create();
        RuleFor(x => x.EntityKey).Equal(descriptor.Key);
        RuleFor(x => x.Id).NotEmpty();
        CatalogEntityValidatorFactory.ApplyPatchRules(this, descriptor, x => x.Payload);
    }
}
