using FluentValidation;
using Fgs.Foundation.CatalogCrud.Abstractions;
using Fgs.Foundation.CatalogCrud.Commands;
using Fgs.Foundation.CatalogCrud.Validation;
using Fgs.Setup.Application.Features.Generated.Descriptors;
using Fgs.Setup.Application.Features.Generated.Dtos;

namespace Fgs.Setup.Application.Features.Generated.Validators;

public sealed class CreateFgsSetupTechTradeCommandValidator : AbstractValidator<CreateCatalogEntityCommand<FgsSetupTechTradeCreateDto, FgsSetupTechTradeDetailDto>>
{
    public CreateFgsSetupTechTradeCommandValidator(IEntityReadRepository readRepository)
    {
        var descriptor = FgsSetupTechTradeDescriptor.Create();
        RuleFor(x => x.EntityKey).Equal(descriptor.Key);
        CatalogEntityValidatorFactory.ApplyCreateRules(this, descriptor, readRepository, x => x.Payload);
    }
}

public sealed class UpdateFgsSetupTechTradeCommandValidator : AbstractValidator<UpdateCatalogEntityCommand<FgsSetupTechTradeUpdateDto, FgsSetupTechTradeDetailDto>>
{
    public UpdateFgsSetupTechTradeCommandValidator(IEntityReadRepository readRepository)
    {
        var descriptor = FgsSetupTechTradeDescriptor.Create();
        RuleFor(x => x.EntityKey).Equal(descriptor.Key);
        RuleFor(x => x.Id).NotEmpty();
        CatalogEntityValidatorFactory.ApplyUpdateRules(this, descriptor, readRepository, x => x.Payload, x => x.Id);
    }
}

public sealed class PatchFgsSetupTechTradeCommandValidator : AbstractValidator<PatchCatalogEntityCommand<FgsSetupTechTradePatchDto, FgsSetupTechTradeDetailDto>>
{
    public PatchFgsSetupTechTradeCommandValidator()
    {
        var descriptor = FgsSetupTechTradeDescriptor.Create();
        RuleFor(x => x.EntityKey).Equal(descriptor.Key);
        RuleFor(x => x.Id).NotEmpty();
        CatalogEntityValidatorFactory.ApplyPatchRules(this, descriptor, x => x.Payload);
    }
}
