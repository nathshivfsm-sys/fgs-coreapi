using Fgs.Setup.Application.Abstractions.UniversalMatrixTiers;
using Fgs.Setup.Application.Features.UniversalMatrixTiers.Commands.CreateFgsUniversalMatrixTier;
using Fgs.Setup.Application.Features.UniversalMatrixTiers.Commands.UpdateFgsUniversalMatrixTier;
using Fgs.Setup.Application.Features.UniversalMatrixTiers.Dtos;
using Fgs.Setup.Application.Features.UniversalMatrixTiers.Validators;
using Moq;

namespace Fgs.Setup.Tests.UniversalMatrixTiers;

public sealed class FgsUniversalMatrixTierValidatorTests
{
    private readonly Mock<IFgsUniversalMatrixTierReadRepository> _readRepository = new();

    [Fact]
    public async Task CreateValidator_WhenNameMissing_HasValidationError()
    {
        _readRepository
            .Setup(r => r.ExistsUniversalPricingServiceIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var validator = new CreateFgsUniversalMatrixTierCommandValidator(_readRepository.Object);
        var command = new CreateFgsUniversalMatrixTierCommand(
            new FgsUniversalMatrixTierCreateDto(1, "", 1.0m, 1));

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Dto.Name");
    }

    [Fact]
    public async Task CreateValidator_WhenParentMissing_HasValidationError()
    {
        _readRepository
            .Setup(r => r.ExistsUniversalPricingServiceIdAsync(99, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        var validator = new CreateFgsUniversalMatrixTierCommandValidator(_readRepository.Object);
        var command = new CreateFgsUniversalMatrixTierCommand(
            new FgsUniversalMatrixTierCreateDto(99, "Standard", 1.0m, 1));

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Dto.UniversalPricingServiceId");
    }

    [Fact]
    public async Task UpdateValidator_WhenValid_Passes()
    {
        _readRepository
            .Setup(r => r.ExistsUniversalPricingServiceIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);
        _readRepository
            .Setup(r => r.ExistsByNameAsync(1, "Standard", 5, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        var validator = new UpdateFgsUniversalMatrixTierCommandValidator(_readRepository.Object);
        var command = new UpdateFgsUniversalMatrixTierCommand(
            5,
            new FgsUniversalMatrixTierUpdateDto(1, "Standard", 1.0m, 1));

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeTrue();
    }
}
