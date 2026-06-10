using FluentValidation;
using Fgs.Foundation.CatalogCrud.Abstractions;
using Fgs.Foundation.CatalogCrud.Commands;
using Fgs.Foundation.CatalogCrud.Validation;
using Fgs.Setup.Application.Features.Generated.Descriptors;
using Fgs.Setup.Application.Features.Generated.Dtos;

namespace Fgs.Setup.Application.Features.Generated.Validators;

public sealed class CreateFgsVehicleCommandValidator : AbstractValidator<CreateCatalogEntityCommand<FgsVehicleCreateDto, FgsVehicleDetailDto>>
{
    public CreateFgsVehicleCommandValidator(IEntityReadRepository readRepository)
    {
        var descriptor = FgsVehicleDescriptor.Create();
        RuleFor(x => x.EntityKey).Equal(descriptor.Key);
        CatalogEntityValidatorFactory.ApplyCreateRules(this, descriptor, readRepository, x => x.Payload);
    }
}

public sealed class UpdateFgsVehicleCommandValidator : AbstractValidator<UpdateCatalogEntityCommand<FgsVehicleUpdateDto, FgsVehicleDetailDto>>
{
    public UpdateFgsVehicleCommandValidator(IEntityReadRepository readRepository)
    {
        var descriptor = FgsVehicleDescriptor.Create();
        RuleFor(x => x.EntityKey).Equal(descriptor.Key);
        RuleFor(x => x.Id).NotEmpty();
        CatalogEntityValidatorFactory.ApplyUpdateRules(this, descriptor, readRepository, x => x.Payload, x => x.Id);
    }
}

public sealed class PatchFgsVehicleCommandValidator : AbstractValidator<PatchCatalogEntityCommand<FgsVehiclePatchDto, FgsVehicleDetailDto>>
{
    public PatchFgsVehicleCommandValidator()
    {
        var descriptor = FgsVehicleDescriptor.Create();
        RuleFor(x => x.EntityKey).Equal(descriptor.Key);
        RuleFor(x => x.Id).NotEmpty();
        CatalogEntityValidatorFactory.ApplyPatchRules(this, descriptor, x => x.Payload);
    }
}
