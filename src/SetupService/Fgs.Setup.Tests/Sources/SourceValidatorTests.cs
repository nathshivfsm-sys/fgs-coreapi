using Fgs.Setup.Application.Abstractions.Sources;
using Fgs.Setup.Application.Features.Sources.Commands.CreateSource;
using Fgs.Setup.Application.Features.Sources.Commands.PatchSource;
using Fgs.Setup.Application.Features.Sources.Commands.UpdateSource;
using Fgs.Setup.Application.Features.Sources.Dtos;
using Fgs.Setup.Application.Features.Sources.Validators;
using Moq;

namespace Fgs.Setup.Tests.Sources;

public sealed class SourceValidatorTests
{
    private readonly Mock<ISourceReadRepository> _readRepository = new();

    [Fact]
    public async Task CreateValidator_WhenSourceCodeMissing_HasValidationError()
    {
        var validator = new CreateSourceCommandValidator(_readRepository.Object);
        var command = new CreateSourceCommand(new SourceCreateDto("", "SourceName", "Description"));

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Dto.SourceCode");
    }

    [Fact]
    public async Task CreateValidator_WhenSourceCodeNotUppercase_HasValidationError()
    {
        var validator = new CreateSourceCommandValidator(_readRepository.Object);
        var args = new SourceCreateDto("TEST", "SourceName", "Description");
        var command = new CreateSourceCommand(args with { SourceCode = "test" });

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Dto.SourceCode");
    }

    [Fact]
    public async Task UpdateValidator_WhenDuplicateCodeExcludesCurrentId_Passes()
    {

        _readRepository
            .Setup(r => r.ExistsBySourceCodeAsync("TEST", 5, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
        var validator = new UpdateSourceCommandValidator(_readRepository.Object);
        var command = new UpdateSourceCommand(5, new SourceUpdateDto("TEST", "SourceName", "Description"));

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeTrue();
    }
}
