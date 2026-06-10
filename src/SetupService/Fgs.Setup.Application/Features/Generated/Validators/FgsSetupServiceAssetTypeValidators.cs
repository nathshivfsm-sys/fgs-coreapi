using FluentValidation;
using Fgs.Foundation.CatalogCrud.Abstractions;
using Fgs.Foundation.CatalogCrud.Commands;
using Fgs.Foundation.CatalogCrud.Validation;
using Fgs.Setup.Application.Features.Generated.Descriptors;
using Fgs.Setup.Application.Features.Generated.Dtos;

namespace Fgs.Setup.Application.Features.Generated.Validators;

public sealed class CreateFgsSetupServiceAssetTypeCommandValidator : AbstractValidator<CreateCatalogEntityCommand<FgsSetupServiceAssetTypeCreateDto, FgsSetupServiceAssetTypeDetailDto>>
{
    public CreateFgsSetupServiceAssetTypeCommandValidator(IEntityReadRepository readRepository)
    {
        var descriptor = FgsSetupServiceAssetTypeDescriptor.Create();
        RuleFor(x => x.EntityKey).Equal(descriptor.Key);
        CatalogEntityValidatorFactory.ApplyCreateRules(this, descriptor, readRepository, x => x.Payload);
    }
}

public sealed class UpdateFgsSetupServiceAssetTypeCommandValidator : AbstractValidator<UpdateCatalogEntityCommand<FgsSetupServiceAssetTypeUpdateDto, FgsSetupServiceAssetTypeDetailDto>>
{
    public UpdateFgsSetupServiceAssetTypeCommandValidator(IEntityReadRepository readRepository)
    {
        var descriptor = FgsSetupServiceAssetTypeDescriptor.Create();
        RuleFor(x => x.EntityKey).Equal(descriptor.Key);
        RuleFor(x => x.Id).NotEmpty();
        CatalogEntityValidatorFactory.ApplyUpdateRules(this, descriptor, readRepository, x => x.Payload, x => x.Id);
    }
}

public sealed class PatchFgsSetupServiceAssetTypeCommandValidator : AbstractValidator<PatchCatalogEntityCommand<FgsSetupServiceAssetTypePatchDto, FgsSetupServiceAssetTypeDetailDto>>
{
    public PatchFgsSetupServiceAssetTypeCommandValidator()
    {
        var descriptor = FgsSetupServiceAssetTypeDescriptor.Create();
        RuleFor(x => x.EntityKey).Equal(descriptor.Key);
        RuleFor(x => x.Id).NotEmpty();
        CatalogEntityValidatorFactory.ApplyPatchRules(this, descriptor, x => x.Payload);
    }
}
