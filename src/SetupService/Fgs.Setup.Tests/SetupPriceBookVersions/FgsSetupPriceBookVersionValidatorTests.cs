using Fgs.Setup.Application.Abstractions.SetupPriceBookVersions;
using Fgs.Setup.Application.Features.SetupPriceBookVersions.Commands.CreateFgsSetupPriceBookVersion;
using Fgs.Setup.Application.Features.SetupPriceBookVersions.Commands.PatchFgsSetupPriceBookVersion;
using Fgs.Setup.Application.Features.SetupPriceBookVersions.Commands.UpdateFgsSetupPriceBookVersion;
using Fgs.Setup.Application.Features.SetupPriceBookVersions.Dtos;
using Fgs.Setup.Application.Features.SetupPriceBookVersions.Validators;
using Moq;

namespace Fgs.Setup.Tests.SetupPriceBookVersions;

public sealed class FgsSetupPriceBookVersionValidatorTests
{
    private readonly Mock<IFgsSetupPriceBookVersionReadRepository> _readRepository = new();

    [Fact]
    public async Task CreateValidator_WhenVersionMissing_HasValidationError()
    {
        var validator = new CreateFgsSetupPriceBookVersionCommandValidator(_readRepository.Object);
        var command = new CreateFgsSetupPriceBookVersionCommand(
            new FgsSetupPriceBookVersionCreateDto(string.Empty, new DateOnly(2026, 1, 1)));

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Dto.Version");
    }

    [Fact]
    public async Task CreateValidator_WhenVersionTooLong_HasValidationError()
    {
        var validator = new CreateFgsSetupPriceBookVersionCommandValidator(_readRepository.Object);
        var command = new CreateFgsSetupPriceBookVersionCommand(
            new FgsSetupPriceBookVersionCreateDto(new string('A', 26), new DateOnly(2026, 1, 1)));

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Dto.Version");
    }

    [Fact]
    public async Task CreateValidator_WhenEffectiveToBeforeFrom_HasValidationError()
    {
        var validator = new CreateFgsSetupPriceBookVersionCommandValidator(_readRepository.Object);
        var command = new CreateFgsSetupPriceBookVersionCommand(
            new FgsSetupPriceBookVersionCreateDto(
                "2026-Q1",
                new DateOnly(2026, 6, 1),
                new DateOnly(2026, 1, 1)));

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public async Task CreateValidator_WhenDuplicateVersion_HasValidationError()
    {
        _readRepository
            .Setup(r => r.ExistsByVersionAsync("2026-Q1", null, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var validator = new CreateFgsSetupPriceBookVersionCommandValidator(_readRepository.Object);
        var command = new CreateFgsSetupPriceBookVersionCommand(
            new FgsSetupPriceBookVersionCreateDto("2026-Q1", new DateOnly(2026, 1, 1)));

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Dto.Version");
    }

    [Fact]
    public async Task UpdateValidator_WhenDuplicateVersionExcludesCurrentId_Passes()
    {
        _readRepository
            .Setup(r => r.ExistsByVersionAsync("2026-Q1", 5, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        var validator = new UpdateFgsSetupPriceBookVersionCommandValidator(_readRepository.Object);
        var command = new UpdateFgsSetupPriceBookVersionCommand(
            5,
            new FgsSetupPriceBookVersionUpdateDto("2026-Q1", new DateOnly(2026, 1, 1), new DateOnly(2026, 3, 31)));

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public async Task PatchValidator_WhenBothDatesProvidedAndInvalid_HasValidationError()
    {
        var validator = new PatchFgsSetupPriceBookVersionCommandValidator(_readRepository.Object);
        var command = new PatchFgsSetupPriceBookVersionCommand(
            1,
            new FgsSetupPriceBookVersionPatchDto(
                EffectiveFrom: new DateOnly(2026, 6, 1),
                EffectiveTo: new DateOnly(2026, 1, 1)));

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
    }
}
