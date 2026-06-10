using FluentValidation;
using Fgs.Foundation.CatalogCrud.Abstractions;
using Fgs.Foundation.CatalogCrud.Commands;
using Fgs.Foundation.CatalogCrud.Validation;
using Fgs.Setup.Application.Features.Generated.Descriptors;
using Fgs.Setup.Application.Features.Generated.Dtos;

namespace Fgs.Setup.Application.Features.Generated.Validators;

public sealed class CreateFgsTagCommandValidator : AbstractValidator<CreateCatalogEntityCommand<FgsTagCreateDto, FgsTagDetailDto>>
{
    public CreateFgsTagCommandValidator(IEntityReadRepository readRepository)
    {
        var descriptor = FgsTagDescriptor.Create();
        RuleFor(x => x.EntityKey).Equal(descriptor.Key);
        CatalogEntityValidatorFactory.ApplyCreateRules(this, descriptor, readRepository, x => x.Payload);
    }
}

public sealed class UpdateFgsTagCommandValidator : AbstractValidator<UpdateCatalogEntityCommand<FgsTagUpdateDto, FgsTagDetailDto>>
{
    public UpdateFgsTagCommandValidator(IEntityReadRepository readRepository)
    {
        var descriptor = FgsTagDescriptor.Create();
        RuleFor(x => x.EntityKey).Equal(descriptor.Key);
        RuleFor(x => x.Id).NotEmpty();
        CatalogEntityValidatorFactory.ApplyUpdateRules(this, descriptor, readRepository, x => x.Payload, x => x.Id);
    }
}

public sealed class PatchFgsTagCommandValidator : AbstractValidator<PatchCatalogEntityCommand<FgsTagPatchDto, FgsTagDetailDto>>
{
    public PatchFgsTagCommandValidator()
    {
        var descriptor = FgsTagDescriptor.Create();
        RuleFor(x => x.EntityKey).Equal(descriptor.Key);
        RuleFor(x => x.Id).NotEmpty();
        CatalogEntityValidatorFactory.ApplyPatchRules(this, descriptor, x => x.Payload);
    }
}
