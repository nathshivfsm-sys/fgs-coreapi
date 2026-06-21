using Fgs.Setup.Application.Abstractions.JobTypes;
using Fgs.Setup.Application.Features.JobTypes.Commands.CreateJobType;
using Fgs.Setup.Application.Features.JobTypes.Commands.PatchJobType;
using Fgs.Setup.Application.Features.JobTypes.Commands.UpdateJobType;
using Fgs.Setup.Application.Features.JobTypes.Dtos;
using Fgs.Setup.Application.Features.JobTypes.Validators;
using Moq;

namespace Fgs.Setup.Tests.JobTypes;

public sealed class JobTypeValidatorTests
{
    private readonly Mock<IJobTypeReadRepository> _readRepository = new();

    [Fact]
    public async Task CreateValidator_WhenJobTypeCodeMissing_HasValidationError()
    {
        var validator = new CreateJobTypeCommandValidator(_readRepository.Object);
        var command = new CreateJobTypeCommand(new JobTypeCreateDto(1, null, "", "TaskName value", "Description value", "UsedFor value", "Trade value", 60, "BusinessUnit value", 5, "#FFFFFF", "#000000", true, true, 1));

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Dto.JobTypeCode");
    }

    [Fact]
    public async Task CreateValidator_WhenJobTypeCodeNotUppercase_HasValidationError()
    {
        var validator = new CreateJobTypeCommandValidator(_readRepository.Object);
        var args = new JobTypeCreateDto(1, null, "TEST", "TaskName value", "Description value", "UsedFor value", "Trade value", 60, "BusinessUnit value", 5, "#FFFFFF", "#000000", true, true, 1);
        var command = new CreateJobTypeCommand(args with { JobTypeCode = "test" });

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Dto.JobTypeCode");
    }

    [Fact]
    public async Task UpdateValidator_WhenDuplicateCodeExcludesCurrentId_Passes()
    {
        _readRepository
            .Setup(r => r.ExistsJobTypeCategoryIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);
        _readRepository
            .Setup(r => r.ExistsByJobTypeCodeAsync("TEST", 5, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
        var validator = new UpdateJobTypeCommandValidator(_readRepository.Object);
        var command = new UpdateJobTypeCommand(5, new JobTypeUpdateDto(1, null, "TEST", "TaskName value", "Description value", "UsedFor value", "Trade value", 60, "BusinessUnit value", 5, "#FFFFFF", "#000000", true, true, 1));

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeTrue();
    }
}
