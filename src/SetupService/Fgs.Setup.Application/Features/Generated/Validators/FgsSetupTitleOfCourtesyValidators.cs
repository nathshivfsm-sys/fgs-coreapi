using FluentValidation;
using Fgs.Foundation.CatalogCrud.Abstractions;
using Fgs.Foundation.CatalogCrud.Commands;
using Fgs.Foundation.CatalogCrud.Validation;
using Fgs.Setup.Application.Features.Generated.Descriptors;
using Fgs.Setup.Application.Features.Generated.Dtos;

namespace Fgs.Setup.Application.Features.Generated.Validators;

public sealed class CreateFgsSetupTitleOfCourtesyCommandValidator : AbstractValidator<CreateCatalogEntityCommand<FgsSetupTitleOfCourtesyCreateDto, FgsSetupTitleOfCourtesyDetailDto>>
{
    public CreateFgsSetupTitleOfCourtesyCommandValidator(IEntityReadRepository readRepository)
    {
        var descriptor = FgsSetupTitleOfCourtesyDescriptor.Create();
        RuleFor(x => x.EntityKey).Equal(descriptor.Key);
        CatalogEntityValidatorFactory.ApplyCreateRules(this, descriptor, readRepository, x => x.Payload);
    }
}

public sealed class UpdateFgsSetupTitleOfCourtesyCommandValidator : AbstractValidator<UpdateCatalogEntityCommand<FgsSetupTitleOfCourtesyUpdateDto, FgsSetupTitleOfCourtesyDetailDto>>
{
    public UpdateFgsSetupTitleOfCourtesyCommandValidator(IEntityReadRepository readRepository)
    {
        var descriptor = FgsSetupTitleOfCourtesyDescriptor.Create();
        RuleFor(x => x.EntityKey).Equal(descriptor.Key);
        RuleFor(x => x.Id).NotEmpty();
        CatalogEntityValidatorFactory.ApplyUpdateRules(this, descriptor, readRepository, x => x.Payload, x => x.Id);
    }
}

public sealed class PatchFgsSetupTitleOfCourtesyCommandValidator : AbstractValidator<PatchCatalogEntityCommand<FgsSetupTitleOfCourtesyPatchDto, FgsSetupTitleOfCourtesyDetailDto>>
{
    public PatchFgsSetupTitleOfCourtesyCommandValidator()
    {
        var descriptor = FgsSetupTitleOfCourtesyDescriptor.Create();
        RuleFor(x => x.EntityKey).Equal(descriptor.Key);
        RuleFor(x => x.Id).NotEmpty();
        CatalogEntityValidatorFactory.ApplyPatchRules(this, descriptor, x => x.Payload);
    }
}
