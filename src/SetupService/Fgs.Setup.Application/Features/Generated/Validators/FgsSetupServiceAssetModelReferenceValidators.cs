using FluentValidation;
using Fgs.Foundation.CatalogCrud.Abstractions;
using Fgs.Foundation.CatalogCrud.Commands;
using Fgs.Foundation.CatalogCrud.Validation;
using Fgs.Setup.Application.Features.Generated.Descriptors;
using Fgs.Setup.Application.Features.Generated.Dtos;

namespace Fgs.Setup.Application.Features.Generated.Validators;

public sealed class CreateFgsSetupServiceAssetModelReferenceCommandValidator : AbstractValidator<CreateCatalogEntityCommand<FgsSetupServiceAssetModelReferenceCreateDto, FgsSetupServiceAssetModelReferenceDetailDto>>
{
    public CreateFgsSetupServiceAssetModelReferenceCommandValidator(IEntityReadRepository readRepository)
    {
        var descriptor = FgsSetupServiceAssetModelReferenceDescriptor.Create();
        RuleFor(x => x.EntityKey).Equal(descriptor.Key);
        CatalogEntityValidatorFactory.ApplyCreateRules(this, descriptor, readRepository, x => x.Payload);
    }
}

public sealed class UpdateFgsSetupServiceAssetModelReferenceCommandValidator : AbstractValidator<UpdateCatalogEntityCommand<FgsSetupServiceAssetModelReferenceUpdateDto, FgsSetupServiceAssetModelReferenceDetailDto>>
{
    public UpdateFgsSetupServiceAssetModelReferenceCommandValidator(IEntityReadRepository readRepository)
    {
        var descriptor = FgsSetupServiceAssetModelReferenceDescriptor.Create();
        RuleFor(x => x.EntityKey).Equal(descriptor.Key);
        RuleFor(x => x.Id).NotEmpty();
        CatalogEntityValidatorFactory.ApplyUpdateRules(this, descriptor, readRepository, x => x.Payload, x => x.Id);
    }
}

public sealed class PatchFgsSetupServiceAssetModelReferenceCommandValidator : AbstractValidator<PatchCatalogEntityCommand<FgsSetupServiceAssetModelReferencePatchDto, FgsSetupServiceAssetModelReferenceDetailDto>>
{
    public PatchFgsSetupServiceAssetModelReferenceCommandValidator()
    {
        var descriptor = FgsSetupServiceAssetModelReferenceDescriptor.Create();
        RuleFor(x => x.EntityKey).Equal(descriptor.Key);
        RuleFor(x => x.Id).NotEmpty();
        CatalogEntityValidatorFactory.ApplyPatchRules(this, descriptor, x => x.Payload);
    }
}
