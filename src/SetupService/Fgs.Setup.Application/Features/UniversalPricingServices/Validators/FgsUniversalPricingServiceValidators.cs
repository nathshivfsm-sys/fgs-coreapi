using Fgs.Setup.Application.Abstractions.UniversalPricingServices;
using Fgs.Setup.Application.Features.UniversalPricingServices.Commands.CreateFgsUniversalPricingService;
using Fgs.Setup.Application.Features.UniversalPricingServices.Commands.PatchFgsUniversalPricingService;
using Fgs.Setup.Application.Features.UniversalPricingServices.Commands.UpdateFgsUniversalPricingService;
using Fgs.Setup.Application.Features.UniversalPricingServices.Dtos;
using FluentValidation;

namespace Fgs.Setup.Application.Features.UniversalPricingServices.Validators;

public sealed class FgsUniversalMatrixTierItemDtoValidator : AbstractValidator<FgsUniversalMatrixTierItemDto>
{
    public FgsUniversalMatrixTierItemDtoValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Multiplier).GreaterThan(0m);
        RuleFor(x => x.DisplayOrder).GreaterThanOrEqualTo((short)1);
    }
}

public sealed class FgsUniversalMatrixSizeTierItemDtoValidator : AbstractValidator<FgsUniversalMatrixSizeTierItemDto>
{
    public FgsUniversalMatrixSizeTierItemDtoValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Multiplier).GreaterThan(0m);
        RuleFor(x => x.DisplayOrder).GreaterThanOrEqualTo((short)1);
    }
}

public sealed class FgsUniversalMatrixItemItemDtoValidator : AbstractValidator<FgsUniversalMatrixItemItemDto>
{
    public FgsUniversalMatrixItemItemDtoValidator()
    {
        RuleFor(x => x.ItemName).NotEmpty().MaximumLength(150);
        RuleFor(x => x.UnitType).NotEmpty().MaximumLength(50);
        RuleFor(x => x.BasePrice).GreaterThanOrEqualTo(0m);
        RuleFor(x => x.DisplayOrder).GreaterThanOrEqualTo((short)1);
    }
}

public sealed class FgsUniversalMatrixFrequencyDiscountItemDtoValidator : AbstractValidator<FgsUniversalMatrixFrequencyDiscountItemDto>
{
    public FgsUniversalMatrixFrequencyDiscountItemDtoValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
        RuleFor(x => x.DiscountPercent).InclusiveBetween(0m, 100m);
        RuleFor(x => x.DisplayOrder).GreaterThanOrEqualTo((short)1);
    }
}

public sealed class FgsUniversalMatrixOneTimeFeeItemDtoValidator : AbstractValidator<FgsUniversalMatrixOneTimeFeeItemDto>
{
    public FgsUniversalMatrixOneTimeFeeItemDtoValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(150);
        RuleFor(x => x.Amount).GreaterThanOrEqualTo(0m);
        RuleFor(x => x.DisplayOrder).GreaterThanOrEqualTo((short)1);
    }
}

public sealed class FgsUniversalMatrixAddOnItemDtoValidator : AbstractValidator<FgsUniversalMatrixAddOnItemDto>
{
    public FgsUniversalMatrixAddOnItemDtoValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(150);
        RuleFor(x => x.UnitType).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Price).GreaterThanOrEqualTo(0m);
        RuleFor(x => x.DisplayOrder).GreaterThanOrEqualTo((short)1);
    }
}

