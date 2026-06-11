using FluentValidation;
using Fgs.Foundation.CatalogCrud.Abstractions;
using Fgs.Foundation.CatalogCrud.Commands;
using Fgs.Foundation.CatalogCrud.Validation;
using Fgs.Setup.Application.Features.Generated.Descriptors;
using Fgs.Setup.Application.Features.Generated.Dtos;

namespace Fgs.Setup.Application.Features.Generated.Validators;

public sealed class CreateFgsJobTypeCategoryCommandValidator : AbstractValidator<CreateCatalogEntityCommand<FgsJobTypeCategoryCreateDto, FgsJobTypeCategoryDetailDto>>
{
    public CreateFgsJobTypeCategoryCommandValidator(IEntityReadRepository readRepository)
    {
        var descriptor = FgsJobTypeCategoryDescriptor.Create();
        RuleFor(x => x.EntityKey).Equal(descriptor.Key);
        CatalogEntityValidatorFactory.ApplyCreateRules(this, descriptor, readRepository, x => x.Payload);
    }
}

public sealed class UpdateFgsJobTypeCategoryCommandValidator : AbstractValidator<UpdateCatalogEntityCommand<FgsJobTypeCategoryUpdateDto, FgsJobTypeCategoryDetailDto>>
{
    public UpdateFgsJobTypeCategoryCommandValidator(IEntityReadRepository readRepository)
    {
        var descriptor = FgsJobTypeCategoryDescriptor.Create();
        RuleFor(x => x.EntityKey).Equal(descriptor.Key);
        RuleFor(x => x.Id).NotEmpty();
        CatalogEntityValidatorFactory.ApplyUpdateRules(this, descriptor, readRepository, x => x.Payload, x => x.Id);
    }
}

public sealed class PatchFgsJobTypeCategoryCommandValidator : AbstractValidator<PatchCatalogEntityCommand<FgsJobTypeCategoryPatchDto, FgsJobTypeCategoryDetailDto>>
{
    public PatchFgsJobTypeCategoryCommandValidator()
    {
        var descriptor = FgsJobTypeCategoryDescriptor.Create();
        RuleFor(x => x.EntityKey).Equal(descriptor.Key);
        RuleFor(x => x.Id).NotEmpty();
        CatalogEntityValidatorFactory.ApplyPatchRules(this, descriptor, x => x.Payload);
    }
}
