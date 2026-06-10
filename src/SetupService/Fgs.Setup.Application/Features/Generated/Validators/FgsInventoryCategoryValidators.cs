using FluentValidation;
using Fgs.Foundation.CatalogCrud.Abstractions;
using Fgs.Foundation.CatalogCrud.Commands;
using Fgs.Foundation.CatalogCrud.Validation;
using Fgs.Setup.Application.Features.Generated.Descriptors;
using Fgs.Setup.Application.Features.Generated.Dtos;

namespace Fgs.Setup.Application.Features.Generated.Validators;

public sealed class CreateFgsInventoryCategoryCommandValidator : AbstractValidator<CreateCatalogEntityCommand<FgsInventoryCategoryCreateDto, FgsInventoryCategoryDetailDto>>
{
    public CreateFgsInventoryCategoryCommandValidator(IEntityReadRepository readRepository)
    {
        var descriptor = FgsInventoryCategoryDescriptor.Create();
        RuleFor(x => x.EntityKey).Equal(descriptor.Key);
        CatalogEntityValidatorFactory.ApplyCreateRules(this, descriptor, readRepository, x => x.Payload);
    }
}

public sealed class UpdateFgsInventoryCategoryCommandValidator : AbstractValidator<UpdateCatalogEntityCommand<FgsInventoryCategoryUpdateDto, FgsInventoryCategoryDetailDto>>
{
    public UpdateFgsInventoryCategoryCommandValidator(IEntityReadRepository readRepository)
    {
        var descriptor = FgsInventoryCategoryDescriptor.Create();
        RuleFor(x => x.EntityKey).Equal(descriptor.Key);
        RuleFor(x => x.Id).NotEmpty();
        CatalogEntityValidatorFactory.ApplyUpdateRules(this, descriptor, readRepository, x => x.Payload, x => x.Id);
    }
}

public sealed class PatchFgsInventoryCategoryCommandValidator : AbstractValidator<PatchCatalogEntityCommand<FgsInventoryCategoryPatchDto, FgsInventoryCategoryDetailDto>>
{
    public PatchFgsInventoryCategoryCommandValidator()
    {
        var descriptor = FgsInventoryCategoryDescriptor.Create();
        RuleFor(x => x.EntityKey).Equal(descriptor.Key);
        RuleFor(x => x.Id).NotEmpty();
        CatalogEntityValidatorFactory.ApplyPatchRules(this, descriptor, x => x.Payload);
    }
}
