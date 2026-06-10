using FluentValidation;
using Fgs.Foundation.CatalogCrud.Abstractions;
using Fgs.Foundation.CatalogCrud.Commands;
using Fgs.Foundation.CatalogCrud.Validation;
using Fgs.Setup.Application.Features.Generated.Descriptors;
using Fgs.Setup.Application.Features.Generated.Dtos;

namespace Fgs.Setup.Application.Features.Generated.Validators;

public sealed class CreateFgsLeadSourceCommandValidator : AbstractValidator<CreateCatalogEntityCommand<FgsLeadSourceCreateDto, FgsLeadSourceDetailDto>>
{
    public CreateFgsLeadSourceCommandValidator(IEntityReadRepository readRepository)
    {
        var descriptor = FgsLeadSourceDescriptor.Create();
        RuleFor(x => x.EntityKey).Equal(descriptor.Key);
        CatalogEntityValidatorFactory.ApplyCreateRules(this, descriptor, readRepository, x => x.Payload);
    }
}

public sealed class UpdateFgsLeadSourceCommandValidator : AbstractValidator<UpdateCatalogEntityCommand<FgsLeadSourceUpdateDto, FgsLeadSourceDetailDto>>
{
    public UpdateFgsLeadSourceCommandValidator(IEntityReadRepository readRepository)
    {
        var descriptor = FgsLeadSourceDescriptor.Create();
        RuleFor(x => x.EntityKey).Equal(descriptor.Key);
        RuleFor(x => x.Id).NotEmpty();
        CatalogEntityValidatorFactory.ApplyUpdateRules(this, descriptor, readRepository, x => x.Payload, x => x.Id);
    }
}

public sealed class PatchFgsLeadSourceCommandValidator : AbstractValidator<PatchCatalogEntityCommand<FgsLeadSourcePatchDto, FgsLeadSourceDetailDto>>
{
    public PatchFgsLeadSourceCommandValidator()
    {
        var descriptor = FgsLeadSourceDescriptor.Create();
        RuleFor(x => x.EntityKey).Equal(descriptor.Key);
        RuleFor(x => x.Id).NotEmpty();
        CatalogEntityValidatorFactory.ApplyPatchRules(this, descriptor, x => x.Payload);
    }
}
