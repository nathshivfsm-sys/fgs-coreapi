using Fgs.Setup.Application.Abstractions.UniversalPricingServices;
using Fgs.Setup.Application.Features.UniversalPricingServices.Commands.CreateFgsUniversalPricingService;
using Fgs.Setup.Application.Features.UniversalPricingServices.Commands.UpdateFgsUniversalPricingService;
using Fgs.Setup.Application.Features.UniversalPricingServices.Dtos;
using Fgs.Setup.Application.Features.UniversalPricingServices.Validators;
using Moq;

namespace Fgs.Setup.Tests.UniversalPricingServices;

public sealed class FgsUniversalPricingServiceValidatorTests
{
    private readonly Mock<IFgsUniversalPricingServiceReadRepository> _readRepository = new();

    [Fact]
    public async Task CreateValidator_WhenUniversalPricingServiceCodeMissing_HasValidationError()
    {
        var validator = new CreateFgsUniversalPricingServiceCommandValidator(_readRepository.Object);
        var command = new CreateFgsUniversalPricingServiceCommand(new FgsUniversalPricingServiceCreateDto("", 5));

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Dto.UniversalPricingServiceCode");
    }

    [Fact]
    public async Task CreateValidator_WhenUniversalPricingServiceCodeNotUppercase_HasValidationError()
    {
        var validator = new CreateFgsUniversalPricingServiceCommandValidator(_readRepository.Object);
        var args = new FgsUniversalPricingServiceCreateDto("TEST", 5);
        var command = new CreateFgsUniversalPricingServiceCommand(args with { UniversalPricingServiceCode = "test" });

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Dto.UniversalPricingServiceCode");
    }

    [Fact]
    public async Task CreateValidator_WhenCustomUniqueCode_PassesWithoutGlobalCatalog()
    {
        _readRepository
            .Setup(r => r.ExistsByUniversalPricingServiceCodeAsync("DIAGFEE", null, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        var validator = new CreateFgsUniversalPricingServiceCommandValidator(_readRepository.Object);
        var command = new CreateFgsUniversalPricingServiceCommand(
            new FgsUniversalPricingServiceCreateDto(
                "DIAGFEE",
                1,
                Tiers: [new FgsUniversalMatrixTierItemDto(null, "Diagnostic Fee", 1.00m, 1)],
                SizeTiers: [new FgsUniversalMatrixSizeTierItemDto(null, "Diagnostic Fee", 1.00m, 1)],
                Items: [new FgsUniversalMatrixItemItemDto(null, "Diagnostic Fee", "Diagnostic Fee", 100.00m, 1)],
                FrequencyDiscounts: [new FgsUniversalMatrixFrequencyDiscountItemDto(null, "Diagnostic Fee", 8.25m, 1)],
                OneTimeFees: [new FgsUniversalMatrixOneTimeFeeItemDto(null, "Diagnostic Fee", 100.00m, 1)],
                AddOns: [new FgsUniversalMatrixAddOnItemDto(null, "Diagnostic Fee", "Diagnostic Fee", 100.00m, 1)]));

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public async Task CreateValidator_WhenNestedTierMissingName_HasValidationError()
    {
        _readRepository
            .Setup(r => r.ExistsByUniversalPricingServiceCodeAsync(It.IsAny<string>(), null, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        var validator = new CreateFgsUniversalPricingServiceCommandValidator(_readRepository.Object);
        var command = new CreateFgsUniversalPricingServiceCommand(
            new FgsUniversalPricingServiceCreateDto(
                "TEST",
                5,
                Tiers: [new FgsUniversalMatrixTierItemDto(null, "", 1.0m, 1)]));

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName.Contains("Tiers") && e.PropertyName.Contains("Name"));
    }

    [Fact]
    public async Task CreateValidator_WhenDuplicateTierNames_HasValidationError()
    {
        _readRepository
            .Setup(r => r.ExistsByUniversalPricingServiceCodeAsync(It.IsAny<string>(), null, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        var validator = new CreateFgsUniversalPricingServiceCommandValidator(_readRepository.Object);
        var command = new CreateFgsUniversalPricingServiceCommand(
            new FgsUniversalPricingServiceCreateDto(
                "TEST",
                5,
                Tiers:
                [
                    new FgsUniversalMatrixTierItemDto(null, "Standard", 1.0m, 1),
                    new FgsUniversalMatrixTierItemDto(null, "standard", 1.5m, 2)
                ]));

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorMessage.Contains("unique", StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public async Task UpdateValidator_WhenDuplicateCodeExcludesCurrentId_Passes()
    {
        _readRepository
            .Setup(r => r.ExistsByUniversalPricingServiceCodeAsync("TEST", 5, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
        var validator = new UpdateFgsUniversalPricingServiceCommandValidator(_readRepository.Object);
        var command = new UpdateFgsUniversalPricingServiceCommand(5, new FgsUniversalPricingServiceUpdateDto("TEST", 5));

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeTrue();
    }
}
