using Fgs.Setup.Application.Abstractions.SetupPricingMatrices;
using Fgs.Setup.Application.Features.SetupPricingMatrices.Commands.CreateFgsSetupPricingMatrix;
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
        var result = await Validator().ValidateAsync(
            new CreateFgsSetupPricingMatrixCommand(BuildDto(name: "")));

        result.Errors.Should().Contain(e => e.PropertyName == "Dto.Name");
    }

    [Fact]
    public async Task CreateValidator_WhenNameIsNotUppercase_HasValidationError()
    {
        var result = await Validator().ValidateAsync(
            new CreateFgsSetupPricingMatrixCommand(BuildDto(name: "Matrix1")));

        result.Errors.Should().Contain(e => e.ErrorMessage.Contains("uppercase"));
    }

    [Fact]
    public async Task CreateValidator_WhenCodeExists_HasValidationError()
    {
        _repository.Setup(r => r.ExistsByCodeAsync(
            "MATRIX1", null, It.IsAny<CancellationToken>())).ReturnsAsync(true);

        var result = await Validator().ValidateAsync(
            new CreateFgsSetupPricingMatrixCommand(BuildDto()));

        result.Errors.Should().Contain(e => e.ErrorMessage.Contains("already exists"));
    }

    [Fact]
    public async Task CreateValidator_WhenHeaderIsValid_Passes()
    {
        _repository.Setup(r => r.ExistsByCodeAsync(
            "MATRIX1", null, It.IsAny<CancellationToken>())).ReturnsAsync(false);

        var result = await Validator().ValidateAsync(
            new CreateFgsSetupPricingMatrixCommand(BuildDto()));

        result.IsValid.Should().BeTrue();
    }

    private CreateFgsSetupPricingMatrixCommandValidator Validator() => new(_repository.Object);

    private static FgsSetupPricingMatrixCreateDto BuildDto(string name = "MATRIX1") =>
        new(name, "Test pricing matrix", false, false, false, 1,
            new DateOnly(2026, 1, 1), null, true);
}
