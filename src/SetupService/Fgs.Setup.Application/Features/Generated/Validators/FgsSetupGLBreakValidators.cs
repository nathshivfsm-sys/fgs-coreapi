using FluentValidation;
using Fgs.Foundation.CatalogCrud.Abstractions;
using Fgs.Foundation.CatalogCrud.Commands;
using Fgs.Foundation.CatalogCrud.Validation;
using Fgs.Setup.Application.Features.Generated.Descriptors;
using Fgs.Setup.Application.Features.Generated.Dtos;

namespace Fgs.Setup.Application.Features.Generated.Validators;

public sealed class CreateFgsSetupGLBreakCommandValidator : AbstractValidator<CreateCatalogEntityCommand<FgsSetupGLBreakCreateDto, FgsSetupGLBreakDetailDto>>
{
    public CreateFgsSetupGLBreakCommandValidator(IEntityReadRepository readRepository)
    {
        var descriptor = FgsSetupGLBreakDescriptor.Create();
        RuleFor(x => x.EntityKey).Equal(descriptor.Key);
        CatalogEntityValidatorFactory.ApplyCreateRules(this, descriptor, readRepository, x => x.Payload);
    }
}

public sealed class UpdateFgsSetupGLBreakCommandValidator : AbstractValidator<UpdateCatalogEntityCommand<FgsSetupGLBreakUpdateDto, FgsSetupGLBreakDetailDto>>
{
    public UpdateFgsSetupGLBreakCommandValidator(IEntityReadRepository readRepository)
    {
        var descriptor = FgsSetupGLBreakDescriptor.Create();
        RuleFor(x => x.EntityKey).Equal(descriptor.Key);
        RuleFor(x => x.Id).NotEmpty();
        CatalogEntityValidatorFactory.ApplyUpdateRules(this, descriptor, readRepository, x => x.Payload, x => x.Id);
    }
}

public sealed class PatchFgsSetupGLBreakCommandValidator : AbstractValidator<PatchCatalogEntityCommand<FgsSetupGLBreakPatchDto, FgsSetupGLBreakDetailDto>>
{
    public PatchFgsSetupGLBreakCommandValidator()
    {
        var descriptor = FgsSetupGLBreakDescriptor.Create();
        RuleFor(x => x.EntityKey).Equal(descriptor.Key);
        RuleFor(x => x.Id).NotEmpty();
        CatalogEntityValidatorFactory.ApplyPatchRules(this, descriptor, x => x.Payload);
    }
}
