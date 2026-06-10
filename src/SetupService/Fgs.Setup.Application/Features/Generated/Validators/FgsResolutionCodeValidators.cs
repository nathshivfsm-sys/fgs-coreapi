using FluentValidation;
using Fgs.Foundation.CatalogCrud.Abstractions;
using Fgs.Foundation.CatalogCrud.Commands;
using Fgs.Foundation.CatalogCrud.Validation;
using Fgs.Setup.Application.Features.Generated.Descriptors;
using Fgs.Setup.Application.Features.Generated.Dtos;

namespace Fgs.Setup.Application.Features.Generated.Validators;

public sealed class CreateFgsResolutionCodeCommandValidator : AbstractValidator<CreateCatalogEntityCommand<FgsResolutionCodeCreateDto, FgsResolutionCodeDetailDto>>
{
    public CreateFgsResolutionCodeCommandValidator(IEntityReadRepository readRepository)
    {
        var descriptor = FgsResolutionCodeDescriptor.Create();
        RuleFor(x => x.EntityKey).Equal(descriptor.Key);
        CatalogEntityValidatorFactory.ApplyCreateRules(this, descriptor, readRepository, x => x.Payload);
    }
}

public sealed class UpdateFgsResolutionCodeCommandValidator : AbstractValidator<UpdateCatalogEntityCommand<FgsResolutionCodeUpdateDto, FgsResolutionCodeDetailDto>>
{
    public UpdateFgsResolutionCodeCommandValidator(IEntityReadRepository readRepository)
    {
        var descriptor = FgsResolutionCodeDescriptor.Create();
        RuleFor(x => x.EntityKey).Equal(descriptor.Key);
        RuleFor(x => x.Id).NotEmpty();
        CatalogEntityValidatorFactory.ApplyUpdateRules(this, descriptor, readRepository, x => x.Payload, x => x.Id);
    }
}

public sealed class PatchFgsResolutionCodeCommandValidator : AbstractValidator<PatchCatalogEntityCommand<FgsResolutionCodePatchDto, FgsResolutionCodeDetailDto>>
{
    public PatchFgsResolutionCodeCommandValidator()
    {
        var descriptor = FgsResolutionCodeDescriptor.Create();
        RuleFor(x => x.EntityKey).Equal(descriptor.Key);
        RuleFor(x => x.Id).NotEmpty();
        CatalogEntityValidatorFactory.ApplyPatchRules(this, descriptor, x => x.Payload);
    }
}
