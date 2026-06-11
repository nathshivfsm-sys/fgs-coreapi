using FluentValidation;
using Fgs.Foundation.CatalogCrud.Abstractions;
using Fgs.Foundation.CatalogCrud.Commands;
using Fgs.Foundation.CatalogCrud.Validation;
using Fgs.Setup.Application.Features.Generated.Descriptors;
using Fgs.Setup.Application.Features.Generated.Dtos;

namespace Fgs.Setup.Application.Features.Generated.Validators;

public sealed class CreateFgsSetupTaxCommandValidator : AbstractValidator<CreateCatalogEntityCommand<FgsSetupTaxCreateDto, FgsSetupTaxDetailDto>>
{
    public CreateFgsSetupTaxCommandValidator(IEntityReadRepository readRepository)
    {
        var descriptor = FgsSetupTaxDescriptor.Create();
        RuleFor(x => x.EntityKey).Equal(descriptor.Key);
        CatalogEntityValidatorFactory.ApplyCreateRules(this, descriptor, readRepository, x => x.Payload);
    }
}

public sealed class UpdateFgsSetupTaxCommandValidator : AbstractValidator<UpdateCatalogEntityCommand<FgsSetupTaxUpdateDto, FgsSetupTaxDetailDto>>
{
    public UpdateFgsSetupTaxCommandValidator(IEntityReadRepository readRepository)
    {
        var descriptor = FgsSetupTaxDescriptor.Create();
        RuleFor(x => x.EntityKey).Equal(descriptor.Key);
        RuleFor(x => x.Id).NotEmpty();
        CatalogEntityValidatorFactory.ApplyUpdateRules(this, descriptor, readRepository, x => x.Payload, x => x.Id);
    }
}

public sealed class PatchFgsSetupTaxCommandValidator : AbstractValidator<PatchCatalogEntityCommand<FgsSetupTaxPatchDto, FgsSetupTaxDetailDto>>
{
    public PatchFgsSetupTaxCommandValidator()
    {
        var descriptor = FgsSetupTaxDescriptor.Create();
        RuleFor(x => x.EntityKey).Equal(descriptor.Key);
        RuleFor(x => x.Id).NotEmpty();
        CatalogEntityValidatorFactory.ApplyPatchRules(this, descriptor, x => x.Payload);
    }
}
