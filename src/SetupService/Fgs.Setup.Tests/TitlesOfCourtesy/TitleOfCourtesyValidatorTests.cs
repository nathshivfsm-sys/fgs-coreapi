using Fgs.Setup.Application.Abstractions.TitlesOfCourtesy;
using Fgs.Setup.Application.Features.TitlesOfCourtesy.Commands.CreateTitleOfCourtesy;
using Fgs.Setup.Application.Features.TitlesOfCourtesy.Commands.PatchTitleOfCourtesy;
using Fgs.Setup.Application.Features.TitlesOfCourtesy.Commands.UpdateTitleOfCourtesy;
using Fgs.Setup.Application.Features.TitlesOfCourtesy.Dtos;
using Fgs.Setup.Application.Features.TitlesOfCourtesy.Validators;
using Moq;

namespace Fgs.Setup.Tests.TitlesOfCourtesy;

public sealed class TitleOfCourtesyValidatorTests
{
    private readonly Mock<ITitleOfCourtesyReadRepository> _readRepository = new();

    [Fact]
    public async Task CreateValidator_WhenCodeMissing_HasValidationError()
    {
        var validator = new CreateTitleOfCourtesyCommandValidator(_readRepository.Object);
        var command = new CreateTitleOfCourtesyCommand(new TitleOfCourtesyCreateDto(string.Empty, "Mr.", 1));

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Dto.Code");
    }

    [Fact]
    public async Task CreateValidator_WhenCodeNotUppercase_HasValidationError()
    {
        var validator = new CreateTitleOfCourtesyCommandValidator(_readRepository.Object);
        var command = new CreateTitleOfCourtesyCommand(new TitleOfCourtesyCreateDto("mr", "Mr.", 1));

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Dto.Code");
    }

    [Fact]
    public async Task CreateValidator_WhenDuplicateCode_HasValidationError()
    {
        _readRepository
            .Setup(r => r.ExistsByCodeAsync("MR", null, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var validator = new CreateTitleOfCourtesyCommandValidator(_readRepository.Object);
        var command = new CreateTitleOfCourtesyCommand(new TitleOfCourtesyCreateDto("MR", "Mr.", 1));

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Dto.Code");
    }

    [Fact]
    public async Task CreateValidator_WhenSortOrderNegative_HasValidationError()
    {
        var validator = new CreateTitleOfCourtesyCommandValidator(_readRepository.Object);
        var command = new CreateTitleOfCourtesyCommand(new TitleOfCourtesyCreateDto("MR", "Mr.", -1));

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Dto.SortOrder");
    }

    [Fact]
    public async Task UpdateValidator_WhenDuplicateCodeExcludesCurrentId_Passes()
    {
        _readRepository
            .Setup(r => r.ExistsByCodeAsync("MR", 5, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
        _readRepository
            .Setup(r => r.ExistsByDisplayNameAsync("Mr.", 5, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        var validator = new UpdateTitleOfCourtesyCommandValidator(_readRepository.Object);
        var command = new UpdateTitleOfCourtesyCommand(5, new TitleOfCourtesyUpdateDto("MR", "Mr.", 1));

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public async Task PatchValidator_WhenDisplayNameProvidedAndEmpty_HasValidationError()
    {
        var validator = new PatchTitleOfCourtesyCommandValidator(_readRepository.Object);
        var command = new PatchTitleOfCourtesyCommand(1, new TitleOfCourtesyPatchDto(null, " ", null, null));

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Dto.DisplayName");
    }
}
