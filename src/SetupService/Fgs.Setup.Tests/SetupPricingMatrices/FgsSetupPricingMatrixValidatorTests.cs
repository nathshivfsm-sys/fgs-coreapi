using Fgs.Setup.Application.Abstractions.BillingCategories;
using Fgs.Setup.Application.Abstractions.SetupLaborRateTypes;
using Fgs.Setup.Application.Abstractions.SetupPricingMatrices;
using Fgs.Setup.Application.Abstractions.SetupTechSkillLevels;
using Fgs.Setup.Application.Features.SetupLaborRateTypes.Dtos;
using Fgs.Setup.Application.Features.SetupPricingMatrices.Commands.CreateFgsSetupPricingMatrix;
using Fgs.Setup.Application.Features.SetupPricingMatrices.Dtos;
using Fgs.Setup.Application.Features.SetupPricingMatrices.Validators;
using Fgs.Setup.Application.Features.SetupTechSkillLevels.Dtos;
using Moq;

namespace Fgs.Setup.Tests.SetupPricingMatrices;

public sealed class FgsSetupPricingMatrixValidatorTests
{
    private readonly Mock<IFgsSetupPricingMatrixReadRepository> _readRepository = new();
    private readonly Mock<IFgsSetupLaborRateTypeReadRepository> _laborRateTypeReadRepository = new();
    private readonly Mock<IFgsSetupTechSkillLevelReadRepository> _techSkillLevelReadRepository = new();
    private readonly Mock<IBillingCategoryReadRepository> _billingCategoryReadRepository = new();

    public FgsSetupPricingMatrixValidatorTests()
    {
        _readRepository
            .Setup(r => r.ExistsByCodeAsync(It.IsAny<string>(), It.IsAny<long?>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        _laborRateTypeReadRepository
            .Setup(r => r.GetByIdAsync(It.IsAny<long>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new FgsSetupLaborRateTypeDetailDto(1, "Standard", null, 1, true));

        _techSkillLevelReadRepository
            .Setup(r => r.GetByIdAsync(It.IsAny<long>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new FgsSetupTechSkillLevelDetailDto(100, "JOURNEY", "Journeyman", null, 1, true));

        _billingCategoryReadRepository
            .Setup(r => r.ExistsByBillingCategoryTypeAsync(It.IsAny<string>(), true, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);
    }

    [Fact]
    public async Task CreateValidator_WhenMaterialAndOtherBothPresent_HasValidationError()
    {
        var validator = CreateValidator();
        var command = new CreateFgsSetupPricingMatrixCommand(
            BuildCreateDto(
                materialTiers: [new FgsSetupPricingMatrixMaterialTierDto(null, 0m, null, 10m)],
                otherItems: [new FgsSetupPricingMatrixOtherItemDto(null, "NI", "Non-Inventory", 10m, null)],
                priceAdjustmentTypeId: 1));

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorMessage.Contains("mutually exclusive"));
    }

    [Fact]
    public async Task CreateValidator_WhenMarkupPresentWithoutPriceAdjustmentType_HasValidationError()
    {
        var validator = CreateValidator();
        var command = new CreateFgsSetupPricingMatrixCommand(
            BuildCreateDto(
                otherItems: [new FgsSetupPricingMatrixOtherItemDto(null, "NI", "Non-Inventory", 10m, null)],
                priceAdjustmentTypeId: null));

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorMessage.Contains("PriceAdjustmentTypeId"));
    }

    [Fact]
    public async Task CreateValidator_WhenSkillLevelRequiredButMissing_HasValidationError()
    {
        var validator = CreateValidator();
        var command = new CreateFgsSetupPricingMatrixCommand(
            BuildCreateDto(
                isLaborRateBySkillLevel: true,
                laborLines:
                [
                    new FgsSetupPricingMatrixLaborLineDto(
                        null, 1, null, 75m, null, null, null, null)
                ]));

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorMessage.Contains("TechSkillLevelId is required"));
    }

    [Fact]
    public async Task CreateValidator_WhenFlatLaborWithTiers_HasValidationError()
    {
        var validator = CreateValidator();
        var command = new CreateFgsSetupPricingMatrixCommand(
            BuildCreateDto(
                isLaborTierStructure: false,
                laborLines:
                [
                    new FgsSetupPricingMatrixLaborLineDto(
                        null,
                        1,
                        null,
                        75m,
                        null,
                        null,
                        null,
                        [new FgsSetupPricingMatrixLaborTierItemDto(null, 1, 60, 50m, null)])
                ]));

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorMessage.Contains("Tiers must be null or empty"));
    }

    [Fact]
    public async Task CreateValidator_WhenTierLaborWithoutTiers_HasValidationError()
    {
        var validator = CreateValidator();
        var command = new CreateFgsSetupPricingMatrixCommand(
            BuildCreateDto(
                isLaborTierStructure: true,
                laborLines:
                [
                    new FgsSetupPricingMatrixLaborLineDto(
                        null, 1, null, null, null, null, null, [])
                ]));

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorMessage.Contains("Tiers is required"));
    }

