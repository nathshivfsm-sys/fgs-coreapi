using FluentValidation;
using Fgs.Foundation.CatalogCrud.Abstractions;
using Fgs.Foundation.CatalogCrud.Commands;
using Fgs.Foundation.CatalogCrud.Validation;
using Fgs.Setup.Application.Features.Generated.Descriptors;
using Fgs.Setup.Application.Features.Generated.Dtos;

namespace Fgs.Setup.Application.Features.Generated.Validators;

public sealed class CreateFgsSalesDispositionReasonCommandValidator : AbstractValidator<CreateCatalogEntityCommand<FgsSalesDispositionReasonCreateDto, FgsSalesDispositionReasonDetailDto>>
{
    public CreateFgsSalesDispositionReasonCommandValidator(IEntityReadRepository readRepository)
    {
        var descriptor = FgsSalesDispositionReasonDescriptor.Create();
        RuleFor(x => x.EntityKey).Equal(descriptor.Key);
        CatalogEntityValidatorFactory.ApplyCreateRules(this, descriptor, readRepository, x => x.Payload);
    }
}

public sealed class UpdateFgsSalesDispositionReasonCommandValidator : AbstractValidator<UpdateCatalogEntityCommand<FgsSalesDispositionReasonUpdateDto, FgsSalesDispositionReasonDetailDto>>
{
    public UpdateFgsSalesDispositionReasonCommandValidator(IEntityReadRepository readRepository)
    {
        var descriptor = FgsSalesDispositionReasonDescriptor.Create();
        RuleFor(x => x.EntityKey).Equal(descriptor.Key);
        RuleFor(x => x.Id).NotEmpty();
        CatalogEntityValidatorFactory.ApplyUpdateRules(this, descriptor, readRepository, x => x.Payload, x => x.Id);
    }
}

public sealed class PatchFgsSalesDispositionReasonCommandValidator : AbstractValidator<PatchCatalogEntityCommand<FgsSalesDispositionReasonPatchDto, FgsSalesDispositionReasonDetailDto>>
{
    public PatchFgsSalesDispositionReasonCommandValidator()
    {
        var descriptor = FgsSalesDispositionReasonDescriptor.Create();
        RuleFor(x => x.EntityKey).Equal(descriptor.Key);
        RuleFor(x => x.Id).NotEmpty();
        CatalogEntityValidatorFactory.ApplyPatchRules(this, descriptor, x => x.Payload);
    }
}
