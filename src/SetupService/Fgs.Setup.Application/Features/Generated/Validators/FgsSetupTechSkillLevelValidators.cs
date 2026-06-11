using FluentValidation;
using Fgs.Foundation.CatalogCrud.Abstractions;
using Fgs.Foundation.CatalogCrud.Commands;
using Fgs.Foundation.CatalogCrud.Validation;
using Fgs.Setup.Application.Features.Generated.Descriptors;
using Fgs.Setup.Application.Features.Generated.Dtos;

namespace Fgs.Setup.Application.Features.Generated.Validators;

public sealed class CreateFgsSetupTechSkillLevelCommandValidator : AbstractValidator<CreateCatalogEntityCommand<FgsSetupTechSkillLevelCreateDto, FgsSetupTechSkillLevelDetailDto>>
{
    public CreateFgsSetupTechSkillLevelCommandValidator(IEntityReadRepository readRepository)
    {
        var descriptor = FgsSetupTechSkillLevelDescriptor.Create();
        RuleFor(x => x.EntityKey).Equal(descriptor.Key);
        CatalogEntityValidatorFactory.ApplyCreateRules(this, descriptor, readRepository, x => x.Payload);
    }
}

public sealed class UpdateFgsSetupTechSkillLevelCommandValidator : AbstractValidator<UpdateCatalogEntityCommand<FgsSetupTechSkillLevelUpdateDto, FgsSetupTechSkillLevelDetailDto>>
{
    public UpdateFgsSetupTechSkillLevelCommandValidator(IEntityReadRepository readRepository)
    {
        var descriptor = FgsSetupTechSkillLevelDescriptor.Create();
        RuleFor(x => x.EntityKey).Equal(descriptor.Key);
        RuleFor(x => x.Id).NotEmpty();
        CatalogEntityValidatorFactory.ApplyUpdateRules(this, descriptor, readRepository, x => x.Payload, x => x.Id);
    }
}

public sealed class PatchFgsSetupTechSkillLevelCommandValidator : AbstractValidator<PatchCatalogEntityCommand<FgsSetupTechSkillLevelPatchDto, FgsSetupTechSkillLevelDetailDto>>
{
    public PatchFgsSetupTechSkillLevelCommandValidator()
    {
        var descriptor = FgsSetupTechSkillLevelDescriptor.Create();
        RuleFor(x => x.EntityKey).Equal(descriptor.Key);
        RuleFor(x => x.Id).NotEmpty();
        CatalogEntityValidatorFactory.ApplyPatchRules(this, descriptor, x => x.Payload);
    }
}
