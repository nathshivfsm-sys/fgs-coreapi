using FluentValidation;
using Fgs.Foundation.CatalogCrud.Abstractions;
using Fgs.Foundation.CatalogCrud.Commands;
using Fgs.Foundation.CatalogCrud.Validation;
using Fgs.Setup.Application.Features.Generated.Descriptors;
using Fgs.Setup.Application.Features.Generated.Dtos;

namespace Fgs.Setup.Application.Features.Generated.Validators;

public sealed class CreateFgsBillingCategoryCommandValidator : AbstractValidator<CreateCatalogEntityCommand<FgsBillingCategoryCreateDto, FgsBillingCategoryDetailDto>>
{
    public CreateFgsBillingCategoryCommandValidator(IEntityReadRepository readRepository)
    {
        var descriptor = FgsBillingCategoryDescriptor.Create();
        RuleFor(x => x.EntityKey).Equal(descriptor.Key);
        CatalogEntityValidatorFactory.ApplyCreateRules(this, descriptor, readRepository, x => x.Payload);
    }
}

public sealed class UpdateFgsBillingCategoryCommandValidator : AbstractValidator<UpdateCatalogEntityCommand<FgsBillingCategoryUpdateDto, FgsBillingCategoryDetailDto>>
{
    public UpdateFgsBillingCategoryCommandValidator(IEntityReadRepository readRepository)
    {
        var descriptor = FgsBillingCategoryDescriptor.Create();
        RuleFor(x => x.EntityKey).Equal(descriptor.Key);
        RuleFor(x => x.Id).NotEmpty();
        CatalogEntityValidatorFactory.ApplyUpdateRules(this, descriptor, readRepository, x => x.Payload, x => x.Id);
    }
}

public sealed class PatchFgsBillingCategoryCommandValidator : AbstractValidator<PatchCatalogEntityCommand<FgsBillingCategoryPatchDto, FgsBillingCategoryDetailDto>>
{
    public PatchFgsBillingCategoryCommandValidator()
    {
        var descriptor = FgsBillingCategoryDescriptor.Create();
        RuleFor(x => x.EntityKey).Equal(descriptor.Key);
        RuleFor(x => x.Id).NotEmpty();
        CatalogEntityValidatorFactory.ApplyPatchRules(this, descriptor, x => x.Payload);
    }
}
