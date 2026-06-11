using FluentValidation;
using Fgs.Foundation.CatalogCrud.Abstractions;
using Fgs.Foundation.CatalogCrud.Commands;
using Fgs.Foundation.CatalogCrud.Validation;
using Fgs.Setup.Application.Features.Generated.Descriptors;
using Fgs.Setup.Application.Features.Generated.Dtos;

namespace Fgs.Setup.Application.Features.Generated.Validators;

public sealed class CreateFgsJobTypeCommandValidator : AbstractValidator<CreateCatalogEntityCommand<FgsJobTypeCreateDto, FgsJobTypeDetailDto>>
{
    public CreateFgsJobTypeCommandValidator(IEntityReadRepository readRepository)
    {
        var descriptor = FgsJobTypeDescriptor.Create();
        RuleFor(x => x.EntityKey).Equal(descriptor.Key);
        CatalogEntityValidatorFactory.ApplyCreateRules(this, descriptor, readRepository, x => x.Payload);
    }
}

public sealed class UpdateFgsJobTypeCommandValidator : AbstractValidator<UpdateCatalogEntityCommand<FgsJobTypeUpdateDto, FgsJobTypeDetailDto>>
{
    public UpdateFgsJobTypeCommandValidator(IEntityReadRepository readRepository)
    {
        var descriptor = FgsJobTypeDescriptor.Create();
        RuleFor(x => x.EntityKey).Equal(descriptor.Key);
        RuleFor(x => x.Id).NotEmpty();
        CatalogEntityValidatorFactory.ApplyUpdateRules(this, descriptor, readRepository, x => x.Payload, x => x.Id);
    }
}

public sealed class PatchFgsJobTypeCommandValidator : AbstractValidator<PatchCatalogEntityCommand<FgsJobTypePatchDto, FgsJobTypeDetailDto>>
{
    public PatchFgsJobTypeCommandValidator()
    {
        var descriptor = FgsJobTypeDescriptor.Create();
        RuleFor(x => x.EntityKey).Equal(descriptor.Key);
        RuleFor(x => x.Id).NotEmpty();
        CatalogEntityValidatorFactory.ApplyPatchRules(this, descriptor, x => x.Payload);
    }
}