public sealed class CreateFgsUniversalPricingServiceCommandValidator : AbstractValidator<CreateFgsUniversalPricingServiceCommand>
{
    public CreateFgsUniversalPricingServiceCommandValidator(IFgsUniversalPricingServiceReadRepository readRepository)
    {
        RuleFor(x => x.Dto.UniversalPricingServiceCode).NotEmpty();
        RuleFor(x => x.Dto.UniversalPricingServiceCode).MaximumLength(50);
        RuleFor(x => x.Dto.UniversalPricingServiceCode)
            .Must(code => string.Equals(code, code.Trim().ToUpperInvariant(), StringComparison.Ordinal))
            .WithMessage("UniversalPricingServiceCode must be uppercase.");
        RuleFor(x => x.Dto.UniversalPricingServiceCode).MustAsync(async (_, code, cancellationToken) =>
                !await readRepository.ExistsByUniversalPricingServiceCodeAsync(code, null, cancellationToken))
            .WithMessage("A universal pricing service with this code already exists.");
        RuleFor(x => x.Dto.DisplayOrder).GreaterThanOrEqualTo((short)1);

        RuleForEach(x => x.Dto.Tiers).SetValidator(new FgsUniversalMatrixTierItemDtoValidator())
            .When(x => x.Dto.Tiers is not null);
        RuleForEach(x => x.Dto.SizeTiers).SetValidator(new FgsUniversalMatrixSizeTierItemDtoValidator())
            .When(x => x.Dto.SizeTiers is not null);
        RuleForEach(x => x.Dto.Items).SetValidator(new FgsUniversalMatrixItemItemDtoValidator())
            .When(x => x.Dto.Items is not null);
        RuleForEach(x => x.Dto.FrequencyDiscounts).SetValidator(new FgsUniversalMatrixFrequencyDiscountItemDtoValidator())
            .When(x => x.Dto.FrequencyDiscounts is not null);
        RuleForEach(x => x.Dto.OneTimeFees).SetValidator(new FgsUniversalMatrixOneTimeFeeItemDtoValidator())
            .When(x => x.Dto.OneTimeFees is not null);
        RuleForEach(x => x.Dto.AddOns).SetValidator(new FgsUniversalMatrixAddOnItemDtoValidator())
            .When(x => x.Dto.AddOns is not null);

        RuleFor(x => x.Dto.Tiers)
            .Must(HaveUniqueNames)
            .WithMessage("Tier names must be unique within the payload.")
            .When(x => x.Dto.Tiers is not null);
        RuleFor(x => x.Dto.SizeTiers)
            .Must(HaveUniqueNames)
            .WithMessage("Size tier names must be unique within the payload.")
            .When(x => x.Dto.SizeTiers is not null);
        RuleFor(x => x.Dto.Items)
            .Must(items => items!.Select(i => i.ItemName.Trim()).Where(n => n.Length > 0).GroupBy(n => n, StringComparer.OrdinalIgnoreCase).All(g => g.Count() == 1))
            .WithMessage("Item names must be unique within the payload.")
            .When(x => x.Dto.Items is not null);
        RuleFor(x => x.Dto.FrequencyDiscounts)
            .Must(HaveUniqueNames)
            .WithMessage("Frequency discount names must be unique within the payload.")
            .When(x => x.Dto.FrequencyDiscounts is not null);
        RuleFor(x => x.Dto.OneTimeFees)
            .Must(HaveUniqueNames)
            .WithMessage("One-time fee names must be unique within the payload.")
            .When(x => x.Dto.OneTimeFees is not null);
        RuleFor(x => x.Dto.AddOns)
            .Must(HaveUniqueNames)
            .WithMessage("Add-on names must be unique within the payload.")
            .When(x => x.Dto.AddOns is not null);
    }

    private static bool HaveUniqueNames(IReadOnlyList<FgsUniversalMatrixTierItemDto>? items) =>
        HaveUnique(items?.Select(i => i.Name));

    private static bool HaveUniqueNames(IReadOnlyList<FgsUniversalMatrixSizeTierItemDto>? items) =>
        HaveUnique(items?.Select(i => i.Name));

    private static bool HaveUniqueNames(IReadOnlyList<FgsUniversalMatrixFrequencyDiscountItemDto>? items) =>
        HaveUnique(items?.Select(i => i.Name));

    private static bool HaveUniqueNames(IReadOnlyList<FgsUniversalMatrixOneTimeFeeItemDto>? items) =>
        HaveUnique(items?.Select(i => i.Name));

    private static bool HaveUniqueNames(IReadOnlyList<FgsUniversalMatrixAddOnItemDto>? items) =>
        HaveUnique(items?.Select(i => i.Name));

    private static bool HaveUnique(IEnumerable<string>? names) =>
        names is null
        || names.Select(n => n.Trim()).Where(n => n.Length > 0)
            .GroupBy(n => n, StringComparer.OrdinalIgnoreCase).All(g => g.Count() == 1);
}

