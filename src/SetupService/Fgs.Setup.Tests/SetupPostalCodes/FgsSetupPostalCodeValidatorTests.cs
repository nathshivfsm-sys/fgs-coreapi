using Fgs.Setup.Application.Abstractions.SetupPostalCodes;
using Fgs.Setup.Application.Features.SetupPostalCodes.Commands.CreateFgsSetupPostalCode;
using Fgs.Setup.Application.Features.SetupPostalCodes.Commands.PatchFgsSetupPostalCode;
using Fgs.Setup.Application.Features.SetupPostalCodes.Commands.UpdateFgsSetupPostalCode;
using Fgs.Setup.Application.Features.SetupPostalCodes.Dtos;
using Fgs.Setup.Application.Features.SetupPostalCodes.Validators;
using Moq;

namespace Fgs.Setup.Tests.SetupPostalCodes;

public sealed class FgsSetupPostalCodeValidatorTests
{
    private readonly Mock<IFgsSetupPostalCodeReadRepository> _readRepository = new();

    [Fact]
    public async Task CreateValidator_WhenPostalCodeMissing_HasValidationError()
    {
        var validator = new CreateFgsSetupPostalCodeCommandValidator(_readRepository.Object);
        var command = new CreateFgsSetupPostalCodeCommand(new FgsSetupPostalCodeCreateDto(
            "", "US", "TX", "Austin", 0m, null, null));

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Dto.PostalCode");
    }

    [Fact]
    public async Task UpdateValidator_WhenDuplicateCodeExcludesCurrentId_Passes()
    {

        _readRepository
            .Setup(r => r.ExistsByPostalCodeAsync("TEST", 5, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
        _readRepository
            .Setup(r => r.ExistsZoneIdAsync(It.IsAny<long>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);
        _readRepository
            .Setup(r => r.ExistsTaxIdAsync(It.IsAny<long>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);
        var validator = new UpdateFgsSetupPostalCodeCommandValidator(_readRepository.Object);
        var command = new UpdateFgsSetupPostalCodeCommand(5, new FgsSetupPostalCodeUpdateDto(
            "PostalCode value", "US", "TX", "Austin", 0m, null, null));

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeTrue();
    }
}
