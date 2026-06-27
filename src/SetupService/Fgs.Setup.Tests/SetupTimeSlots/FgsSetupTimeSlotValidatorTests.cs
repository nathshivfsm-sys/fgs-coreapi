using Fgs.Setup.Application.Abstractions.SetupTimeSlots;
using Fgs.Setup.Application.Features.SetupTimeSlots.Commands.CreateFgsSetupTimeSlot;
using Fgs.Setup.Application.Features.SetupTimeSlots.Commands.PatchFgsSetupTimeSlot;
using Fgs.Setup.Application.Features.SetupTimeSlots.Commands.UpdateFgsSetupTimeSlot;
using Fgs.Setup.Application.Features.SetupTimeSlots.Dtos;
using Fgs.Setup.Application.Features.SetupTimeSlots.Validators;
using Moq;

namespace Fgs.Setup.Tests.SetupTimeSlots;

public sealed class FgsSetupTimeSlotValidatorTests
{
    private readonly Mock<IFgsSetupTimeSlotReadRepository> _readRepository = new();

    [Fact]
    public async Task CreateValidator_WhenCodeMissing_HasValidationError()
    {
        var validator = new CreateFgsSetupTimeSlotCommandValidator(_readRepository.Object);
        var command = new CreateFgsSetupTimeSlotCommand(new FgsSetupTimeSlotCreateDto(null, "", "Name value", TimeSpan.FromHours(8), TimeSpan.FromHours(17), null, null, true, true));

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Dto.Code");
    }

    [Fact]
    public async Task CreateValidator_WhenCodeNotUppercase_HasValidationError()
    {
        var validator = new CreateFgsSetupTimeSlotCommandValidator(_readRepository.Object);
        var args = new FgsSetupTimeSlotCreateDto(null, "TEST", "Name value", TimeSpan.FromHours(8), TimeSpan.FromHours(17), null, null, true, true);
        var command = new CreateFgsSetupTimeSlotCommand(args with { Code = "test" });

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
        _readRepository
            .Setup(r => r.ExistsZoneIdAsync(It.IsAny<long>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);
        var validator = new UpdateFgsSetupTimeSlotCommandValidator(_readRepository.Object);
        var command = new UpdateFgsSetupTimeSlotCommand(5, new FgsSetupTimeSlotUpdateDto(null, "TEST", "Name value", TimeSpan.FromHours(8), TimeSpan.FromHours(17), null, null, true, true));

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeTrue();
    }
}
