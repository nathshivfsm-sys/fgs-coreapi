using Fgs.Setup.Application.Abstractions.SetupTechSkillLevels;
using Fgs.Setup.Application.Features.SetupTechSkillLevels.Commands.CreateFgsSetupTechSkillLevel;
using Fgs.Setup.Application.Features.SetupTechSkillLevels.Commands.PatchFgsSetupTechSkillLevel;
using Fgs.Setup.Application.Features.SetupTechSkillLevels.Commands.UpdateFgsSetupTechSkillLevel;
using Fgs.Setup.Application.Features.SetupTechSkillLevels.Dtos;
using Fgs.Setup.Application.Features.SetupTechSkillLevels.Validators;
using Moq;

namespace Fgs.Setup.Tests.SetupTechSkillLevels;

public sealed class FgsSetupTechSkillLevelValidatorTests
{
    private readonly Mock<IFgsSetupTechSkillLevelReadRepository> _readRepository = new();

    [Fact]
    public async Task CreateValidator_WhenCodeMissing_HasValidationError()
    {
        var validator = new CreateFgsSetupTechSkillLevelCommandValidator(_readRepository.Object);
        var command = new CreateFgsSetupTechSkillLevelCommand(new FgsSetupTechSkillLevelCreateDto("", "Name value", "Description value", 60));

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Dto.Code");
    }

    [Fact]
    public async Task CreateValidator_WhenCodeNotUppercase_HasValidationError()
    {
        var validator = new CreateFgsSetupTechSkillLevelCommandValidator(_readRepository.Object);
        var args = new FgsSetupTechSkillLevelCreateDto("TEST", "Name value", "Description value", 60);
        var command = new CreateFgsSetupTechSkillLevelCommand(args with { Code = "test" });

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
        var validator = new UpdateFgsSetupTechSkillLevelCommandValidator(_readRepository.Object);
        var command = new UpdateFgsSetupTechSkillLevelCommand(5, new FgsSetupTechSkillLevelUpdateDto("TEST", "Name value", "Description value", 60));

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeTrue();
    }
}
