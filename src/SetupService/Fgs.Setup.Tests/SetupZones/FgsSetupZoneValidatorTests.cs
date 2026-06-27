using Fgs.Setup.Application.Abstractions.SetupZones;
using Fgs.Setup.Application.Features.SetupZones.Commands.CreateFgsSetupZone;
using Fgs.Setup.Application.Features.SetupZones.Commands.PatchFgsSetupZone;
using Fgs.Setup.Application.Features.SetupZones.Commands.UpdateFgsSetupZone;
using Fgs.Setup.Application.Features.SetupZones.Dtos;
using Fgs.Setup.Application.Features.SetupZones.Validators;
using Moq;

namespace Fgs.Setup.Tests.SetupZones;

public sealed class FgsSetupZoneValidatorTests
{
    private readonly Mock<IFgsSetupZoneReadRepository> _readRepository = new();

    [Fact]
    public async Task CreateValidator_WhenCodeMissing_HasValidationError()
    {
        var validator = new CreateFgsSetupZoneCommandValidator(_readRepository.Object);
        var command = new CreateFgsSetupZoneCommand(new FgsSetupZoneCreateDto("", "Name value", "Description value"));

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Dto.Code");
    }

    [Fact]
    public async Task CreateValidator_WhenCodeNotUppercase_HasValidationError()
    {
        var validator = new CreateFgsSetupZoneCommandValidator(_readRepository.Object);
        var args = new FgsSetupZoneCreateDto("TEST", "Name value", "Description value");
        var command = new CreateFgsSetupZoneCommand(args with { Code = "test" });

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Dto.Code");
    }

    [Fact]
    public async Task UpdateValidator_WhenDuplicateCodeExcludesCurrentId_Passes()
    {

        _readRepository
            .Setup(r => r.ExistsByCodeAsync("TEST", 5, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
        var validator = new UpdateFgsSetupZoneCommandValidator(_readRepository.Object);
        var command = new UpdateFgsSetupZoneCommand(5, new FgsSetupZoneUpdateDto("TEST", "Name value", "Description value"));

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeTrue();
    }
}
