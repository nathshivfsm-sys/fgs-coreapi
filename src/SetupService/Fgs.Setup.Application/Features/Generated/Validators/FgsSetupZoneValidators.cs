using FluentValidation;
using Fgs.Foundation.CatalogCrud.Abstractions;
using Fgs.Foundation.CatalogCrud.Commands;
using Fgs.Foundation.CatalogCrud.Validation;
using Fgs.Setup.Application.Features.Generated.Descriptors;
using Fgs.Setup.Application.Features.Generated.Dtos;

namespace Fgs.Setup.Application.Features.Generated.Validators;

public sealed class CreateFgsSetupZoneCommandValidator : AbstractValidator<CreateCatalogEntityCommand<FgsSetupZoneCreateDto, FgsSetupZoneDetailDto>>
{
    public CreateFgsSetupZoneCommandValidator(IEntityReadRepository readRepository)
    {
        var descriptor = FgsSetupZoneDescriptor.Create();
        RuleFor(x => x.EntityKey).Equal(descriptor.Key);
        CatalogEntityValidatorFactory.ApplyCreateRules(this, descriptor, readRepository, x => x.Payload);
    }
}

public sealed class UpdateFgsSetupZoneCommandValidator : AbstractValidator<UpdateCatalogEntityCommand<FgsSetupZoneUpdateDto, FgsSetupZoneDetailDto>>
{
    public UpdateFgsSetupZoneCommandValidator(IEntityReadRepository readRepository)
    {
        var descriptor = FgsSetupZoneDescriptor.Create();
        RuleFor(x => x.EntityKey).Equal(descriptor.Key);
        RuleFor(x => x.Id).NotEmpty();
        CatalogEntityValidatorFactory.ApplyUpdateRules(this, descriptor, readRepository, x => x.Payload, x => x.Id);
    }
}

public sealed class PatchFgsSetupZoneCommandValidator : AbstractValidator<PatchCatalogEntityCommand<FgsSetupZonePatchDto, FgsSetupZoneDetailDto>>
{
    public PatchFgsSetupZoneCommandValidator()
    {
        var descriptor = FgsSetupZoneDescriptor.Create();
        RuleFor(x => x.EntityKey).Equal(descriptor.Key);
        RuleFor(x => x.Id).NotEmpty();
        CatalogEntityValidatorFactory.ApplyPatchRules(this, descriptor, x => x.Payload);
    }
}
