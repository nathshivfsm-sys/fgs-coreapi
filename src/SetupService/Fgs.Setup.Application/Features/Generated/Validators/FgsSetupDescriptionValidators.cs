using FluentValidation;
using Fgs.Foundation.CatalogCrud.Abstractions;
using Fgs.Foundation.CatalogCrud.Commands;
using Fgs.Foundation.CatalogCrud.Validation;
using Fgs.Setup.Application.Features.Generated.Descriptors;
using Fgs.Setup.Application.Features.Generated.Dtos;

namespace Fgs.Setup.Application.Features.Generated.Validators;

public sealed class CreateFgsSetupDescriptionCommandValidator : AbstractValidator<CreateCatalogEntityCommand<FgsSetupDescriptionCreateDto, FgsSetupDescriptionDetailDto>>
{
    public CreateFgsSetupDescriptionCommandValidator(IEntityReadRepository readRepository)
    {
        var descriptor = FgsSetupDescriptionDescriptor.Create();
        RuleFor(x => x.EntityKey).Equal(descriptor.Key);
        CatalogEntityValidatorFactory.ApplyCreateRules(this, descriptor, readRepository, x => x.Payload);
    }
}

public sealed class UpdateFgsSetupDescriptionCommandValidator : AbstractValidator<UpdateCatalogEntityCommand<FgsSetupDescriptionUpdateDto, FgsSetupDescriptionDetailDto>>
{
    public UpdateFgsSetupDescriptionCommandValidator(IEntityReadRepository readRepository)
    {
        var descriptor = FgsSetupDescriptionDescriptor.Create();
        RuleFor(x => x.EntityKey).Equal(descriptor.Key);
        RuleFor(x => x.Id).NotEmpty();
        CatalogEntityValidatorFactory.ApplyUpdateRules(this, descriptor, readRepository, x => x.Payload, x => x.Id);
    }
}

public sealed class PatchFgsSetupDescriptionCommandValidator : AbstractValidator<PatchCatalogEntityCommand<FgsSetupDescriptionPatchDto, FgsSetupDescriptionDetailDto>>
{
    public PatchFgsSetupDescriptionCommandValidator()
    {
        var descriptor = FgsSetupDescriptionDescriptor.Create();
        RuleFor(x => x.EntityKey).Equal(descriptor.Key);
        RuleFor(x => x.Id).NotEmpty();
        CatalogEntityValidatorFactory.ApplyPatchRules(this, descriptor, x => x.Payload);
    }
}
