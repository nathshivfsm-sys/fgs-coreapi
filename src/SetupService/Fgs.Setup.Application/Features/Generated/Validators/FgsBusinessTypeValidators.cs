using FluentValidation;
using Fgs.Foundation.CatalogCrud.Abstractions;
using Fgs.Foundation.CatalogCrud.Commands;
using Fgs.Foundation.CatalogCrud.Validation;
using Fgs.Setup.Application.Features.Generated.Descriptors;
using Fgs.Setup.Application.Features.Generated.Dtos;

namespace Fgs.Setup.Application.Features.Generated.Validators;

public sealed class CreateFgsBusinessTypeCommandValidator : AbstractValidator<CreateCatalogEntityCommand<FgsBusinessTypeCreateDto, FgsBusinessTypeDetailDto>>
{
    public CreateFgsBusinessTypeCommandValidator(IEntityReadRepository readRepository)
    {
        var descriptor = FgsBusinessTypeDescriptor.Create();
        RuleFor(x => x.EntityKey).Equal(descriptor.Key);
        CatalogEntityValidatorFactory.ApplyCreateRules(this, descriptor, readRepository, x => x.Payload);
    }
}

public sealed class UpdateFgsBusinessTypeCommandValidator : AbstractValidator<UpdateCatalogEntityCommand<FgsBusinessTypeUpdateDto, FgsBusinessTypeDetailDto>>
{
    public UpdateFgsBusinessTypeCommandValidator(IEntityReadRepository readRepository)
    {
        var descriptor = FgsBusinessTypeDescriptor.Create();
        RuleFor(x => x.EntityKey).Equal(descriptor.Key);
        RuleFor(x => x.Id).NotEmpty();
        CatalogEntityValidatorFactory.ApplyUpdateRules(this, descriptor, readRepository, x => x.Payload, x => x.Id);
    }
}

public sealed class PatchFgsBusinessTypeCommandValidator : AbstractValidator<PatchCatalogEntityCommand<FgsBusinessTypePatchDto, FgsBusinessTypeDetailDto>>
{
    public PatchFgsBusinessTypeCommandValidator()
    {
        var descriptor = FgsBusinessTypeDescriptor.Create();
        RuleFor(x => x.EntityKey).Equal(descriptor.Key);
        RuleFor(x => x.Id).NotEmpty();
        CatalogEntityValidatorFactory.ApplyPatchRules(this, descriptor, x => x.Payload);
    }
}
