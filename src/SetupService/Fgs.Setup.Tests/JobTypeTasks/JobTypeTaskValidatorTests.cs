using Fgs.Setup.Application.Abstractions.JobTypeTasks;
using Fgs.Setup.Application.Features.JobTypeTasks.Commands.CreateJobTypeTask;
using Fgs.Setup.Application.Features.JobTypeTasks.Commands.PatchJobTypeTask;
using Fgs.Setup.Application.Features.JobTypeTasks.Commands.UpdateJobTypeTask;
using Fgs.Setup.Application.Features.JobTypeTasks.Dtos;
using Fgs.Setup.Application.Features.JobTypeTasks.Validators;
using Moq;

namespace Fgs.Setup.Tests.JobTypeTasks;

public sealed class JobTypeTaskValidatorTests
{
    private readonly Mock<IJobTypeTaskReadRepository> _readRepository = new();

    [Fact]
    public async Task CreateValidator_WhenTaskNameMissing_HasValidationError()
    {
        var validator = new CreateJobTypeTaskCommandValidator(_readRepository.Object);
        var command = new CreateJobTypeTaskCommand(new JobTypeTaskCreateDto(1, 1, "", 5, 10.5m, 1));

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Dto.TaskName");
    }

    [Fact]
    public async Task UpdateValidator_WhenDuplicateCodeExcludesCurrentId_Passes()
    {

        _readRepository
            .Setup(r => r.ExistsJobTypeCategoryIdAsync(It.IsAny<long>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);
        _readRepository
            .Setup(r => r.ExistsTradeIdAsync(It.IsAny<long>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);
        var validator = new UpdateJobTypeTaskCommandValidator(_readRepository.Object);
        var command = new UpdateJobTypeTaskCommand(5, new JobTypeTaskUpdateDto(1, 1, "TaskName", 5, 10.5m, 1));

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeTrue();
    }
}
