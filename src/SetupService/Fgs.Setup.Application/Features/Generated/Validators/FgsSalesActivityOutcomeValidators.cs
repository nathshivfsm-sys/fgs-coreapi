using FluentValidation;
using Fgs.Foundation.CatalogCrud.Abstractions;
using Fgs.Foundation.CatalogCrud.Commands;
using Fgs.Foundation.CatalogCrud.Validation;
using Fgs.Setup.Application.Features.Generated.Descriptors;
using Fgs.Setup.Application.Features.Generated.Dtos;

namespace Fgs.Setup.Application.Features.Generated.Validators;

public sealed class CreateFgsSalesActivityOutcomeCommandValidator : AbstractValidator<CreateCatalogEntityCommand<FgsSalesActivityOutcomeCreateDto, FgsSalesActivityOutcomeDetailDto>>
{
    public CreateFgsSalesActivityOutcomeCommandValidator(IEntityReadRepository readRepository)
    {
        var descriptor = FgsSalesActivityOutcomeDescriptor.Create();
        RuleFor(x => x.EntityKey).Equal(descriptor.Key);
        CatalogEntityValidatorFactory.ApplyCreateRules(this, descriptor, readRepository, x => x.Payload);
    }
}

public sealed class UpdateFgsSalesActivityOutcomeCommandValidator : AbstractValidator<UpdateCatalogEntityCommand<FgsSalesActivityOutcomeUpdateDto, FgsSalesActivityOutcomeDetailDto>>
{
    public UpdateFgsSalesActivityOutcomeCommandValidator(IEntityReadRepository readRepository)
    {
        var descriptor = FgsSalesActivityOutcomeDescriptor.Create();
        RuleFor(x => x.EntityKey).Equal(descriptor.Key);
        RuleFor(x => x.Id).NotEmpty();
        CatalogEntityValidatorFactory.ApplyUpdateRules(this, descriptor, readRepository, x => x.Payload, x => x.Id);
    }
}

public sealed class PatchFgsSalesActivityOutcomeCommandValidator : AbstractValidator<PatchCatalogEntityCommand<FgsSalesActivityOutcomePatchDto, FgsSalesActivityOutcomeDetailDto>>
{
    public PatchFgsSalesActivityOutcomeCommandValidator()
    {
        var descriptor = FgsSalesActivityOutcomeDescriptor.Create();
        RuleFor(x => x.EntityKey).Equal(descriptor.Key);
        RuleFor(x => x.Id).NotEmpty();
        CatalogEntityValidatorFactory.ApplyPatchRules(this, descriptor, x => x.Payload);
    }
}