    [Fact]
    public async Task CreateValidator_WhenSkillLevelSentButFlagOff_HasValidationError()
    {
        var validator = CreateValidator();
        var command = new CreateFgsSetupPricingMatrixCommand(
            BuildCreateDto(
                isLaborRateBySkillLevel: false,
                laborLines:
                [
                    new FgsSetupPricingMatrixLaborLineDto(
                        null, 1, 100, 75m, null, null, null, null)
                ]));

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorMessage.Contains("TechSkillLevelId must be null"));
    }

    [Fact]
    public async Task CreateValidator_WhenLaborRateTypeMissing_HasValidationError()
    {
        _laborRateTypeReadRepository
            .Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync((FgsSetupLaborRateTypeDetailDto?)null);

        var validator = CreateValidator();
        var command = new CreateFgsSetupPricingMatrixCommand(
            BuildCreateDto(
                laborLines:
                [
                    new FgsSetupPricingMatrixLaborLineDto(
                        null, 1, null, 85m, 1.5m, 2.0m, null, null)
                ]));

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorMessage.Contains("LaborRateTypeId '1' was not found"));
    }

    [Fact]
    public async Task CreateValidator_WhenInvalidBillingCategoryType_HasValidationError()
    {
        _billingCategoryReadRepository
            .Setup(r => r.ExistsByBillingCategoryTypeAsync("XX", true, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        var validator = CreateValidator();
        var command = new CreateFgsSetupPricingMatrixCommand(
            BuildCreateDto(
                otherItems: [new FgsSetupPricingMatrixOtherItemDto(null, "XX", "Invalid", 10m, null)],
                priceAdjustmentTypeId: 1));

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorMessage.Contains("CategoryCode 'XX' was not found"));
    }

    [Fact]
    public async Task CreateValidator_WhenValidOtherItems_Passes()
    {
        var validator = CreateValidator();
        var command = new CreateFgsSetupPricingMatrixCommand(
            BuildCreateDto(
                otherItems: [new FgsSetupPricingMatrixOtherItemDto(null, "NI", "Non-Inventory markup", 15m, null)],
                priceAdjustmentTypeId: 1));

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeTrue();
    }

    private CreateFgsSetupPricingMatrixCommandValidator CreateValidator() =>
        new(
            _readRepository.Object,
            _laborRateTypeReadRepository.Object,
            _techSkillLevelReadRepository.Object,
            _billingCategoryReadRepository.Object);

    private static FgsSetupPricingMatrixCreateDto BuildCreateDto(
        bool isLaborTierStructure = false,
        bool isLaborRateBySkillLevel = false,
        short? priceAdjustmentTypeId = null,
        IReadOnlyList<FgsSetupPricingMatrixLaborLineDto>? laborLines = null,
        IReadOnlyList<FgsSetupPricingMatrixMaterialTierDto>? materialTiers = null,
        IReadOnlyList<FgsSetupPricingMatrixOtherItemDto>? otherItems = null) =>
        new(
            "MATRIX1",
            "Test pricing matrix",
            false,
            isLaborTierStructure,
            isLaborRateBySkillLevel,
            priceAdjustmentTypeId,
            null,
            null,
            true,
            laborLines,
            materialTiers,
            otherItems);
}
