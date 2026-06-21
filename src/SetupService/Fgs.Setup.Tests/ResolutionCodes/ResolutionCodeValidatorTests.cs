using Fgs.Setup.Application.Abstractions.ResolutionCodes;
using Fgs.Setup.Application.Features.ResolutionCodes.Commands.CreateResolutionCode;
using Fgs.Setup.Application.Features.ResolutionCodes.Commands.PatchResolutionCode;
using Fgs.Setup.Application.Features.ResolutionCodes.Commands.UpdateResolutionCode;
using Fgs.Setup.Application.Features.ResolutionCodes.Dtos;
using Fgs.Setup.Application.Features.ResolutionCodes.Validators;
using Moq;

namespace Fgs.Setup.Tests.ResolutionCodes;

public sealed class ResolutionCodeValidatorTests
{
    private readonly Mock<IResolutionCodeReadRepository> _readRepository = new();

    [Fact]
    public async Task CreateValidator_WhenResolutionCodeMissing_HasValidationError()
    {
        var validator = new CreateResolutionCodeCommandValidator(_readRepository.Object);
        var command = new CreateResolutionCodeCommand(new ResolutionCodeCreateDto(1, "", "ResolutionName value", true));

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Dto.ResolutionCode");
    }

    [Fact]
    public async Task CreateValidator_WhenResolutionCodeNotUppercase_HasValidationError()
    {
        var validator = new CreateResolutionCodeCommandValidator(_readRepository.Object);
        var args = new ResolutionCodeCreateDto(1, "TEST", "ResolutionName value", true);
        var command = new CreateResolutionCodeCommand(args with { ResolutionCode = "test" });

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Dto.ResolutionCode");
    }

    [Fact]
    public async Task UpdateValidator_WhenDuplicateCodeExcludesCurrentId_Passes()
    {
        _readRepository
            .Setup(r => r.ExistsGloResolutionTypeIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);
        _readRepository
            .Setup(r => r.ExistsByResolutionCodeAsync("TEST", 5, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
        var validator = new UpdateResolutionCodeCommandValidator(_readRepository.Object);
        var command = new UpdateResolutionCodeCommand(5, new ResolutionCodeUpdateDto(1, "TEST", "ResolutionName value", true));

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeTrue();
    }
}
