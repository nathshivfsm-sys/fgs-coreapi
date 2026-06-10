using FluentValidation;
using Fgs.Foundation.CatalogCrud.Abstractions;
using Fgs.Foundation.CatalogCrud.Commands;
using Fgs.Foundation.CatalogCrud.Validation;
using Fgs.Setup.Application.Features.Generated.Descriptors;
using Fgs.Setup.Application.Features.Generated.Dtos;

namespace Fgs.Setup.Application.Features.Generated.Validators;

public sealed class CreateFgsVendorInventoryItemCommandValidator : AbstractValidator<CreateCatalogEntityCommand<FgsVendorInventoryItemCreateDto, FgsVendorInventoryItemDetailDto>>
{
    public CreateFgsVendorInventoryItemCommandValidator(IEntityReadRepository readRepository)
    {
        var descriptor = FgsVendorInventoryItemDescriptor.Create();
        RuleFor(x => x.EntityKey).Equal(descriptor.Key);
        CatalogEntityValidatorFactory.ApplyCreateRules(this, descriptor, readRepository, x => x.Payload);
    }
}

public sealed class UpdateFgsVendorInventoryItemCommandValidator : AbstractValidator<UpdateCatalogEntityCommand<FgsVendorInventoryItemUpdateDto, FgsVendorInventoryItemDetailDto>>
{
    public UpdateFgsVendorInventoryItemCommandValidator(IEntityReadRepository readRepository)
    {
        var descriptor = FgsVendorInventoryItemDescriptor.Create();
        RuleFor(x => x.EntityKey).Equal(descriptor.Key);
        RuleFor(x => x.Id).NotEmpty();
        CatalogEntityValidatorFactory.ApplyUpdateRules(this, descriptor, readRepository, x => x.Payload, x => x.Id);
    }
}

public sealed class PatchFgsVendorInventoryItemCommandValidator : AbstractValidator<PatchCatalogEntityCommand<FgsVendorInventoryItemPatchDto, FgsVendorInventoryItemDetailDto>>
{
    public PatchFgsVendorInventoryItemCommandValidator()
    {
        var descriptor = FgsVendorInventoryItemDescriptor.Create();
        RuleFor(x => x.EntityKey).Equal(descriptor.Key);
        RuleFor(x => x.Id).NotEmpty();
        CatalogEntityValidatorFactory.ApplyPatchRules(this, descriptor, x => x.Payload);
    }
}
