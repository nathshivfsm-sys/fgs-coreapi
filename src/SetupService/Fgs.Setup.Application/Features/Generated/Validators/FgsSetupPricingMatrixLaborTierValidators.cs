using FluentValidation;
using Fgs.Foundation.CatalogCrud.Abstractions;
using Fgs.Foundation.CatalogCrud.Commands;
using Fgs.Foundation.CatalogCrud.Validation;
using Fgs.Setup.Application.Features.Generated.Descriptors;
using Fgs.Setup.Application.Features.Generated.Dtos;

namespace Fgs.Setup.Application.Features.Generated.Validators;

public sealed class CreateFgsSetupPricingMatrixLaborTierCommandValidator : AbstractValidator<CreateCatalogEntityCommand<FgsSetupPricingMatrixLaborTierCreateDto, FgsSetupPricingMatrixLaborTierDetailDto>>
{
    public CreateFgsSetupPricingMatrixLaborTierCommandValidator(IEntityReadRepository readRepository)
    {
        var descriptor = FgsSetupPricingMatrixLaborTierDescriptor.Create();
        RuleFor(x => x.EntityKey).Equal(descriptor.Key);
        CatalogEntityValidatorFactory.ApplyCreateRules(this, descriptor, readRepository, x => x.Payload);
    }
}

public sealed class UpdateFgsSetupPricingMatrixLaborTierCommandValidator : AbstractValidator<UpdateCatalogEntityCommand<FgsSetupPricingMatrixLaborTierUpdateDto, FgsSetupPricingMatrixLaborTierDetailDto>>
{
    public UpdateFgsSetupPricingMatrixLaborTierCommandValidator(IEntityReadRepository readRepository)
    {
        var descriptor = FgsSetupPricingMatrixLaborTierDescriptor.Create();
        RuleFor(x => x.EntityKey).Equal(descriptor.Key);
        RuleFor(x => x.Id).NotEmpty();
        CatalogEntityValidatorFactory.ApplyUpdateRules(this, descriptor, readRepository, x => x.Payload, x => x.Id);
    }
}

public sealed class PatchFgsSetupPricingMatrixLaborTierCommandValidator : AbstractValidator<PatchCatalogEntityCommand<FgsSetupPricingMatrixLaborTierPatchDto, FgsSetupPricingMatrixLaborTierDetailDto>>
{
    public PatchFgsSetupPricingMatrixLaborTierCommandValidator()
    {
        var descriptor = FgsSetupPricingMatrixLaborTierDescriptor.Create();
        RuleFor(x => x.EntityKey).Equal(descriptor.Key);
        RuleFor(x => x.Id).NotEmpty();
        CatalogEntityValidatorFactory.ApplyPatchRules(this, descriptor, x => x.Payload);
    }
}
