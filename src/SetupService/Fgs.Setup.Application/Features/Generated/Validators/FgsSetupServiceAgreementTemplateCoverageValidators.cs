using FluentValidation;
using Fgs.Foundation.CatalogCrud.Abstractions;
using Fgs.Foundation.CatalogCrud.Commands;
using Fgs.Foundation.CatalogCrud.Validation;
using Fgs.Setup.Application.Features.Generated.Descriptors;
using Fgs.Setup.Application.Features.Generated.Dtos;

namespace Fgs.Setup.Application.Features.Generated.Validators;

public sealed class CreateFgsSetupServiceAgreementTemplateCoverageCommandValidator : AbstractValidator<CreateCatalogEntityCommand<FgsSetupServiceAgreementTemplateCoverageCreateDto, FgsSetupServiceAgreementTemplateCoverageDetailDto>>
{
    public CreateFgsSetupServiceAgreementTemplateCoverageCommandValidator(IEntityReadRepository readRepository)
    {
        var descriptor = FgsSetupServiceAgreementTemplateCoverageDescriptor.Create();
        RuleFor(x => x.EntityKey).Equal(descriptor.Key);
        CatalogEntityValidatorFactory.ApplyCreateRules(this, descriptor, readRepository, x => x.Payload);
    }
}

public sealed class UpdateFgsSetupServiceAgreementTemplateCoverageCommandValidator : AbstractValidator<UpdateCatalogEntityCommand<FgsSetupServiceAgreementTemplateCoverageUpdateDto, FgsSetupServiceAgreementTemplateCoverageDetailDto>>
{
    public UpdateFgsSetupServiceAgreementTemplateCoverageCommandValidator(IEntityReadRepository readRepository)
    {
        var descriptor = FgsSetupServiceAgreementTemplateCoverageDescriptor.Create();
        RuleFor(x => x.EntityKey).Equal(descriptor.Key);
        RuleFor(x => x.Id).NotEmpty();
        CatalogEntityValidatorFactory.ApplyUpdateRules(this, descriptor, readRepository, x => x.Payload, x => x.Id);
    }
}

public sealed class PatchFgsSetupServiceAgreementTemplateCoverageCommandValidator : AbstractValidator<PatchCatalogEntityCommand<FgsSetupServiceAgreementTemplateCoveragePatchDto, FgsSetupServiceAgreementTemplateCoverageDetailDto>>
{
    public PatchFgsSetupServiceAgreementTemplateCoverageCommandValidator()
    {
        var descriptor = FgsSetupServiceAgreementTemplateCoverageDescriptor.Create();
        RuleFor(x => x.EntityKey).Equal(descriptor.Key);
        RuleFor(x => x.Id).NotEmpty();
        CatalogEntityValidatorFactory.ApplyPatchRules(this, descriptor, x => x.Payload);
    }
}
