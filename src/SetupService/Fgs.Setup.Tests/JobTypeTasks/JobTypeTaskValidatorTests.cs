using Fgs.Setup.Application.Abstractions.JobTypeTasks;
using Fgs.Setup.Application.Features.JobTypeTasks.Commands.CreateJobTypeTask;
using Fgs.Setup.Application.Features.JobTypeTasks.Commands.UpdateJobTypeTask;
using Fgs.Setup.Application.Features.JobTypeTasks.Dtos;
using Fgs.Setup.Application.Features.JobTypeTasks.Validators;
using Moq;

namespace Fgs.Setup.Tests.JobTypeTasks;

public sealed class JobTypeTaskValidatorTests
{
    private readonly Mock<IJobTypeTaskReadRepository> _readRepository = new();

    [Fact]
    public async Task CreateValidator_WhenNameMissing_HasValidationError()
    {
        var validator = new CreateJobTypeTaskCommandValidator(_readRepository.Object);
        var command = new CreateJobTypeTaskCommand(new JobTypeTaskCreateDto(1, 1, "", 5, 10.5m, 1));

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Dto.Name");
    }

    [Fact]
    public async Task CreateValidator_WhenSkillLevelIdOmitted_Passes()
    {
        SetupRequiredLookupsExist();
        var validator = new CreateJobTypeTaskCommandValidator(_readRepository.Object);
        var command = new CreateJobTypeTaskCommand(new JobTypeTaskCreateDto(1, 1, "Repair", 5, 10.5m, 1));

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeTrue();
        _readRepository.Verify(
            r => r.ExistsSkillLevelIdAsync(It.IsAny<long>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task CreateValidator_WhenSkillLevelIdNotFound_HasValidationError()
    {
        SetupRequiredLookupsExist();
        _readRepository
            .Setup(r => r.ExistsSkillLevelIdAsync(99, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
        var validator = new CreateJobTypeTaskCommandValidator(_readRepository.Object);
        var command = new CreateJobTypeTaskCommand(
            new JobTypeTaskCreateDto(1, 1, "Repair", 5, 10.5m, 1, SkillLevelId: 99));

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Dto.SkillLevelId");
    }

    [Fact]
    public async Task CreateValidator_WhenNameAlreadyExistsInCategory_HasValidationError()
    {
        SetupRequiredLookupsExist();
        _readRepository
            .Setup(r => r.ExistsByNameAsync(1, "Repair", null, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);
        var validator = new CreateJobTypeTaskCommandValidator(_readRepository.Object);
        var command = new CreateJobTypeTaskCommand(new JobTypeTaskCreateDto(1, 1, "Repair", 5, 10.5m, 1));

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Dto.Name");
    }

    [Fact]
    public async Task CreateValidator_WhenSameNameExistsInDifferentCategory_Passes()
    {
        SetupRequiredLookupsExist();
        _readRepository
            .Setup(r => r.ExistsByNameAsync(2, "Repair", null, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
        var validator = new CreateJobTypeTaskCommandValidator(_readRepository.Object);
        var command = new CreateJobTypeTaskCommand(new JobTypeTaskCreateDto(2, 1, "Repair", 5, 10.5m, 1));

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeTrue();
        _readRepository.Verify(
            r => r.ExistsByNameAsync(2, "Repair", null, It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task UpdateValidator_WhenDuplicateNameExcludesCurrentId_Passes()
    {
        SetupRequiredLookupsExist();
        var validator = new UpdateJobTypeTaskCommandValidator(_readRepository.Object);
        var command = new UpdateJobTypeTaskCommand(5, new JobTypeTaskUpdateDto(1, 1, "Repair", 5, 10.5m, 1));

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeTrue();
        _readRepository.Verify(
            r => r.ExistsByNameAsync(1, "Repair", 5, It.IsAny<CancellationToken>()),
            Times.Once);
    }

    private void SetupRequiredLookupsExist()
    {
        _readRepository
            .Setup(r => r.ExistsJobTypeCategoryIdAsync(It.IsAny<long>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);
        _readRepository
            .Setup(r => r.ExistsTradeIdAsync(It.IsAny<long>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);
        _readRepository
            .Setup(r => r.ExistsByNameAsync(
                It.IsAny<long>(),
                It.IsAny<string>(),
                It.IsAny<long?>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
    }
}
