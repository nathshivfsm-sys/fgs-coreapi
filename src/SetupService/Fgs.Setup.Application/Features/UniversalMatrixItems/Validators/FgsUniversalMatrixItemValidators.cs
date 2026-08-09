using Fgs.Setup.Application.Abstractions.UniversalMatrixItems;
using Fgs.Setup.Application.Features.UniversalMatrixItems.Commands.CreateFgsUniversalMatrixItem;
using Fgs.Setup.Application.Features.UniversalMatrixItems.Commands.PatchFgsUniversalMatrixItem;
using Fgs.Setup.Application.Features.UniversalMatrixItems.Commands.UpdateFgsUniversalMatrixItem;
using FluentValidation;

namespace Fgs.Setup.Application.Features.UniversalMatrixItems.Validators;

public sealed class CreateFgsUniversalMatrixItemCommandValidator : AbstractValidator<CreateFgsUniversalMatrixItemCommand>
{
    public CreateFgsUniversalMatrixItemCommandValidator(IFgsUniversalMatrixItemReadRepository readRepository)
    {
        RuleFor(x => x.Dto.UniversalPricingServiceId).GreaterThan(0);
        RuleFor(x => x.Dto.UniversalPricingServiceId).MustAsync(async (_, value, cancellationToken) =>
                await readRepository.ExistsUniversalPricingServiceIdAsync(value, cancellationToken))
            .WithMessage("The specified universal pricing service was not found.");
        RuleFor(x => x.Dto.ItemName).NotEmpty();
        RuleFor(x => x.Dto.ItemName).MaximumLength(150);
        RuleFor(x => x.Dto.UnitType).NotEmpty();
        RuleFor(x => x.Dto.UnitType).MaximumLength(50);
        RuleFor(x => x.Dto.BasePrice).GreaterThanOrEqualTo(0m);
        RuleFor(x => x.Dto.DisplayOrder).GreaterThanOrEqualTo((short)1);
        RuleFor(x => x.Dto.ItemName).MustAsync(async (command, value, cancellationToken) =>
                !await readRepository.ExistsByItemNameAsync(command.Dto.UniversalPricingServiceId, value, null, cancellationToken))
            .WithMessage("A universal matrix item with this itemname already exists for the universal pricing service.");
    }
}

public sealed class UpdateFgsUniversalMatrixItemCommandValidator : AbstractValidator<UpdateFgsUniversalMatrixItemCommand>
{
    public UpdateFgsUniversalMatrixItemCommandValidator(IFgsUniversalMatrixItemReadRepository readRepository)
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Dto.UniversalPricingServiceId).GreaterThan(0);
        RuleFor(x => x.Dto.UniversalPricingServiceId).MustAsync(async (_, value, cancellationToken) =>
                await readRepository.ExistsUniversalPricingServiceIdAsync(value, cancellationToken))
            .WithMessage("The specified universal pricing service was not found.");
        RuleFor(x => x.Dto.ItemName).NotEmpty();
        RuleFor(x => x.Dto.ItemName).MaximumLength(150);
        RuleFor(x => x.Dto.UnitType).NotEmpty();
        RuleFor(x => x.Dto.UnitType).MaximumLength(50);
        RuleFor(x => x.Dto.BasePrice).GreaterThanOrEqualTo(0m);
        RuleFor(x => x.Dto.DisplayOrder).GreaterThanOrEqualTo((short)1);
        RuleFor(x => x.Dto.ItemName).MustAsync(async (command, value, cancellationToken) =>
                !await readRepository.ExistsByItemNameAsync(command.Dto.UniversalPricingServiceId, value, command.Id, cancellationToken))
            .WithMessage("A universal matrix item with this itemname already exists for the universal pricing service.");
    }
}

public sealed class PatchFgsUniversalMatrixItemCommandValidator : AbstractValidator<PatchFgsUniversalMatrixItemCommand>
{
    public PatchFgsUniversalMatrixItemCommandValidator(IFgsUniversalMatrixItemReadRepository readRepository)
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Dto.UniversalPricingServiceId).GreaterThan(0).When(x => x.Dto.UniversalPricingServiceId.HasValue);
        RuleFor(x => x.Dto.UniversalPricingServiceId).MustAsync(async (_, value, cancellationToken) =>
                !value.HasValue || await readRepository.ExistsUniversalPricingServiceIdAsync(value.Value, cancellationToken))
            .WithMessage("The specified universal pricing service was not found.")
            .When(x => x.Dto.UniversalPricingServiceId.HasValue);
        RuleFor(x => x.Dto.ItemName).NotEmpty().When(x => x.Dto.ItemName is not null);
        RuleFor(x => x.Dto.ItemName).MaximumLength(150).When(x => x.Dto.ItemName is not null);
        RuleFor(x => x.Dto.UnitType).NotEmpty().When(x => x.Dto.UnitType is not null);
        RuleFor(x => x.Dto.UnitType).MaximumLength(50).When(x => x.Dto.UnitType is not null);
        RuleFor(x => x.Dto.BasePrice).GreaterThanOrEqualTo(0m).When(x => x.Dto.BasePrice.HasValue);
        RuleFor(x => x.Dto.DisplayOrder).GreaterThanOrEqualTo((short)1).When(x => x.Dto.DisplayOrder.HasValue);
    }
}
