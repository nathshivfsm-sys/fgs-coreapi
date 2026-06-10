using FluentValidation;
using Fgs.Foundation.CatalogCrud.Abstractions;
using Fgs.Foundation.CatalogCrud.Commands;
using Fgs.Foundation.CatalogCrud.Validation;
using Fgs.Setup.Application.Features.Generated.Descriptors;
using Fgs.Setup.Application.Features.Generated.Dtos;

namespace Fgs.Setup.Application.Features.Generated.Validators;

public sealed class CreateFgsJobTypeSubCategoryCommandValidator : AbstractValidator<CreateCatalogEntityCommand<FgsJobTypeSubCategoryCreateDto, FgsJobTypeSubCategoryDetailDto>>
{
    public CreateFgsJobTypeSubCategoryCommandValidator(IEntityReadRepository readRepository)
    {
        var descriptor = FgsJobTypeSubCategoryDescriptor.Create();
        RuleFor(x => x.EntityKey).Equal(descriptor.Key);
        CatalogEntityValidatorFactory.ApplyCreateRules(this, descriptor, readRepository, x => x.Payload);
    }
}

public sealed class UpdateFgsJobTypeSubCategoryCommandValidator : AbstractValidator<UpdateCatalogEntityCommand<FgsJobTypeSubCategoryUpdateDto, FgsJobTypeSubCategoryDetailDto>>
{
    public UpdateFgsJobTypeSubCategoryCommandValidator(IEntityReadRepository readRepository)
    {
        var descriptor = FgsJobTypeSubCategoryDescriptor.Create();
        RuleFor(x => x.EntityKey).Equal(descriptor.Key);
        RuleFor(x => x.Id).NotEmpty();
        CatalogEntityValidatorFactory.ApplyUpdateRules(this, descriptor, readRepository, x => x.Payload, x => x.Id);
    }
}

public sealed class PatchFgsJobTypeSubCategoryCommandValidator : AbstractValidator<PatchCatalogEntityCommand<FgsJobTypeSubCategoryPatchDto, FgsJobTypeSubCategoryDetailDto>>
{
    public PatchFgsJobTypeSubCategoryCommandValidator()
    {
        var descriptor = FgsJobTypeSubCategoryDescriptor.Create();
        RuleFor(x => x.EntityKey).Equal(descriptor.Key);
        RuleFor(x => x.Id).NotEmpty();
        CatalogEntityValidatorFactory.ApplyPatchRules(this, descriptor, x => x.Payload);
    }
}
