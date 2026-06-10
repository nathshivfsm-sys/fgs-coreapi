using FluentValidation;
using Fgs.Foundation.CatalogCrud.Abstractions;
using Fgs.Foundation.CatalogCrud.Commands;
using Fgs.Foundation.CatalogCrud.Validation;
using Fgs.Setup.Application.Features.Generated.Descriptors;
using Fgs.Setup.Application.Features.Generated.Dtos;

namespace Fgs.Setup.Application.Features.Generated.Validators;

public sealed class CreateFgsSetupTaxAuthorityCommandValidator : AbstractValidator<CreateCatalogEntityCommand<FgsSetupTaxAuthorityCreateDto, FgsSetupTaxAuthorityDetailDto>>
{
    public CreateFgsSetupTaxAuthorityCommandValidator(IEntityReadRepository readRepository)
    {
        var descriptor = FgsSetupTaxAuthorityDescriptor.Create();
        RuleFor(x => x.EntityKey).Equal(descriptor.Key);
        CatalogEntityValidatorFactory.ApplyCreateRules(this, descriptor, readRepository, x => x.Payload);
    }
}

public sealed class UpdateFgsSetupTaxAuthorityCommandValidator : AbstractValidator<UpdateCatalogEntityCommand<FgsSetupTaxAuthorityUpdateDto, FgsSetupTaxAuthorityDetailDto>>
{
    public UpdateFgsSetupTaxAuthorityCommandValidator(IEntityReadRepository readRepository)
    {
        var descriptor = FgsSetupTaxAuthorityDescriptor.Create();
        RuleFor(x => x.EntityKey).Equal(descriptor.Key);
        RuleFor(x => x.Id).NotEmpty();
        CatalogEntityValidatorFactory.ApplyUpdateRules(this, descriptor, readRepository, x => x.Payload, x => x.Id);
    }
}

public sealed class PatchFgsSetupTaxAuthorityCommandValidator : AbstractValidator<PatchCatalogEntityCommand<FgsSetupTaxAuthorityPatchDto, FgsSetupTaxAuthorityDetailDto>>
{
    public PatchFgsSetupTaxAuthorityCommandValidator()
    {
        var descriptor = FgsSetupTaxAuthorityDescriptor.Create();
        RuleFor(x => x.EntityKey).Equal(descriptor.Key);
        RuleFor(x => x.Id).NotEmpty();
        CatalogEntityValidatorFactory.ApplyPatchRules(this, descriptor, x => x.Payload);
    }
}
