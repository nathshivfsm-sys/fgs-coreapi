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
            .CustomAsync(async (dto, context, cancellationToken) =>
            {
                if (!HasOtherItems(dto.OtherItems))
                {
                    return;
                }

                var error = await ValidateOtherItemsAsync(dto, billingCategoryReadRepository, cancellationToken);
                if (error is not null)
                {
                    context.AddFailure(error);
                }
            });

        validator.RuleFor(x => dtoSelector(x).OtherItems)
            .Must(items => items!.Select(i => i.CategoryCode.Trim().ToUpperInvariant()).Distinct().Count() == items!.Count)
            .WithMessage("Duplicate CategoryCode values are not allowed within OtherItems.")
            .When(x => HasOtherItems(dtoSelector(x).OtherItems));

        validator.RuleFor(x => dtoSelector(x))
            .CustomAsync(async (dto, context, cancellationToken) =>
            {
                var error = await ValidateLaborLinesAsync(
                    dto,
                    laborRateTypeReadRepository,
                    techSkillLevelReadRepository,
                    cancellationToken);
                if (error is not null)
                {
                    context.AddFailure(error);
                }
            });
    }

    private static bool HasMaterialTiers(IReadOnlyList<FgsSetupPricingMatrixMaterialTierDto>? materialTiers) =>
        materialTiers is { Count: > 0 };

    private static bool HasOtherItems(IReadOnlyList<FgsSetupPricingMatrixOtherItemDto>? otherItems) =>
        otherItems is { Count: > 0 };

    private static bool HasMarkup(FgsSetupPricingMatrixWriteDto dto) =>
        HasMaterialTiers(dto.MaterialTiers) || HasOtherItems(dto.OtherItems);

    private static async Task<string?> ValidateLaborLinesAsync(
        FgsSetupPricingMatrixWriteDto dto,
        IFgsSetupLaborRateTypeReadRepository laborRateTypeReadRepository,
        IFgsSetupTechSkillLevelReadRepository techSkillLevelReadRepository,
        CancellationToken cancellationToken)
    {
        var lineIndex = 0;
        foreach (var line in dto.LaborLines ?? [])
        {
            var lineLabel = $"LaborLines[{lineIndex}]";
            lineIndex++;

            if (await laborRateTypeReadRepository.GetByIdAsync(line.LaborRateTypeId, cancellationToken) is null)
            {
                return $"{lineLabel}.LaborRateTypeId '{line.LaborRateTypeId}' was not found. Call GET /api/v1/laborratetype/lookup and use a real id.";
            }

            if (!dto.IsLaborRateBySkillLevel && line.TechSkillLevelId.HasValue)
            {
                return $"{lineLabel}.TechSkillLevelId must be null when IsLaborRateBySkillLevel is false.";
            }

            if (dto.IsLaborRateBySkillLevel && !line.TechSkillLevelId.HasValue)
            {
                return $"{lineLabel}.TechSkillLevelId is required when IsLaborRateBySkillLevel is true.";
            }

            if (line.TechSkillLevelId.HasValue &&
                await techSkillLevelReadRepository.GetByIdAsync(line.TechSkillLevelId.Value, cancellationToken) is null)
            {
                return $"{lineLabel}.TechSkillLevelId '{line.TechSkillLevelId.Value}' was not found.";
            }

            if (dto.IsLaborTierStructure)
            {
                if (line.Tiers is not { Count: > 0 })
                {
                    return $"{lineLabel}.Tiers is required when IsLaborTierStructure is true.";
                }

                if (line.Tiers.Select(t => t.SequenceOrder).Distinct().Count() != line.Tiers.Count)
                {
                    return $"{lineLabel}.Tiers contains duplicate SequenceOrder values.";
                }

                var tierIndex = 0;
                foreach (var tier in line.Tiers)
                {
                    var tierLabel = $"{lineLabel}.Tiers[{tierIndex}]";
                    tierIndex++;

                    if (tier.DurationMinutes <= 0)
                    {
                        return $"{tierLabel}.DurationMinutes must be greater than 0.";
                    }

                    if (tier.Rate < 0)
                    {
                        return $"{tierLabel}.Rate must be greater than or equal to 0.";
                    }

                    if (!dto.IsLaborRateBySkillLevel && tier.TechSkillLevelId.HasValue)
                    {
                        return $"{tierLabel}.TechSkillLevelId must be null when IsLaborRateBySkillLevel is false.";
                    }

                    if (tier.TechSkillLevelId.HasValue &&
                        await techSkillLevelReadRepository.GetByIdAsync(tier.TechSkillLevelId.Value, cancellationToken) is null)
                    {
                        return $"{tierLabel}.TechSkillLevelId '{tier.TechSkillLevelId.Value}' was not found.";
                    }
                }
            }
            else
            {
                if (line.Tiers is { Count: > 0 })
                {
                    return $"{lineLabel}.Tiers must be null or empty when IsLaborTierStructure is false.";
                }

                if (line.BaseRate is null)
                {
                    return $"{lineLabel}.BaseRate is required when IsLaborTierStructure is false.";
                }

                if (line.BaseRate < 0)
                {
                    return $"{lineLabel}.BaseRate must be greater than or equal to 0.";
                }

                if (line.OvertimeMultiplier is < 1)
                {
                    return $"{lineLabel}.OvertimeMultiplier must be greater than or equal to 1 when provided.";
                }

                if (line.DoubleTimeMultiplier is < 1)
                {
                    return $"{lineLabel}.DoubleTimeMultiplier must be greater than or equal to 1 when provided.";
                }

                if (line.DiscountPercent is < 0 or > 100)
                {
                    return $"{lineLabel}.DiscountPercent must be between 0 and 100 when provided.";
                }
            }
        }

        return null;
    }

    private static async Task<string?> ValidateOtherItemsAsync(
        FgsSetupPricingMatrixWriteDto dto,
        IBillingCategoryReadRepository billingCategoryReadRepository,
        CancellationToken cancellationToken)
    {
        var itemIndex = 0;
        foreach (var item in dto.OtherItems ?? [])
        {
            if (!await billingCategoryReadRepository.ExistsByBillingCategoryTypeAsync(
                    item.CategoryCode,
                    activeOnly: true,
                    cancellationToken))
            {
                return $"OtherItems[{itemIndex}].CategoryCode '{item.CategoryCode}' was not found or is inactive.";
            }

            itemIndex++;
        }

        return null;
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
        RuleFor(x => x.Dto)
            .NotNull()
            .WithMessage(
                "Request body is required. Ensure the JSON is valid (unresolved Postman variables produce invalid JSON).");

        When(x => x.Dto is not null, () =>
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
        });
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
        RuleFor(x => x.Dto)
            .NotNull()
            .WithMessage(
                "Request body is required. Ensure the JSON is valid (unresolved Postman variables produce invalid JSON).");

        When(x => x.Dto is not null, () =>
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
                billingCategoryReadRepository,
                command => command.Id);
        });
    }
}

public sealed class PatchFgsSetupPricingMatrixCommandValidator : AbstractValidator<PatchFgsSetupPricingMatrixCommand>
{
    public PatchFgsSetupPricingMatrixCommandValidator(IFgsSetupPricingMatrixReadRepository readRepository)
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Dto)
            .NotNull()
            .WithMessage(
                "Request body is required. Ensure the JSON is valid (unresolved Postman variables produce invalid JSON).");

        When(x => x.Dto is not null, () =>
        {
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
        });
    }
}
