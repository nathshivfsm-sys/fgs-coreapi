using FluentValidation;
using Fgs.Foundation.CatalogCrud.Abstractions;
using Fgs.Foundation.CatalogCrud.Commands;
using Fgs.Foundation.CatalogCrud.Validation;
using Fgs.Setup.Application.Features.Generated.Descriptors;
using Fgs.Setup.Application.Features.Generated.Dtos;

namespace Fgs.Setup.Application.Features.Generated.Validators;

public sealed class CreateFgsLeadDisqualificationReasonCommandValidator : AbstractValidator<CreateCatalogEntityCommand<FgsLeadDisqualificationReasonCreateDto, FgsLeadDisqualificationReasonDetailDto>>
{
    public CreateFgsLeadDisqualificationReasonCommandValidator(IEntityReadRepository readRepository)
    {
        var descriptor = FgsLeadDisqualificationReasonDescriptor.Create();
        RuleFor(x => x.EntityKey).Equal(descriptor.Key);
        CatalogEntityValidatorFactory.ApplyCreateRules(this, descriptor, readRepository, x => x.Payload);
    }
}

public sealed class UpdateFgsLeadDisqualificationReasonCommandValidator : AbstractValidator<UpdateCatalogEntityCommand<FgsLeadDisqualificationReasonUpdateDto, FgsLeadDisqualificationReasonDetailDto>>
{
    public UpdateFgsLeadDisqualificationReasonCommandValidator(IEntityReadRepository readRepository)
    {
        var descriptor = FgsLeadDisqualificationReasonDescriptor.Create();
        RuleFor(x => x.EntityKey).Equal(descriptor.Key);
        RuleFor(x => x.Id).NotEmpty();
        CatalogEntityValidatorFactory.ApplyUpdateRules(this, descriptor, readRepository, x => x.Payload, x => x.Id);
    }
}

public sealed class PatchFgsLeadDisqualificationReasonCommandValidator : AbstractValidator<PatchCatalogEntityCommand<FgsLeadDisqualificationReasonPatchDto, FgsLeadDisqualificationReasonDetailDto>>
{
    public PatchFgsLeadDisqualificationReasonCommandValidator()
    {
        var descriptor = FgsLeadDisqualificationReasonDescriptor.Create();
        RuleFor(x => x.EntityKey).Equal(descriptor.Key);
        RuleFor(x => x.Id).NotEmpty();
        CatalogEntityValidatorFactory.ApplyPatchRules(this, descriptor, x => x.Payload);
    }
}
