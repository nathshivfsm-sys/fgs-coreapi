using Fgs.Setup.Application.Abstractions.UniversalMatrixAddOns;
using Fgs.Setup.Application.Features.UniversalMatrixAddOns.Commands.CreateFgsUniversalMatrixAddOn;
using Fgs.Setup.Application.Features.UniversalMatrixAddOns.Commands.PatchFgsUniversalMatrixAddOn;
using Fgs.Setup.Application.Features.UniversalMatrixAddOns.Commands.UpdateFgsUniversalMatrixAddOn;
using Fgs.Setup.Application.Features.UniversalMatrixAddOns.Dtos;
using Fgs.Setup.Application.Features.UniversalMatrixAddOns.Validators;
using Moq;

namespace Fgs.Setup.Tests.UniversalMatrixAddOns;

public sealed class FgsUniversalMatrixAddOnValidatorTests
{
    private readonly Mock<IFgsUniversalMatrixAddOnReadRepository> _readRepository = new();

    [Fact]
    public async Task CreateValidator_WhenNameMissing_HasValidationError()
    {
        var validator = new CreateFgsUniversalMatrixAddOnCommandValidator(_readRepository.Object);
        var command = new CreateFgsUniversalMatrixAddOnCommand(new FgsUniversalMatrixAddOnCreateDto(1, "", "UnitType", 10.5m, 5));

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
        var validator = new UpdateFgsUniversalMatrixAddOnCommandValidator(_readRepository.Object);
        var command = new UpdateFgsUniversalMatrixAddOnCommand(5, new FgsUniversalMatrixAddOnUpdateDto(1, "Name", "UnitType", 10.5m, 5));

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeTrue();
    }
}
