using FluentValidation;
using Fgs.Foundation.CatalogCrud.Abstractions;
using Fgs.Foundation.CatalogCrud.Commands;
using Fgs.Foundation.CatalogCrud.Validation;
using Fgs.Setup.Application.Features.Generated.Descriptors;
using Fgs.Setup.Application.Features.Generated.Dtos;

namespace Fgs.Setup.Application.Features.Generated.Validators;

public sealed class CreateFgsSetupPostalCodeCommandValidator : AbstractValidator<CreateCatalogEntityCommand<FgsSetupPostalCodeCreateDto, FgsSetupPostalCodeDetailDto>>
{
    public CreateFgsSetupPostalCodeCommandValidator(IEntityReadRepository readRepository)
    {
        var descriptor = FgsSetupPostalCodeDescriptor.Create();
        RuleFor(x => x.EntityKey).Equal(descriptor.Key);
        CatalogEntityValidatorFactory.ApplyCreateRules(this, descriptor, readRepository, x => x.Payload);
    }
}

public sealed class UpdateFgsSetupPostalCodeCommandValidator : AbstractValidator<UpdateCatalogEntityCommand<FgsSetupPostalCodeUpdateDto, FgsSetupPostalCodeDetailDto>>
{
    public UpdateFgsSetupPostalCodeCommandValidator(IEntityReadRepository readRepository)
    {
        var descriptor = FgsSetupPostalCodeDescriptor.Create();
        RuleFor(x => x.EntityKey).Equal(descriptor.Key);
        RuleFor(x => x.Id).NotEmpty();
        CatalogEntityValidatorFactory.ApplyUpdateRules(this, descriptor, readRepository, x => x.Payload, x => x.Id);
    }
}

public sealed class PatchFgsSetupPostalCodeCommandValidator : AbstractValidator<PatchCatalogEntityCommand<FgsSetupPostalCodePatchDto, FgsSetupPostalCodeDetailDto>>
{
    public PatchFgsSetupPostalCodeCommandValidator()
    {
        var descriptor = FgsSetupPostalCodeDescriptor.Create();
        RuleFor(x => x.EntityKey).Equal(descriptor.Key);
        RuleFor(x => x.Id).NotEmpty();
        CatalogEntityValidatorFactory.ApplyPatchRules(this, descriptor, x => x.Payload);
    }
}
