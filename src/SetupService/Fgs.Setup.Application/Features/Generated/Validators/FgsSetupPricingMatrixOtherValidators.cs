using FluentValidation;
using Fgs.Foundation.CatalogCrud.Abstractions;
using Fgs.Foundation.CatalogCrud.Commands;
using Fgs.Foundation.CatalogCrud.Validation;
using Fgs.Setup.Application.Features.Generated.Descriptors;
using Fgs.Setup.Application.Features.Generated.Dtos;

namespace Fgs.Setup.Application.Features.Generated.Validators;

public sealed class CreateFgsSetupPricingMatrixOtherCommandValidator : AbstractValidator<CreateCatalogEntityCommand<FgsSetupPricingMatrixOtherCreateDto, FgsSetupPricingMatrixOtherDetailDto>>
{
    public CreateFgsSetupPricingMatrixOtherCommandValidator(IEntityReadRepository readRepository)
    {
        var descriptor = FgsSetupPricingMatrixOtherDescriptor.Create();
        RuleFor(x => x.EntityKey).Equal(descriptor.Key);
        CatalogEntityValidatorFactory.ApplyCreateRules(this, descriptor, readRepository, x => x.Payload);
    }
}

public sealed class UpdateFgsSetupPricingMatrixOtherCommandValidator : AbstractValidator<UpdateCatalogEntityCommand<FgsSetupPricingMatrixOtherUpdateDto, FgsSetupPricingMatrixOtherDetailDto>>
{
    public UpdateFgsSetupPricingMatrixOtherCommandValidator(IEntityReadRepository readRepository)
    {
        var descriptor = FgsSetupPricingMatrixOtherDescriptor.Create();
        RuleFor(x => x.EntityKey).Equal(descriptor.Key);
        RuleFor(x => x.Id).NotEmpty();
        CatalogEntityValidatorFactory.ApplyUpdateRules(this, descriptor, readRepository, x => x.Payload, x => x.Id);
    }
}

public sealed class PatchFgsSetupPricingMatrixOtherCommandValidator : AbstractValidator<PatchCatalogEntityCommand<FgsSetupPricingMatrixOtherPatchDto, FgsSetupPricingMatrixOtherDetailDto>>
{
    public PatchFgsSetupPricingMatrixOtherCommandValidator()
    {
        var descriptor = FgsSetupPricingMatrixOtherDescriptor.Create();
        RuleFor(x => x.EntityKey).Equal(descriptor.Key);
        RuleFor(x => x.Id).NotEmpty();
        CatalogEntityValidatorFactory.ApplyPatchRules(this, descriptor, x => x.Payload);
    }
}
