using FluentValidation;
using Fgs.Foundation.CatalogCrud.Abstractions;
using Fgs.Foundation.CatalogCrud.Commands;
using Fgs.Foundation.CatalogCrud.Validation;
using Fgs.Setup.Application.Features.Generated.Descriptors;
using Fgs.Setup.Application.Features.Generated.Dtos;

namespace Fgs.Setup.Application.Features.Generated.Validators;

public sealed class CreateFgsSalesPipelineStatusCommandValidator : AbstractValidator<CreateCatalogEntityCommand<FgsSalesPipelineStatusCreateDto, FgsSalesPipelineStatusDetailDto>>
{
    public CreateFgsSalesPipelineStatusCommandValidator(IEntityReadRepository readRepository)
    {
        var descriptor = FgsSalesPipelineStatusDescriptor.Create();
        RuleFor(x => x.EntityKey).Equal(descriptor.Key);
        CatalogEntityValidatorFactory.ApplyCreateRules(this, descriptor, readRepository, x => x.Payload);
    }
}

public sealed class UpdateFgsSalesPipelineStatusCommandValidator : AbstractValidator<UpdateCatalogEntityCommand<FgsSalesPipelineStatusUpdateDto, FgsSalesPipelineStatusDetailDto>>
{
    public UpdateFgsSalesPipelineStatusCommandValidator(IEntityReadRepository readRepository)
    {
        var descriptor = FgsSalesPipelineStatusDescriptor.Create();
        RuleFor(x => x.EntityKey).Equal(descriptor.Key);
        RuleFor(x => x.Id).NotEmpty();
        CatalogEntityValidatorFactory.ApplyUpdateRules(this, descriptor, readRepository, x => x.Payload, x => x.Id);
    }
}

public sealed class PatchFgsSalesPipelineStatusCommandValidator : AbstractValidator<PatchCatalogEntityCommand<FgsSalesPipelineStatusPatchDto, FgsSalesPipelineStatusDetailDto>>
{
    public PatchFgsSalesPipelineStatusCommandValidator()
    {
        var descriptor = FgsSalesPipelineStatusDescriptor.Create();
        RuleFor(x => x.EntityKey).Equal(descriptor.Key);
        RuleFor(x => x.Id).NotEmpty();
        CatalogEntityValidatorFactory.ApplyPatchRules(this, descriptor, x => x.Payload);
    }
}
