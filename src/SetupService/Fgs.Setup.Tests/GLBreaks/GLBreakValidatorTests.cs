using Fgs.Setup.Application.Abstractions.GLBreaks;
using Fgs.Setup.Application.Abstractions.TechTrades;
using Fgs.Setup.Application.Common.Locations;
using Fgs.Setup.Application.Features.GLBreaks.Commands.CreateGLBreak;
using Fgs.Setup.Application.Features.GLBreaks.Commands.PatchGLBreak;
using Fgs.Setup.Application.Features.GLBreaks.Commands.UpdateGLBreak;
using Fgs.Setup.Application.Features.GLBreaks.Dtos;
using Fgs.Setup.Application.Features.GLBreaks.Validators;
using Moq;

namespace Fgs.Setup.Tests.GLBreaks;

public sealed class GLBreakValidatorTests
{
    private readonly Mock<IGLBreakReadRepository> _glBreakReadRepository = new();
    private readonly Mock<ITechTradeReadRepository> _techTradeReadRepository = new();

    [Fact]
    public async Task CreateValidator_WhenBreakLevelInvalid_HasValidationError()
    {
        var validator = new CreateGLBreakCommandValidator(
            _glBreakReadRepository.Object,
            _techTradeReadRepository.Object);
        var command = new CreateGLBreakCommand(new GLBreakCreateDto(
            "HVAC", "HVAC", null, 3, null, null, []));

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Dto.BreakLevel");
    }

    [Fact]
    public async Task CreateValidator_WhenDuplicateTradeCode_HasValidationError()
    {
        var validator = new CreateGLBreakCommandValidator(
            _glBreakReadRepository.Object,
            _techTradeReadRepository.Object);
        var command = new CreateGLBreakCommand(new GLBreakCreateDto(
            "HVAC", "HVAC", null, 1, null, null, ["HVAC", "HVAC"]));

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Dto.TradeCodes");
    }

    [Fact]
    public async Task CreateValidator_WhenTradeCodeNotActive_HasValidationError()
    {
        _techTradeReadRepository
            .Setup(r => r.ExistsActiveTradeCodeAsync("HVAC", It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        var validator = new CreateGLBreakCommandValidator(
            _glBreakReadRepository.Object,
            _techTradeReadRepository.Object);
        var command = new CreateGLBreakCommand(new GLBreakCreateDto(
            "HVAC", "HVAC", null, 1, null, null, ["HVAC"]));

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName.StartsWith("Dto.TradeCodes"));
    }

    [Fact]
    public async Task CreateValidator_WhenAddressLineTooLong_HasValidationError()
    {
        var validator = new CreateGLBreakCommandValidator(
            _glBreakReadRepository.Object,
            _techTradeReadRepository.Object);
        var command = new CreateGLBreakCommand(new GLBreakCreateDto(
            "HVAC",
            "HVAC",
            null,
            1,
            null,
            new LocationWriteDto(new string('A', 201), null, null, null, null, null, null, null, null, null, null, null, null),
            []));

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Dto.Address.AddressLine1");
    }

    [Fact]
    public async Task UpdateValidator_WhenDuplicateCodeAndBreakLevel_HasValidationError()
    {
        _glBreakReadRepository
            .Setup(r => r.ExistsByCodeAndBreakLevelAsync("HVAC", 1, 5, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var validator = new UpdateGLBreakCommandValidator(
            _glBreakReadRepository.Object,
            _techTradeReadRepository.Object);
        var command = new UpdateGLBreakCommand(5, new GLBreakUpdateDto(
            "HVAC", "HVAC", null, 1, null, null, []));

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Dto.Code");
    }

    [Fact]
    public async Task PatchValidator_WhenNameEmpty_HasValidationError()
    {
        var validator = new PatchGLBreakCommandValidator(
            _glBreakReadRepository.Object,
            _techTradeReadRepository.Object);
        var command = new PatchGLBreakCommand(1, new GLBreakPatchDto(null, " ", null, null, null, null, null, null));

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Dto.Name");
    }
}
