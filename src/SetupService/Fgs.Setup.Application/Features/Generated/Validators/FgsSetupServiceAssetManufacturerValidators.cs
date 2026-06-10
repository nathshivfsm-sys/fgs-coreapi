using FluentValidation;
using Fgs.Foundation.CatalogCrud.Abstractions;
using Fgs.Foundation.CatalogCrud.Commands;
using Fgs.Foundation.CatalogCrud.Validation;
using Fgs.Setup.Application.Features.Generated.Descriptors;
using Fgs.Setup.Application.Features.Generated.Dtos;

namespace Fgs.Setup.Application.Features.Generated.Validators;

public sealed class CreateFgsSetupServiceAssetManufacturerCommandValidator : AbstractValidator<CreateCatalogEntityCommand<FgsSetupServiceAssetManufacturerCreateDto, FgsSetupServiceAssetManufacturerDetailDto>>
{
    public CreateFgsSetupServiceAssetManufacturerCommandValidator(IEntityReadRepository readRepository)
    {
        var descriptor = FgsSetupServiceAssetManufacturerDescriptor.Create();
        RuleFor(x => x.EntityKey).Equal(descriptor.Key);
        CatalogEntityValidatorFactory.ApplyCreateRules(this, descriptor, readRepository, x => x.Payload);
    }
}

public sealed class UpdateFgsSetupServiceAssetManufacturerCommandValidator : AbstractValidator<UpdateCatalogEntityCommand<FgsSetupServiceAssetManufacturerUpdateDto, FgsSetupServiceAssetManufacturerDetailDto>>
{
    public UpdateFgsSetupServiceAssetManufacturerCommandValidator(IEntityReadRepository readRepository)
    {
        var descriptor = FgsSetupServiceAssetManufacturerDescriptor.Create();
        RuleFor(x => x.EntityKey).Equal(descriptor.Key);
        RuleFor(x => x.Id).NotEmpty();
        CatalogEntityValidatorFactory.ApplyUpdateRules(this, descriptor, readRepository, x => x.Payload, x => x.Id);
    }
}

public sealed class PatchFgsSetupServiceAssetManufacturerCommandValidator : AbstractValidator<PatchCatalogEntityCommand<FgsSetupServiceAssetManufacturerPatchDto, FgsSetupServiceAssetManufacturerDetailDto>>
{
    public PatchFgsSetupServiceAssetManufacturerCommandValidator()
    {
        var descriptor = FgsSetupServiceAssetManufacturerDescriptor.Create();
        RuleFor(x => x.EntityKey).Equal(descriptor.Key);
        RuleFor(x => x.Id).NotEmpty();
        CatalogEntityValidatorFactory.ApplyPatchRules(this, descriptor, x => x.Payload);
    }
}
