using FluentValidation;
using Fgs.Foundation.CatalogCrud.Abstractions;
using Fgs.Foundation.CatalogCrud.Commands;
using Fgs.Foundation.CatalogCrud.Validation;
using Fgs.Setup.Application.Features.Generated.Descriptors;
using Fgs.Setup.Application.Features.Generated.Dtos;

namespace Fgs.Setup.Application.Features.Generated.Validators;

public sealed class CreateFgsSetupPaymentMethodCommandValidator : AbstractValidator<CreateCatalogEntityCommand<FgsSetupPaymentMethodCreateDto, FgsSetupPaymentMethodDetailDto>>
{
    public CreateFgsSetupPaymentMethodCommandValidator(IEntityReadRepository readRepository)
    {
        var descriptor = FgsSetupPaymentMethodDescriptor.Create();
        RuleFor(x => x.EntityKey).Equal(descriptor.Key);
        CatalogEntityValidatorFactory.ApplyCreateRules(this, descriptor, readRepository, x => x.Payload);
    }
}

public sealed class UpdateFgsSetupPaymentMethodCommandValidator : AbstractValidator<UpdateCatalogEntityCommand<FgsSetupPaymentMethodUpdateDto, FgsSetupPaymentMethodDetailDto>>
{
    public UpdateFgsSetupPaymentMethodCommandValidator(IEntityReadRepository readRepository)
    {
        var descriptor = FgsSetupPaymentMethodDescriptor.Create();
        RuleFor(x => x.EntityKey).Equal(descriptor.Key);
        RuleFor(x => x.Id).NotEmpty();
        CatalogEntityValidatorFactory.ApplyUpdateRules(this, descriptor, readRepository, x => x.Payload, x => x.Id);
    }
}

public sealed class PatchFgsSetupPaymentMethodCommandValidator : AbstractValidator<PatchCatalogEntityCommand<FgsSetupPaymentMethodPatchDto, FgsSetupPaymentMethodDetailDto>>
{
    public PatchFgsSetupPaymentMethodCommandValidator()
    {
        var descriptor = FgsSetupPaymentMethodDescriptor.Create();
        RuleFor(x => x.EntityKey).Equal(descriptor.Key);
        RuleFor(x => x.Id).NotEmpty();
        CatalogEntityValidatorFactory.ApplyPatchRules(this, descriptor, x => x.Payload);
    }
}
