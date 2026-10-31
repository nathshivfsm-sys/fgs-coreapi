using Fgs.Setup.Application.Abstractions.SetupPricingMatrices;
using Fgs.Setup.Application.Features.SetupPricingMatrices.Commands.CreateFgsSetupPricingMatrix;
using Fgs.Setup.Application.Features.SetupPricingMatrices.Commands.PatchFgsSetupPricingMatrix;
using Fgs.Setup.Application.Features.SetupPricingMatrices.Commands.UpdateFgsSetupPricingMatrix;
using Fgs.Setup.Application.Features.SetupPricingMatrices.Dtos;
using Fgs.Setup.Application.Features.SetupPricingMatrices.Validators;
using Moq;

namespace Fgs.Setup.Tests.SetupPricingMatrices;

public sealed class FgsSetupPricingMatrixValidatorTests
{
    private readonly Mock<IFgsSetupPricingMatrixReadRepository> _repository = new();

    [Fact]
    public async Task CreateValidator_WhenNameMissing_HasValidationError()
    {
        var result = await CreateValidator().ValidateAsync(
            new CreateFgsSetupPricingMatrixCommand(BuildCreateDto(name: "")));

        result.Errors.Should().Contain(e =>
            e.PropertyName.Equals("Dto.Name", StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public async Task CreateValidator_WhenNameIsNotUppercase_HasValidationError()
    {
        var result = await CreateValidator().ValidateAsync(
            new CreateFgsSetupPricingMatrixCommand(BuildCreateDto(name: "Matrix1")));

        result.Errors.Should().Contain(e => e.ErrorMessage.Contains("uppercase"));
    }

    [Fact]
    public async Task CreateValidator_WhenCodeExists_HasValidationError()
    {
        _repository.Setup(r => r.ExistsByCodeAsync(
            "MATRIX1", null, It.IsAny<CancellationToken>())).ReturnsAsync(true);

        var result = await CreateValidator().ValidateAsync(
            new CreateFgsSetupPricingMatrixCommand(BuildCreateDto()));

        result.Errors.Should().Contain(e => e.ErrorMessage.Contains("already exists"));
    }

    [Fact]
    public async Task CreateValidator_WhenHeaderIsValid_Passes()
    {
        _repository.Setup(r => r.ExistsByCodeAsync(
            "MATRIX1", null, It.IsAny<CancellationToken>())).ReturnsAsync(false);

        var result = await CreateValidator().ValidateAsync(
            new CreateFgsSetupPricingMatrixCommand(BuildCreateDto()));

        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public async Task UpdateValidator_WhenPriceAdjustmentTypeDiffers_HasValidationError()
    {
        _repository.Setup(r => r.ExistsByCodeAsync(
            "MATRIX1", 1, It.IsAny<CancellationToken>())).ReturnsAsync(false);
        _repository.Setup(r => r.GetFlagsByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new FgsSetupPricingMatrixFlagsDto(1, false, false, 1, true));

        var result = await UpdateValidator().ValidateAsync(
            new UpdateFgsSetupPricingMatrixCommand(1, BuildUpdateDto(priceAdjustmentTypeId: 2)));

        result.Errors.Should().Contain(e =>
            e.ErrorMessage == "Price adjustment type cannot be changed after create.");
    }

    [Fact]
    public async Task UpdateValidator_WhenPriceAdjustmentTypeMatches_Passes()
    {
        _repository.Setup(r => r.ExistsByCodeAsync(
            "MATRIX1", 1, It.IsAny<CancellationToken>())).ReturnsAsync(false);
        _repository.Setup(r => r.GetFlagsByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new FgsSetupPricingMatrixFlagsDto(1, false, false, 1, true));

        var result = await UpdateValidator().ValidateAsync(
            new UpdateFgsSetupPricingMatrixCommand(1, BuildUpdateDto(priceAdjustmentTypeId: 1)));

        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public async Task UpdateValidator_WhenPriceAdjustmentTypeOutOfRange_HasValidationError()
    {
        _repository.Setup(r => r.ExistsByCodeAsync(
            "MATRIX1", 1, It.IsAny<CancellationToken>())).ReturnsAsync(false);

        var result = await UpdateValidator().ValidateAsync(
            new UpdateFgsSetupPricingMatrixCommand(1, BuildUpdateDto(priceAdjustmentTypeId: 9)));

        result.Errors.Should().Contain(e => e.PropertyName.Contains("PriceAdjustmentTypeId"));
    }

    [Fact]
    public async Task PatchValidator_WhenPriceAdjustmentTypeDiffers_HasValidationError()
    {
        _repository.Setup(r => r.GetFlagsByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new FgsSetupPricingMatrixFlagsDto(1, false, false, 1, true));

        var result = await PatchValidator().ValidateAsync(
            new PatchFgsSetupPricingMatrixCommand(1,
                new(null, null, null, null, null, 3, null, null, null, null)));

        result.Errors.Should().Contain(e =>
            e.ErrorMessage == "Price adjustment type cannot be changed after create.");
    }

    [Fact]
    public async Task PatchValidator_WhenPriceAdjustmentTypeOmitted_Passes()
    {
        var result = await PatchValidator().ValidateAsync(
            new PatchFgsSetupPricingMatrixCommand(1,
                new(null, "Updated", null, null, null, null, null, null, null, null)));

        result.IsValid.Should().BeTrue();
    }

    private CreateFgsSetupPricingMatrixCommandValidator CreateValidator() => new(_repository.Object);

    private UpdateFgsSetupPricingMatrixCommandValidator UpdateValidator() => new(_repository.Object);

    private PatchFgsSetupPricingMatrixCommandValidator PatchValidator() => new(_repository.Object);

    private static FgsSetupPricingMatrixCreateDto BuildCreateDto(string name = "MATRIX1") =>
        new(name, "Test pricing matrix", false, false, false, 1,
            new DateOnly(2026, 1, 1), null, true);

    private static FgsSetupPricingMatrixUpdateDto BuildUpdateDto(
        string name = "MATRIX1",
        short? priceAdjustmentTypeId = 1) =>
        new(name, "Test pricing matrix", false, false, false, priceAdjustmentTypeId,
            new DateOnly(2026, 1, 1), null, true);
}