public sealed class UpdateFgsUniversalPricingServiceCommandValidator : AbstractValidator<UpdateFgsUniversalPricingServiceCommand>
{
    public UpdateFgsUniversalPricingServiceCommandValidator(IFgsUniversalPricingServiceReadRepository readRepository)
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Dto.UniversalPricingServiceCode).NotEmpty();
        RuleFor(x => x.Dto.UniversalPricingServiceCode).MaximumLength(50);
        RuleFor(x => x.Dto.UniversalPricingServiceCode)
            .Must(code => string.Equals(code, code.Trim().ToUpperInvariant(), StringComparison.Ordinal))
            .WithMessage("UniversalPricingServiceCode must be uppercase.");
        RuleFor(x => x.Dto.UniversalPricingServiceCode).MustAsync(async (command, code, cancellationToken) =>
                !await readRepository.ExistsByUniversalPricingServiceCodeAsync(code, command.Id, cancellationToken))
            .WithMessage("A universal pricing service with this code already exists.");
        RuleFor(x => x.Dto.DisplayOrder).GreaterThanOrEqualTo((short)1);

        RuleForEach(x => x.Dto.Tiers).SetValidator(new FgsUniversalMatrixTierItemDtoValidator())
            .When(x => x.Dto.Tiers is not null);
        RuleForEach(x => x.Dto.SizeTiers).SetValidator(new FgsUniversalMatrixSizeTierItemDtoValidator())
            .When(x => x.Dto.SizeTiers is not null);
        RuleForEach(x => x.Dto.Items).SetValidator(new FgsUniversalMatrixItemItemDtoValidator())
            .When(x => x.Dto.Items is not null);
        RuleForEach(x => x.Dto.FrequencyDiscounts).SetValidator(new FgsUniversalMatrixFrequencyDiscountItemDtoValidator())
            .When(x => x.Dto.FrequencyDiscounts is not null);
        RuleForEach(x => x.Dto.OneTimeFees).SetValidator(new FgsUniversalMatrixOneTimeFeeItemDtoValidator())
            .When(x => x.Dto.OneTimeFees is not null);
        RuleForEach(x => x.Dto.AddOns).SetValidator(new FgsUniversalMatrixAddOnItemDtoValidator())
            .When(x => x.Dto.AddOns is not null);

        RuleFor(x => x.Dto.Tiers)
            .Must(tiers => tiers!.Select(i => i.Name.Trim()).Where(n => n.Length > 0).GroupBy(n => n, StringComparer.OrdinalIgnoreCase).All(g => g.Count() == 1))
            .WithMessage("Tier names must be unique within the payload.")
            .When(x => x.Dto.Tiers is not null);
        RuleFor(x => x.Dto.SizeTiers)
            .Must(tiers => tiers!.Select(i => i.Name.Trim()).Where(n => n.Length > 0).GroupBy(n => n, StringComparer.OrdinalIgnoreCase).All(g => g.Count() == 1))
            .WithMessage("Size tier names must be unique within the payload.")
            .When(x => x.Dto.SizeTiers is not null);
        RuleFor(x => x.Dto.Items)
            .Must(items => items!.Select(i => i.ItemName.Trim()).Where(n => n.Length > 0).GroupBy(n => n, StringComparer.OrdinalIgnoreCase).All(g => g.Count() == 1))
            .WithMessage("Item names must be unique within the payload.")
            .When(x => x.Dto.Items is not null);
        RuleFor(x => x.Dto.FrequencyDiscounts)
            .Must(items => items!.Select(i => i.Name.Trim()).Where(n => n.Length > 0).GroupBy(n => n, StringComparer.OrdinalIgnoreCase).All(g => g.Count() == 1))
            .WithMessage("Frequency discount names must be unique within the payload.")
            .When(x => x.Dto.FrequencyDiscounts is not null);
        RuleFor(x => x.Dto.OneTimeFees)
            .Must(items => items!.Select(i => i.Name.Trim()).Where(n => n.Length > 0).GroupBy(n => n, StringComparer.OrdinalIgnoreCase).All(g => g.Count() == 1))
            .WithMessage("One-time fee names must be unique within the payload.")
            .When(x => x.Dto.OneTimeFees is not null);
        RuleFor(x => x.Dto.AddOns)
            .Must(items => items!.Select(i => i.Name.Trim()).Where(n => n.Length > 0).GroupBy(n => n, StringComparer.OrdinalIgnoreCase).All(g => g.Count() == 1))
            .WithMessage("Add-on names must be unique within the payload.")
            .When(x => x.Dto.AddOns is not null);
    }
}

public sealed class PatchFgsUniversalPricingServiceCommandValidator : AbstractValidator<PatchFgsUniversalPricingServiceCommand>
{
    public PatchFgsUniversalPricingServiceCommandValidator(IFgsUniversalPricingServiceReadRepository readRepository)
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Dto.UniversalPricingServiceCode).NotEmpty().When(x => x.Dto.UniversalPricingServiceCode is not null);
        RuleFor(x => x.Dto.UniversalPricingServiceCode).MaximumLength(50).When(x => x.Dto.UniversalPricingServiceCode is not null);
        RuleFor(x => x.Dto.UniversalPricingServiceCode)
            .Must(code => string.Equals(code!, code!.Trim().ToUpperInvariant(), StringComparison.Ordinal))
            .WithMessage("UniversalPricingServiceCode must be uppercase.")
            .When(x => x.Dto.UniversalPricingServiceCode is not null);
        RuleFor(x => x.Dto.UniversalPricingServiceCode).MustAsync(async (command, code, cancellationToken) =>
                !await readRepository.ExistsByUniversalPricingServiceCodeAsync(code!, command.Id, cancellationToken))
            .WithMessage("A universal pricing service with this code already exists.")
            .When(x => x.Dto.UniversalPricingServiceCode is not null);
        RuleFor(x => x.Dto.DisplayOrder).GreaterThanOrEqualTo((short)1).When(x => x.Dto.DisplayOrder.HasValue);
    }
}
