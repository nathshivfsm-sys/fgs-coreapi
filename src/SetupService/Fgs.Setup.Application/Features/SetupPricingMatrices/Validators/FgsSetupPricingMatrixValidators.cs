using Fgs.Setup.Application.Abstractions.BillingCategories;
using Fgs.Setup.Application.Abstractions.SetupLaborRateTypes;
using Fgs.Setup.Application.Abstractions.SetupPricingMatrices;
using Fgs.Setup.Application.Abstractions.SetupTechSkillLevels;
using Fgs.Setup.Application.Features.SetupPricingMatrices.Commands.CreateFgsSetupPricingMatrix;
using Fgs.Setup.Application.Features.SetupPricingMatrices.Commands.PatchFgsSetupPricingMatrix;
using Fgs.Setup.Application.Features.SetupPricingMatrices.Commands.UpdateFgsSetupPricingMatrix;
using Fgs.Setup.Application.Features.SetupPricingMatrices.Dtos;
using FluentValidation;

namespace Fgs.Setup.Application.Features.SetupPricingMatrices.Validators;

internal static class FgsSetupPricingMatrixValidationRules
{
    public static void ApplyWriteRules<T>(
        AbstractValidator<T> validator,
        Func<T, FgsSetupPricingMatrixWriteDto> dtoSelector,
        IFgsSetupPricingMatrixReadRepository readRepository,
        IFgsSetupLaborRateTypeReadRepository laborRateTypeReadRepository,
        IFgsSetupTechSkillLevelReadRepository techSkillLevelReadRepository,
        IBillingCategoryReadRepository billingCategoryReadRepository,
        Func<T, long?>? excludeIdSelector = null)
    {
        validator.RuleFor(x => dtoSelector(x).Name)
            .NotEmpty()
            .MaximumLength(50);
        validator.RuleFor(x => dtoSelector(x).Name)
            .Must(code => string.Equals(code, code.Trim().ToUpperInvariant(), StringComparison.Ordinal))
            .WithMessage("Name (code) must be uppercase.");
        validator.RuleFor(x => dtoSelector(x).Description)
            .NotEmpty()
            .MaximumLength(200);

        validator.RuleFor(x => dtoSelector(x))
            .MustAsync(async (command, dto, cancellationToken) =>
            {
                var excludeId = excludeIdSelector?.Invoke(command);
                return !await readRepository.ExistsByCodeAsync(dto.Name, excludeId, cancellationToken);
            })
            .WithMessage("A pricing matrix with this code already exists.");

        validator.RuleFor(x => dtoSelector(x))
            .Must(dto => !HasMaterialTiers(dto.MaterialTiers) || !HasOtherItems(dto.OtherItems))
            .WithMessage("MaterialTiers and OtherItems are mutually exclusive.");

        validator.RuleFor(x => dtoSelector(x))
            .Must(dto => !HasMarkup(dto) || dto.PriceAdjustmentTypeId is >= 1 and <= 3)
            .WithMessage("PriceAdjustmentTypeId is required and must be between 1 and 3 when markup items are provided.");

        validator.RuleFor(x => dtoSelector(x))
            .Must(dto => dto.EffectiveTo is null || dto.EffectiveFrom is null || dto.EffectiveTo >= dto.EffectiveFrom)
            .WithMessage("EffectiveTo must be greater than or equal to EffectiveFrom.");

        validator.RuleForEach(x => dtoSelector(x).MaterialTiers)
            .ChildRules(tier =>
            {
                tier.RuleFor(t => t!.FromCost).GreaterThanOrEqualTo(0);
                tier.RuleFor(t => t!.AdjustmentValue).GreaterThanOrEqualTo(0);
                tier.RuleFor(t => t)
                    .Must(t => t!.ToCost is null || t.ToCost >= t.FromCost)
                    .WithMessage("ToCost must be greater than or equal to FromCost.");
            })
            .When(x => HasMaterialTiers(dtoSelector(x).MaterialTiers));

        validator.RuleFor(x => dtoSelector(x).MaterialTiers)
            .Must(tiers => tiers!.Select(t => t.FromCost).Distinct().Count() == tiers!.Count)
            .WithMessage("Duplicate FromCost values are not allowed within MaterialTiers.")
            .When(x => HasMaterialTiers(dtoSelector(x).MaterialTiers));

        validator.RuleForEach(x => dtoSelector(x).OtherItems)
            .ChildRules(item =>
            {
                item.RuleFor(i => i!.CategoryCode)
                    .NotEmpty()
                    .MaximumLength(2)
                    .Must(code => string.Equals(code, code.Trim().ToUpperInvariant(), StringComparison.Ordinal))
                    .WithMessage("CategoryCode must be uppercase.");
                item.RuleFor(i => i!.Name).NotEmpty().MaximumLength(200);
                item.RuleFor(i => i!.AdjustmentValue).GreaterThanOrEqualTo(0).When(i => i!.AdjustmentValue.HasValue);
                item.RuleFor(i => i!.DiscountPercent).InclusiveBetween(0, 100).When(i => i!.DiscountPercent.HasValue);
            })
            .When(x => HasOtherItems(dtoSelector(x).OtherItems));

        validator.RuleFor(x => dtoSelector(x))
            .MustAsync(async (command, dto, cancellationToken) =>
                await ValidateOtherItemsAsync(dto, billingCategoryReadRepository, cancellationToken))
            .WithMessage("Other item validation failed.")
            .When(x => HasOtherItems(dtoSelector(x).OtherItems));

        validator.RuleFor(x => dtoSelector(x).OtherItems)
            .Must(items => items!.Select(i => i.CategoryCode.Trim().ToUpperInvariant()).Distinct().Count() == items!.Count)
            .WithMessage("Duplicate CategoryCode values are not allowed within OtherItems.")
            .When(x => HasOtherItems(dtoSelector(x).OtherItems));

        validator.RuleFor(x => dtoSelector(x))
            .MustAsync(async (command, dto, cancellationToken) =>
                await ValidateLaborLinesAsync(
                    dto,
                    laborRateTypeReadRepository,
                    techSkillLevelReadRepository,
                    cancellationToken))
            .WithMessage("Labor line validation failed.");
    }

