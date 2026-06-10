using FluentValidation;
using Fgs.Foundation.CatalogCrud.Abstractions;
using Fgs.Foundation.CatalogCrud.Commands;
using Fgs.Foundation.CatalogCrud.Validation;
using Fgs.Setup.Application.Features.Generated.Descriptors;
using Fgs.Setup.Application.Features.Generated.Dtos;

namespace Fgs.Setup.Application.Features.Generated.Validators;

public sealed class CreateFgsSetupTaxDetailCommandValidator : AbstractValidator<CreateCatalogEntityCommand<FgsSetupTaxDetailCreateDto, FgsSetupTaxDetailDetailDto>>
{
    public CreateFgsSetupTaxDetailCommandValidator(IEntityReadRepository readRepository)
    {
        var descriptor = FgsSetupTaxDetailDescriptor.Create();
        RuleFor(x => x.EntityKey).Equal(descriptor.Key);
        CatalogEntityValidatorFactory.ApplyCreateRules(this, descriptor, readRepository, x => x.Payload);
    }
}

public sealed class UpdateFgsSetupTaxDetailCommandValidator : AbstractValidator<UpdateCatalogEntityCommand<FgsSetupTaxDetailUpdateDto, FgsSetupTaxDetailDetailDto>>
{
    public UpdateFgsSetupTaxDetailCommandValidator(IEntityReadRepository readRepository)
    {
        var descriptor = FgsSetupTaxDetailDescriptor.Create();
        RuleFor(x => x.EntityKey).Equal(descriptor.Key);
        RuleFor(x => x.Id).NotEmpty();
        CatalogEntityValidatorFactory.ApplyUpdateRules(this, descriptor, readRepository, x => x.Payload, x => x.Id);
    }
}

public sealed class PatchFgsSetupTaxDetailCommandValidator : AbstractValidator<PatchCatalogEntityCommand<FgsSetupTaxDetailPatchDto, FgsSetupTaxDetailDetailDto>>
{
    public PatchFgsSetupTaxDetailCommandValidator()
    {
        var descriptor = FgsSetupTaxDetailDescriptor.Create();
        RuleFor(x => x.EntityKey).Equal(descriptor.Key);
        RuleFor(x => x.Id).NotEmpty();
        CatalogEntityValidatorFactory.ApplyPatchRules(this, descriptor, x => x.Payload);
    }
}
