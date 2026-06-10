using FluentValidation;
using Fgs.Foundation.CatalogCrud.Abstractions;
using Fgs.Foundation.CatalogCrud.Commands;
using Fgs.Foundation.CatalogCrud.Validation;
using Fgs.Setup.Application.Features.Generated.Descriptors;
using Fgs.Setup.Application.Features.Generated.Dtos;

namespace Fgs.Setup.Application.Features.Generated.Validators;

public sealed class CreateFgsInventoryItemAlternateCommandValidator : AbstractValidator<CreateCatalogEntityCommand<FgsInventoryItemAlternateCreateDto, FgsInventoryItemAlternateDetailDto>>
{
    public CreateFgsInventoryItemAlternateCommandValidator(IEntityReadRepository readRepository)
    {
        var descriptor = FgsInventoryItemAlternateDescriptor.Create();
        RuleFor(x => x.EntityKey).Equal(descriptor.Key);
        CatalogEntityValidatorFactory.ApplyCreateRules(this, descriptor, readRepository, x => x.Payload);
    }
}

public sealed class UpdateFgsInventoryItemAlternateCommandValidator : AbstractValidator<UpdateCatalogEntityCommand<FgsInventoryItemAlternateUpdateDto, FgsInventoryItemAlternateDetailDto>>
{
    public UpdateFgsInventoryItemAlternateCommandValidator(IEntityReadRepository readRepository)
    {
        var descriptor = FgsInventoryItemAlternateDescriptor.Create();
        RuleFor(x => x.EntityKey).Equal(descriptor.Key);
        RuleFor(x => x.Id).NotEmpty();
        CatalogEntityValidatorFactory.ApplyUpdateRules(this, descriptor, readRepository, x => x.Payload, x => x.Id);
    }
}

public sealed class PatchFgsInventoryItemAlternateCommandValidator : AbstractValidator<PatchCatalogEntityCommand<FgsInventoryItemAlternatePatchDto, FgsInventoryItemAlternateDetailDto>>
{
    public PatchFgsInventoryItemAlternateCommandValidator()
    {
        var descriptor = FgsInventoryItemAlternateDescriptor.Create();
        RuleFor(x => x.EntityKey).Equal(descriptor.Key);
        RuleFor(x => x.Id).NotEmpty();
        CatalogEntityValidatorFactory.ApplyPatchRules(this, descriptor, x => x.Payload);
    }
}
