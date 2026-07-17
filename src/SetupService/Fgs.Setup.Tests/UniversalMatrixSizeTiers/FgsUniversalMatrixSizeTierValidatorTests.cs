using Fgs.Setup.Application.Abstractions.UniversalMatrixSizeTiers;
using Fgs.Setup.Application.Features.UniversalMatrixSizeTiers.Commands.CreateFgsUniversalMatrixSizeTier;
using Fgs.Setup.Application.Features.UniversalMatrixSizeTiers.Commands.PatchFgsUniversalMatrixSizeTier;
using Fgs.Setup.Application.Features.UniversalMatrixSizeTiers.Commands.UpdateFgsUniversalMatrixSizeTier;
using Fgs.Setup.Application.Features.UniversalMatrixSizeTiers.Dtos;
using Fgs.Setup.Application.Features.UniversalMatrixSizeTiers.Validators;
using Moq;

namespace Fgs.Setup.Tests.UniversalMatrixSizeTiers;

public sealed class FgsUniversalMatrixSizeTierValidatorTests
{
    private readonly Mock<IFgsUniversalMatrixSizeTierReadRepository> _readRepository = new();

    [Fact]
    public async Task CreateValidator_WhenNameMissing_HasValidationError()
    {
        var validator = new CreateFgsUniversalMatrixSizeTierCommandValidator(_readRepository.Object);
        var command = new CreateFgsUniversalMatrixSizeTierCommand(new FgsUniversalMatrixSizeTierCreateDto(1, "", 10.5m, 5));

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
        var validator = new UpdateFgsUniversalMatrixSizeTierCommandValidator(_readRepository.Object);
        var command = new UpdateFgsUniversalMatrixSizeTierCommand(5, new FgsUniversalMatrixSizeTierUpdateDto(1, "Name", 10.5m, 5));

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeTrue();
    }
}
