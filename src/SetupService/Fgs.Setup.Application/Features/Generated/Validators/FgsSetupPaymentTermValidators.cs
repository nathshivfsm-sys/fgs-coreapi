using FluentValidation;
using Fgs.Foundation.CatalogCrud.Abstractions;
using Fgs.Foundation.CatalogCrud.Commands;
using Fgs.Foundation.CatalogCrud.Validation;
using Fgs.Setup.Application.Features.Generated.Descriptors;
using Fgs.Setup.Application.Features.Generated.Dtos;

namespace Fgs.Setup.Application.Features.Generated.Validators;

public sealed class CreateFgsSetupPaymentTermCommandValidator : AbstractValidator<CreateCatalogEntityCommand<FgsSetupPaymentTermCreateDto, FgsSetupPaymentTermDetailDto>>
{
    public CreateFgsSetupPaymentTermCommandValidator(IEntityReadRepository readRepository)
    {
        var descriptor = FgsSetupPaymentTermDescriptor.Create();
        RuleFor(x => x.EntityKey).Equal(descriptor.Key);
        CatalogEntityValidatorFactory.ApplyCreateRules(this, descriptor, readRepository, x => x.Payload);
    }
}

public sealed class UpdateFgsSetupPaymentTermCommandValidator : AbstractValidator<UpdateCatalogEntityCommand<FgsSetupPaymentTermUpdateDto, FgsSetupPaymentTermDetailDto>>
{
    public UpdateFgsSetupPaymentTermCommandValidator(IEntityReadRepository readRepository)
    {
        var descriptor = FgsSetupPaymentTermDescriptor.Create();
        RuleFor(x => x.EntityKey).Equal(descriptor.Key);
        RuleFor(x => x.Id).NotEmpty();
        CatalogEntityValidatorFactory.ApplyUpdateRules(this, descriptor, readRepository, x => x.Payload, x => x.Id);
    }
}

public sealed class PatchFgsSetupPaymentTermCommandValidator : AbstractValidator<PatchCatalogEntityCommand<FgsSetupPaymentTermPatchDto, FgsSetupPaymentTermDetailDto>>
{
    public PatchFgsSetupPaymentTermCommandValidator()
    {
        var descriptor = FgsSetupPaymentTermDescriptor.Create();
        RuleFor(x => x.EntityKey).Equal(descriptor.Key);
        RuleFor(x => x.Id).NotEmpty();
        CatalogEntityValidatorFactory.ApplyPatchRules(this, descriptor, x => x.Payload);
    }
}
