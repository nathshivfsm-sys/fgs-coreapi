using FluentValidation;
using Fgs.Foundation.CatalogCrud.Abstractions;
using Fgs.Foundation.CatalogCrud.Commands;
using Fgs.Foundation.CatalogCrud.Validation;
using Fgs.Setup.Application.Features.Generated.Descriptors;
using Fgs.Setup.Application.Features.Generated.Dtos;

namespace Fgs.Setup.Application.Features.Generated.Validators;

public sealed class CreateFgsInventoryItemTypeCommandValidator : AbstractValidator<CreateCatalogEntityCommand<FgsInventoryItemTypeCreateDto, FgsInventoryItemTypeDetailDto>>
{
    public CreateFgsInventoryItemTypeCommandValidator(IEntityReadRepository readRepository)
    {
        var descriptor = FgsInventoryItemTypeDescriptor.Create();
        RuleFor(x => x.EntityKey).Equal(descriptor.Key);
        CatalogEntityValidatorFactory.ApplyCreateRules(this, descriptor, readRepository, x => x.Payload);
    }
}

public sealed class UpdateFgsInventoryItemTypeCommandValidator : AbstractValidator<UpdateCatalogEntityCommand<FgsInventoryItemTypeUpdateDto, FgsInventoryItemTypeDetailDto>>
{
    public UpdateFgsInventoryItemTypeCommandValidator(IEntityReadRepository readRepository)
    {
        var descriptor = FgsInventoryItemTypeDescriptor.Create();
        RuleFor(x => x.EntityKey).Equal(descriptor.Key);
        RuleFor(x => x.Id).NotEmpty();
        CatalogEntityValidatorFactory.ApplyUpdateRules(this, descriptor, readRepository, x => x.Payload, x => x.Id);
    }
}

public sealed class PatchFgsInventoryItemTypeCommandValidator : AbstractValidator<PatchCatalogEntityCommand<FgsInventoryItemTypePatchDto, FgsInventoryItemTypeDetailDto>>
{
    public PatchFgsInventoryItemTypeCommandValidator()
    {
        var descriptor = FgsInventoryItemTypeDescriptor.Create();
        RuleFor(x => x.EntityKey).Equal(descriptor.Key);
        RuleFor(x => x.Id).NotEmpty();
        CatalogEntityValidatorFactory.ApplyPatchRules(this, descriptor, x => x.Payload);
    }
}