    private static bool HasMaterialTiers(IReadOnlyList<FgsSetupPricingMatrixMaterialTierDto>? materialTiers) =>
        materialTiers is { Count: > 0 };

    private static bool HasOtherItems(IReadOnlyList<FgsSetupPricingMatrixOtherItemDto>? otherItems) =>
        otherItems is { Count: > 0 };

    private static bool HasMarkup(FgsSetupPricingMatrixWriteDto dto) =>
        HasMaterialTiers(dto.MaterialTiers) || HasOtherItems(dto.OtherItems);

    private static async Task<bool> ValidateLaborLinesAsync(
        FgsSetupPricingMatrixWriteDto dto,
        IFgsSetupLaborRateTypeReadRepository laborRateTypeReadRepository,
        IFgsSetupTechSkillLevelReadRepository techSkillLevelReadRepository,
        CancellationToken cancellationToken)
    {
        foreach (var line in dto.LaborLines ?? [])
        {
            if (await laborRateTypeReadRepository.GetByIdAsync(line.LaborRateTypeId, cancellationToken) is null)
            {
                return false;
            }

            if (!dto.IsLaborRateBySkillLevel && line.TechSkillLevelId.HasValue)
            {
                return false;
            }

            if (dto.IsLaborRateBySkillLevel && !line.TechSkillLevelId.HasValue)
            {
                return false;
            }

            if (line.TechSkillLevelId.HasValue &&
                await techSkillLevelReadRepository.GetByIdAsync(line.TechSkillLevelId.Value, cancellationToken) is null)
            {
                return false;
            }

            if (dto.IsLaborTierStructure)
            {
                if (line.Tiers is not { Count: > 0 })
                {
                    return false;
                }

                if (line.Tiers.Select(t => t.SequenceOrder).Distinct().Count() != line.Tiers.Count)
                {
                    return false;
                }

                foreach (var tier in line.Tiers)
                {
                    if (tier.DurationMinutes <= 0 || tier.Rate < 0)
                    {
                        return false;
                    }

                    if (!dto.IsLaborRateBySkillLevel && tier.TechSkillLevelId.HasValue)
                    {
                        return false;
                    }

                    if (tier.TechSkillLevelId.HasValue &&
                        await techSkillLevelReadRepository.GetByIdAsync(tier.TechSkillLevelId.Value, cancellationToken) is null)
                    {
                        return false;
                    }
                }
            }
            else
            {
                if (line.Tiers is { Count: > 0 })
                {
                    return false;
                }

                if (line.BaseRate is null or < 0)
                {
                    return false;
                }

                if (line.OvertimeMultiplier is < 1)
                {
                    return false;
                }

                if (line.DoubleTimeMultiplier is < 1)
                {
                    return false;
                }

                if (line.DiscountPercent is < 0 or > 100)
                {
                    return false;
                }
            }
        }

        return true;
    }

