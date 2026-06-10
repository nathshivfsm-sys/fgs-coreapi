using FluentValidation;
using Fgs.Foundation.CatalogCrud.Abstractions;
using Fgs.Foundation.CatalogCrud.Commands;
using Fgs.Foundation.CatalogCrud.Validation;
using Fgs.Setup.Application.Features.Generated.Descriptors;
using Fgs.Setup.Application.Features.Generated.Dtos;

namespace Fgs.Setup.Application.Features.Generated.Validators;

public sealed class CreateFgsSetupLaborRateTypeCommandValidator : AbstractValidator<CreateCatalogEntityCommand<FgsSetupLaborRateTypeCreateDto, FgsSetupLaborRateTypeDetailDto>>
{
    public CreateFgsSetupLaborRateTypeCommandValidator(IEntityReadRepository readRepository)
    {
        var descriptor = FgsSetupLaborRateTypeDescriptor.Create();
        RuleFor(x => x.EntityKey).Equal(descriptor.Key);
        CatalogEntityValidatorFactory.ApplyCreateRules(this, descriptor, readRepository, x => x.Payload);
    }
}

public sealed class UpdateFgsSetupLaborRateTypeCommandValidator : AbstractValidator<UpdateCatalogEntityCommand<FgsSetupLaborRateTypeUpdateDto, FgsSetupLaborRateTypeDetailDto>>
{
    public UpdateFgsSetupLaborRateTypeCommandValidator(IEntityReadRepository readRepository)
    {
        var descriptor = FgsSetupLaborRateTypeDescriptor.Create();
        RuleFor(x => x.EntityKey).Equal(descriptor.Key);
        RuleFor(x => x.Id).NotEmpty();
        CatalogEntityValidatorFactory.ApplyUpdateRules(this, descriptor, readRepository, x => x.Payload, x => x.Id);
    }
}

public sealed class PatchFgsSetupLaborRateTypeCommandValidator : AbstractValidator<PatchCatalogEntityCommand<FgsSetupLaborRateTypePatchDto, FgsSetupLaborRateTypeDetailDto>>
{
    public PatchFgsSetupLaborRateTypeCommandValidator()
    {
        var descriptor = FgsSetupLaborRateTypeDescriptor.Create();
        RuleFor(x => x.EntityKey).Equal(descriptor.Key);
        RuleFor(x => x.Id).NotEmpty();
        CatalogEntityValidatorFactory.ApplyPatchRules(this, descriptor, x => x.Payload);
    }
}
