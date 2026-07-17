using Fgs.Setup.Application.Abstractions.UniversalMatrixOneTimeFees;
using Fgs.Setup.Application.Features.UniversalMatrixOneTimeFees.Commands.CreateFgsUniversalMatrixOneTimeFee;
using Fgs.Setup.Application.Features.UniversalMatrixOneTimeFees.Commands.PatchFgsUniversalMatrixOneTimeFee;
using Fgs.Setup.Application.Features.UniversalMatrixOneTimeFees.Commands.UpdateFgsUniversalMatrixOneTimeFee;
using Fgs.Setup.Application.Features.UniversalMatrixOneTimeFees.Dtos;
using Fgs.Setup.Application.Features.UniversalMatrixOneTimeFees.Validators;
using Moq;

namespace Fgs.Setup.Tests.UniversalMatrixOneTimeFees;

public sealed class FgsUniversalMatrixOneTimeFeeValidatorTests
{
    private readonly Mock<IFgsUniversalMatrixOneTimeFeeReadRepository> _readRepository = new();

    [Fact]
    public async Task CreateValidator_WhenNameMissing_HasValidationError()
    {
        var validator = new CreateFgsUniversalMatrixOneTimeFeeCommandValidator(_readRepository.Object);
        var command = new CreateFgsUniversalMatrixOneTimeFeeCommand(new FgsUniversalMatrixOneTimeFeeCreateDto(1, "", 10.5m, 5));

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
        var validator = new UpdateFgsUniversalMatrixOneTimeFeeCommandValidator(_readRepository.Object);
        var command = new UpdateFgsUniversalMatrixOneTimeFeeCommand(5, new FgsUniversalMatrixOneTimeFeeUpdateDto(1, "Name", 10.5m, 5));

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeTrue();
    }
}