    private static async Task<bool> ValidateOtherItemsAsync(
        FgsSetupPricingMatrixWriteDto dto,
        IBillingCategoryReadRepository billingCategoryReadRepository,
        CancellationToken cancellationToken)
    {
        foreach (var item in dto.OtherItems ?? [])
        {
            if (!await billingCategoryReadRepository.ExistsByBillingCategoryTypeAsync(
                    item.CategoryCode,
                    activeOnly: true,
                    cancellationToken))
            {
                return false;
            }
        }

        return true;
    }
}

public sealed class CreateFgsSetupPricingMatrixCommandValidator : AbstractValidator<CreateFgsSetupPricingMatrixCommand>
{
    public CreateFgsSetupPricingMatrixCommandValidator(
        IFgsSetupPricingMatrixReadRepository readRepository,
        IFgsSetupLaborRateTypeReadRepository laborRateTypeReadRepository,
        IFgsSetupTechSkillLevelReadRepository techSkillLevelReadRepository,
        IBillingCategoryReadRepository billingCategoryReadRepository)
    {
        FgsSetupPricingMatrixValidationRules.ApplyWriteRules(
            this,
            command => new FgsSetupPricingMatrixWriteDto(
                command.Dto.Name,
                command.Dto.Description,
                command.Dto.IsDefault,
                command.Dto.IsLaborTierStructure,
                command.Dto.IsLaborRateBySkillLevel,
                command.Dto.PriceAdjustmentTypeId,
                command.Dto.EffectiveFrom,
                command.Dto.EffectiveTo,
                command.Dto.IsMobileVisible,
                command.Dto.LaborLines,
                command.Dto.MaterialTiers,
                command.Dto.OtherItems),
            readRepository,
            laborRateTypeReadRepository,
            techSkillLevelReadRepository,
            billingCategoryReadRepository);
    }
}

public sealed class UpdateFgsSetupPricingMatrixCommandValidator : AbstractValidator<UpdateFgsSetupPricingMatrixCommand>
{
    public UpdateFgsSetupPricingMatrixCommandValidator(
        IFgsSetupPricingMatrixReadRepository readRepository,
        IFgsSetupLaborRateTypeReadRepository laborRateTypeReadRepository,
        IFgsSetupTechSkillLevelReadRepository techSkillLevelReadRepository,
        IBillingCategoryReadRepository billingCategoryReadRepository)
    {
        RuleFor(x => x.Id).GreaterThan(0);

        FgsSetupPricingMatrixValidationRules.ApplyWriteRules(
            this,
            command => new FgsSetupPricingMatrixWriteDto(
                command.Dto.Name,
                command.Dto.Description,
                command.Dto.IsDefault,
                command.Dto.IsLaborTierStructure,
                command.Dto.IsLaborRateBySkillLevel,
                command.Dto.PriceAdjustmentTypeId,
                command.Dto.EffectiveFrom,
                command.Dto.EffectiveTo,
                command.Dto.IsMobileVisible,
                command.Dto.LaborLines,
                command.Dto.MaterialTiers,
                command.Dto.OtherItems),
            readRepository,
            laborRateTypeReadRepository,
            techSkillLevelReadRepository,
            billingCategoryReadRepository,
            command => command.Id);
    }
}

public sealed class PatchFgsSetupPricingMatrixCommandValidator : AbstractValidator<PatchFgsSetupPricingMatrixCommand>
{
    public PatchFgsSetupPricingMatrixCommandValidator(IFgsSetupPricingMatrixReadRepository readRepository)
    {
        RuleFor(x => x.Id).GreaterThan(0);

        RuleFor(x => x.Dto.Name)
            .NotEmpty()
            .MaximumLength(50)
            .When(x => x.Dto.Name is not null);
        RuleFor(x => x.Dto.Name)
            .Must(code => string.Equals(code!, code!.Trim().ToUpperInvariant(), StringComparison.Ordinal))
            .When(x => x.Dto.Name is not null)
            .WithMessage("Name (code) must be uppercase.");
        RuleFor(x => x.Dto.Description)
            .NotEmpty()
            .MaximumLength(200)
            .When(x => x.Dto.Description is not null);
        RuleFor(x => x.Dto.PriceAdjustmentTypeId)
            .InclusiveBetween((short)1, (short)3)
            .When(x => x.Dto.PriceAdjustmentTypeId.HasValue);
        RuleFor(x => x)
            .MustAsync(async (command, cancellationToken) =>
                command.Dto.Name is null ||
                !await readRepository.ExistsByCodeAsync(command.Dto.Name, command.Id, cancellationToken))
            .WithMessage("A pricing matrix with this code already exists.");
    }
}
