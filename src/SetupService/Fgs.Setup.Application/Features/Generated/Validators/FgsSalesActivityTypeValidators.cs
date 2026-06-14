using FluentValidation;
using Fgs.Foundation.CatalogCrud.Abstractions;
using Fgs.Foundation.CatalogCrud.Commands;
using Fgs.Foundation.CatalogCrud.Validation;
using Fgs.Setup.Application.Features.Generated.Descriptors;
using Fgs.Setup.Application.Features.Generated.Dtos;

namespace Fgs.Setup.Application.Features.Generated.Validators;

public sealed class CreateFgsSalesActivityTypeCommandValidator : AbstractValidator<CreateCatalogEntityCommand<FgsSalesActivityTypeCreateDto, FgsSalesActivityTypeDetailDto>>
{
    public CreateFgsSalesActivityTypeCommandValidator(IEntityReadRepository readRepository)
    {
        var descriptor = FgsSalesActivityTypeDescriptor.Create();
        RuleFor(x => x.EntityKey).Equal(descriptor.Key);
        CatalogEntityValidatorFactory.ApplyCreateRules(this, descriptor, readRepository, x => x.Payload);
    }
}

public sealed class UpdateFgsSalesActivityTypeCommandValidator : AbstractValidator<UpdateCatalogEntityCommand<FgsSalesActivityTypeUpdateDto, FgsSalesActivityTypeDetailDto>>
{
    public UpdateFgsSalesActivityTypeCommandValidator(IEntityReadRepository readRepository)
    {
        var descriptor = FgsSalesActivityTypeDescriptor.Create();
        RuleFor(x => x.EntityKey).Equal(descriptor.Key);
        RuleFor(x => x.Id).NotEmpty();
        CatalogEntityValidatorFactory.ApplyUpdateRules(this, descriptor, readRepository, x => x.Payload, x => x.Id);
    }
}

public sealed class PatchFgsSalesActivityTypeCommandValidator : AbstractValidator<PatchCatalogEntityCommand<FgsSalesActivityTypePatchDto, FgsSalesActivityTypeDetailDto>>
{
    public PatchFgsSalesActivityTypeCommandValidator()
    {
        var descriptor = FgsSalesActivityTypeDescriptor.Create();
        RuleFor(x => x.EntityKey).Equal(descriptor.Key);
        RuleFor(x => x.Id).NotEmpty();
        CatalogEntityValidatorFactory.ApplyPatchRules(this, descriptor, x => x.Payload);
    }
}
