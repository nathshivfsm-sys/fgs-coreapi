using Fgs.Setup.Application.Abstractions.SetupLaborRateTypes;
using Fgs.Setup.Application.Features.SetupLaborRateTypes.Commands.CreateFgsSetupLaborRateType;
using Fgs.Setup.Application.Features.SetupLaborRateTypes.Commands.PatchFgsSetupLaborRateType;
using Fgs.Setup.Application.Features.SetupLaborRateTypes.Commands.UpdateFgsSetupLaborRateType;
using Fgs.Setup.Application.Features.SetupLaborRateTypes.Dtos;
using Fgs.Setup.Application.Features.SetupLaborRateTypes.Validators;
using Moq;

namespace Fgs.Setup.Tests.SetupLaborRateTypes;

public sealed class FgsSetupLaborRateTypeValidatorTests
{
    private readonly Mock<IFgsSetupLaborRateTypeReadRepository> _readRepository = new();

    [Fact]
    public async Task CreateValidator_WhenNameMissing_HasValidationError()
    {
        var validator = new CreateFgsSetupLaborRateTypeCommandValidator(_readRepository.Object);
        var command = new CreateFgsSetupLaborRateTypeCommand(new FgsSetupLaborRateTypeCreateDto("", "Description value", 1, false));

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Dto.Name");
    }

    [Fact]
    public async Task UpdateValidator_WhenDuplicateCodeExcludesCurrentId_Passes()
    {

        _readRepository
            .Setup(r => r.ExistsByNameAsync(It.IsAny<string>(), 5, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
        var validator = new UpdateFgsSetupLaborRateTypeCommandValidator(_readRepository.Object);
        var command = new UpdateFgsSetupLaborRateTypeCommand(5, new FgsSetupLaborRateTypeUpdateDto("Name value", "Description value", 1, false));

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeTrue();
    }
}
