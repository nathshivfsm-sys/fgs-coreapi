using Fgs.Setup.Application.Abstractions.JobTypeTasks;
using Fgs.Setup.Application.Abstractions.JobTypes;
using Fgs.Setup.Application.Features.JobTypes.Commands.CreateJobType;
using Fgs.Setup.Application.Features.JobTypes.Commands.UpdateJobType;
using Fgs.Setup.Application.Features.JobTypes.Dtos;
using Fgs.Setup.Application.Features.JobTypes.Validators;
using Moq;

namespace Fgs.Setup.Tests.JobTypes;

public sealed class JobTypeValidatorTests
{
    private readonly Mock<IJobTypeReadRepository> _readRepository = new();
    private readonly Mock<IJobTypeTaskReadRepository> _taskReadRepository = new();

    public JobTypeValidatorTests()
    {
        _taskReadRepository
            .Setup(r => r.ExistsActiveByIdAsync(It.IsAny<long>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);
    }

    [Fact]
    public async Task CreateValidator_WhenJobTypeCodeMissing_HasValidationError()
    {
        var validator = new CreateJobTypeCommandValidator(_readRepository.Object, _taskReadRepository.Object);
        var command = new CreateJobTypeCommand(
            new JobTypeCreateDto("", "Name", 5, "BusinessUnit", true, true, 1, [new JobTypeSubCategoryWriteDto(10, 1)]));

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Dto.JobTypeCode");
    }

    [Fact]
    public async Task CreateValidator_WhenJobTypeCodeNotUppercase_HasValidationError()
    {
        var validator = new CreateJobTypeCommandValidator(_readRepository.Object, _taskReadRepository.Object);
        var args = new JobTypeCreateDto("TEST", "Name", 5, "BusinessUnit", true, true, 1, [new JobTypeSubCategoryWriteDto(10, 1)]);
        var command = new CreateJobTypeCommand(args with { JobTypeCode = "test" });

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Dto.JobTypeCode");
    }

    [Fact]
    public async Task CreateValidator_WhenSubCategoriesMissing_HasValidationError()
    {
        var validator = new CreateJobTypeCommandValidator(_readRepository.Object, _taskReadRepository.Object);
        var command = new CreateJobTypeCommand(new JobTypeCreateDto("TEST", "Name", 1, "BusinessUnit", true, true, 1));

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Dto.SubCategories");
    }

    [Fact]
    public async Task CreateValidator_WhenDuplicateSubCategories_HasValidationError()
    {
        var validator = new CreateJobTypeCommandValidator(_readRepository.Object, _taskReadRepository.Object);
        var command = new CreateJobTypeCommand(
            new JobTypeCreateDto(
                "TEST",
                "Name",
                1,
                "BusinessUnit",
                true,
                true,
                1,
                [new JobTypeSubCategoryWriteDto(10, 1), new JobTypeSubCategoryWriteDto(10, 2)]));

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorMessage.Contains("duplicate", StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public async Task UpdateValidator_WhenDuplicateCodeExcludesCurrentId_Passes()
    {
        _readRepository
            .Setup(r => r.ExistsByJobTypeCodeAsync("TEST", 5, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
        _readRepository
            .Setup(r => r.ExistsByNameAsync(It.IsAny<string>(), 5, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
        var validator = new UpdateJobTypeCommandValidator(_readRepository.Object, _taskReadRepository.Object);
        var command = new UpdateJobTypeCommand(
            5,
            new JobTypeUpdateDto("TEST", "Name", 1, "BusinessUnit", true, true, 1, [new JobTypeSubCategoryWriteDto(10, 1)]));

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeTrue();
    }
}
