using FluentValidation;
using Fgs.Foundation.CatalogCrud.Abstractions;
using Fgs.Foundation.CatalogCrud.Commands;
using Fgs.Foundation.CatalogCrud.Validation;
using Fgs.Setup.Application.Features.Generated.Descriptors;
using Fgs.Setup.Application.Features.Generated.Dtos;

namespace Fgs.Setup.Application.Features.Generated.Validators;

public sealed class CreateFgsVendorCommandValidator : AbstractValidator<CreateCatalogEntityCommand<FgsVendorCreateDto, FgsVendorDetailDto>>
{
    public CreateFgsVendorCommandValidator(IEntityReadRepository readRepository)
    {
        var descriptor = FgsVendorDescriptor.Create();
        RuleFor(x => x.EntityKey).Equal(descriptor.Key);
        CatalogEntityValidatorFactory.ApplyCreateRules(this, descriptor, readRepository, x => x.Payload);
    }
}

public sealed class UpdateFgsVendorCommandValidator : AbstractValidator<UpdateCatalogEntityCommand<FgsVendorUpdateDto, FgsVendorDetailDto>>
{
    public UpdateFgsVendorCommandValidator(IEntityReadRepository readRepository)
    {
        var descriptor = FgsVendorDescriptor.Create();
        RuleFor(x => x.EntityKey).Equal(descriptor.Key);
        RuleFor(x => x.Id).NotEmpty();
        CatalogEntityValidatorFactory.ApplyUpdateRules(this, descriptor, readRepository, x => x.Payload, x => x.Id);
    }
}

public sealed class PatchFgsVendorCommandValidator : AbstractValidator<PatchCatalogEntityCommand<FgsVendorPatchDto, FgsVendorDetailDto>>
{
    public PatchFgsVendorCommandValidator()
    {
        var descriptor = FgsVendorDescriptor.Create();
        RuleFor(x => x.EntityKey).Equal(descriptor.Key);
        RuleFor(x => x.Id).NotEmpty();
        CatalogEntityValidatorFactory.ApplyPatchRules(this, descriptor, x => x.Payload);
    }
}
