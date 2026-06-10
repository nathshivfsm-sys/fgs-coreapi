using FluentValidation;
using Fgs.Foundation.CatalogCrud.Abstractions;
using Fgs.Foundation.CatalogCrud.Commands;
using Fgs.Foundation.CatalogCrud.Validation;
using Fgs.Setup.Application.Features.Generated.Descriptors;
using Fgs.Setup.Application.Features.Generated.Dtos;

namespace Fgs.Setup.Application.Features.Generated.Validators;

public sealed class CreateFgsSetupTimeSlotCommandValidator : AbstractValidator<CreateCatalogEntityCommand<FgsSetupTimeSlotCreateDto, FgsSetupTimeSlotDetailDto>>
{
    public CreateFgsSetupTimeSlotCommandValidator(IEntityReadRepository readRepository)
    {
        var descriptor = FgsSetupTimeSlotDescriptor.Create();
        RuleFor(x => x.EntityKey).Equal(descriptor.Key);
        CatalogEntityValidatorFactory.ApplyCreateRules(this, descriptor, readRepository, x => x.Payload);
    }
}

public sealed class UpdateFgsSetupTimeSlotCommandValidator : AbstractValidator<UpdateCatalogEntityCommand<FgsSetupTimeSlotUpdateDto, FgsSetupTimeSlotDetailDto>>
{
    public UpdateFgsSetupTimeSlotCommandValidator(IEntityReadRepository readRepository)
    {
        var descriptor = FgsSetupTimeSlotDescriptor.Create();
        RuleFor(x => x.EntityKey).Equal(descriptor.Key);
        RuleFor(x => x.Id).NotEmpty();
        CatalogEntityValidatorFactory.ApplyUpdateRules(this, descriptor, readRepository, x => x.Payload, x => x.Id);
    }
}

public sealed class PatchFgsSetupTimeSlotCommandValidator : AbstractValidator<PatchCatalogEntityCommand<FgsSetupTimeSlotPatchDto, FgsSetupTimeSlotDetailDto>>
{
    public PatchFgsSetupTimeSlotCommandValidator()
    {
        var descriptor = FgsSetupTimeSlotDescriptor.Create();
        RuleFor(x => x.EntityKey).Equal(descriptor.Key);
        RuleFor(x => x.Id).NotEmpty();
        CatalogEntityValidatorFactory.ApplyPatchRules(this, descriptor, x => x.Payload);
    }
}
