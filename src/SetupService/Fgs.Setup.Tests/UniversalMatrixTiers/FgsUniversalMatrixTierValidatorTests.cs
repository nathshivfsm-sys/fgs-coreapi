using Fgs.Setup.Application.Abstractions.UniversalMatrixTiers;
using Fgs.Setup.Application.Features.UniversalMatrixTiers.Commands.CreateFgsUniversalMatrixTier;
using Fgs.Setup.Application.Features.UniversalMatrixTiers.Commands.PatchFgsUniversalMatrixTier;
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
        var validator = new CreateFgsUniversalMatrixTierCommandValidator(_readRepository.Object);
        var command = new CreateFgsUniversalMatrixTierCommand(new FgsUniversalMatrixTierCreateDto(1, "", 10.5m, 5));

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Dto.Name");
    }

    [Fact]
    public async Task UpdateValidator_WhenDuplicateCodeExcludesCurrentId_Passes()
    {

        _readRepository
            .Setup(r => r.ExistsByUniversalPricingServiceIdAndNameAsync(It.IsAny<long>(), It.IsAny<string>(), 5, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
        _readRepository
            .Setup(r => r.ExistsUniversalPricingServiceIdAsync(It.IsAny<long>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);
        var validator = new UpdateFgsUniversalMatrixTierCommandValidator(_readRepository.Object);
        var command = new UpdateFgsUniversalMatrixTierCommand(5, new FgsUniversalMatrixTierUpdateDto(1, "Name", 10.5m, 5));

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeTrue();
    }
}
