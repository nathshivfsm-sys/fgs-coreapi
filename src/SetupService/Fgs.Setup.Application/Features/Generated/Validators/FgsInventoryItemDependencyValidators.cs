using FluentValidation;
using Fgs.Foundation.CatalogCrud.Abstractions;
using Fgs.Foundation.CatalogCrud.Commands;
using Fgs.Foundation.CatalogCrud.Validation;
using Fgs.Setup.Application.Features.Generated.Descriptors;
using Fgs.Setup.Application.Features.Generated.Dtos;

namespace Fgs.Setup.Application.Features.Generated.Validators;

public sealed class CreateFgsInventoryItemDependencyCommandValidator : AbstractValidator<CreateCatalogEntityCommand<FgsInventoryItemDependencyCreateDto, FgsInventoryItemDependencyDetailDto>>
{
    public CreateFgsInventoryItemDependencyCommandValidator(IEntityReadRepository readRepository)
    {
        var descriptor = FgsInventoryItemDependencyDescriptor.Create();
        RuleFor(x => x.EntityKey).Equal(descriptor.Key);
        CatalogEntityValidatorFactory.ApplyCreateRules(this, descriptor, readRepository, x => x.Payload);
    }
}

public sealed class UpdateFgsInventoryItemDependencyCommandValidator : AbstractValidator<UpdateCatalogEntityCommand<FgsInventoryItemDependencyUpdateDto, FgsInventoryItemDependencyDetailDto>>
{
    public UpdateFgsInventoryItemDependencyCommandValidator(IEntityReadRepository readRepository)
    {
        var descriptor = FgsInventoryItemDependencyDescriptor.Create();
        RuleFor(x => x.EntityKey).Equal(descriptor.Key);
        RuleFor(x => x.Id).NotEmpty();
        CatalogEntityValidatorFactory.ApplyUpdateRules(this, descriptor, readRepository, x => x.Payload, x => x.Id);
    }
}

public sealed class PatchFgsInventoryItemDependencyCommandValidator : AbstractValidator<PatchCatalogEntityCommand<FgsInventoryItemDependencyPatchDto, FgsInventoryItemDependencyDetailDto>>
{
    public PatchFgsInventoryItemDependencyCommandValidator()
    {
        var descriptor = FgsInventoryItemDependencyDescriptor.Create();
        RuleFor(x => x.EntityKey).Equal(descriptor.Key);
        RuleFor(x => x.Id).NotEmpty();
        CatalogEntityValidatorFactory.ApplyPatchRules(this, descriptor, x => x.Payload);
    }
}
