using FluentValidation;
using Fgs.Foundation.CatalogCrud.Abstractions;
using Fgs.Foundation.CatalogCrud.Commands;
using Fgs.Foundation.CatalogCrud.Validation;
using Fgs.Setup.Application.Features.Generated.Descriptors;
using Fgs.Setup.Application.Features.Generated.Dtos;

namespace Fgs.Setup.Application.Features.Generated.Validators;

public sealed class CreateFgsLeadStatusCommandValidator : AbstractValidator<CreateCatalogEntityCommand<FgsLeadStatusCreateDto, FgsLeadStatusDetailDto>>
{
    public CreateFgsLeadStatusCommandValidator(IEntityReadRepository readRepository)
    {
        var descriptor = FgsLeadStatusDescriptor.Create();
        RuleFor(x => x.EntityKey).Equal(descriptor.Key);
        CatalogEntityValidatorFactory.ApplyCreateRules(this, descriptor, readRepository, x => x.Payload);
    }
}

public sealed class UpdateFgsLeadStatusCommandValidator : AbstractValidator<UpdateCatalogEntityCommand<FgsLeadStatusUpdateDto, FgsLeadStatusDetailDto>>
{
    public UpdateFgsLeadStatusCommandValidator(IEntityReadRepository readRepository)
    {
        var descriptor = FgsLeadStatusDescriptor.Create();
        RuleFor(x => x.EntityKey).Equal(descriptor.Key);
        RuleFor(x => x.Id).NotEmpty();
        CatalogEntityValidatorFactory.ApplyUpdateRules(this, descriptor, readRepository, x => x.Payload, x => x.Id);
    }
}

public sealed class PatchFgsLeadStatusCommandValidator : AbstractValidator<PatchCatalogEntityCommand<FgsLeadStatusPatchDto, FgsLeadStatusDetailDto>>
{
    public PatchFgsLeadStatusCommandValidator()
    {
        var descriptor = FgsLeadStatusDescriptor.Create();
        RuleFor(x => x.EntityKey).Equal(descriptor.Key);
        RuleFor(x => x.Id).NotEmpty();
        CatalogEntityValidatorFactory.ApplyPatchRules(this, descriptor, x => x.Payload);
    }
}
