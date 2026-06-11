using FluentValidation;
using Fgs.Foundation.CatalogCrud.Abstractions;
using Fgs.Foundation.CatalogCrud.Commands;
using Fgs.Foundation.CatalogCrud.Validation;
using Fgs.Setup.Application.Features.Generated.Descriptors;
using Fgs.Setup.Application.Features.Generated.Dtos;

namespace Fgs.Setup.Application.Features.Generated.Validators;

public sealed class CreateFgsSetupPricingMatrixMaterialTierCommandValidator : AbstractValidator<CreateCatalogEntityCommand<FgsSetupPricingMatrixMaterialTierCreateDto, FgsSetupPricingMatrixMaterialTierDetailDto>>
{
    public CreateFgsSetupPricingMatrixMaterialTierCommandValidator(IEntityReadRepository readRepository)
    {
        var descriptor = FgsSetupPricingMatrixMaterialTierDescriptor.Create();
        RuleFor(x => x.EntityKey).Equal(descriptor.Key);
        CatalogEntityValidatorFactory.ApplyCreateRules(this, descriptor, readRepository, x => x.Payload);
    }
}

public sealed class UpdateFgsSetupPricingMatrixMaterialTierCommandValidator : AbstractValidator<UpdateCatalogEntityCommand<FgsSetupPricingMatrixMaterialTierUpdateDto, FgsSetupPricingMatrixMaterialTierDetailDto>>
{
    public UpdateFgsSetupPricingMatrixMaterialTierCommandValidator(IEntityReadRepository readRepository)
    {
        var descriptor = FgsSetupPricingMatrixMaterialTierDescriptor.Create();
        RuleFor(x => x.EntityKey).Equal(descriptor.Key);
        RuleFor(x => x.Id).NotEmpty();
        CatalogEntityValidatorFactory.ApplyUpdateRules(this, descriptor, readRepository, x => x.Payload, x => x.Id);
    }
}

public sealed class PatchFgsSetupPricingMatrixMaterialTierCommandValidator : AbstractValidator<PatchCatalogEntityCommand<FgsSetupPricingMatrixMaterialTierPatchDto, FgsSetupPricingMatrixMaterialTierDetailDto>>
{
    public PatchFgsSetupPricingMatrixMaterialTierCommandValidator()
    {
        var descriptor = FgsSetupPricingMatrixMaterialTierDescriptor.Create();
        RuleFor(x => x.EntityKey).Equal(descriptor.Key);
        RuleFor(x => x.Id).NotEmpty();
        CatalogEntityValidatorFactory.ApplyPatchRules(this, descriptor, x => x.Payload);
    }
}
