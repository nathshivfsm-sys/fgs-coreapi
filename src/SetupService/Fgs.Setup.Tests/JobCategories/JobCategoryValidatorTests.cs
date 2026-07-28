using Fgs.Setup.Application.Abstractions.JobCategories;
using Fgs.Setup.Application.Features.JobCategories.Commands.CreateJobCategory;
using Fgs.Setup.Application.Features.JobCategories.Commands.PatchJobCategory;
using Fgs.Setup.Application.Features.JobCategories.Commands.UpdateJobCategory;
using Fgs.Setup.Application.Features.JobCategories.Dtos;
using Fgs.Setup.Application.Features.JobCategories.Validators;
using Moq;

namespace Fgs.Setup.Tests.JobCategories;

public sealed class JobCategoryValidatorTests
{
    private readonly Mock<IJobCategoryReadRepository> _readRepository = new();

    [Fact]
    public async Task CreateValidator_WhenCategoryCodeMissing_HasValidationError()
    {
        var validator = new CreateJobCategoryCommandValidator(_readRepository.Object);
        var command = new CreateJobCategoryCommand(new JobCategoryCreateDto("", "Name", 1));

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Dto.CategoryCode");
    }

    [Fact]
    public async Task CreateValidator_WhenCategoryCodeNotUppercase_HasValidationError()
    {
        var validator = new CreateJobCategoryCommandValidator(_readRepository.Object);
        var args = new JobCategoryCreateDto("TEST", "Name", 1);
        var command = new CreateJobCategoryCommand(args with { CategoryCode = "test" });

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Dto.CategoryCode");
    }

    [Fact]
    public async Task UpdateValidator_WhenDuplicateCodeExcludesCurrentId_Passes()
    {

        _readRepository
            .Setup(r => r.ExistsByCategoryCodeAsync("TEST", 5, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
        var validator = new UpdateJobCategoryCommandValidator(_readRepository.Object);
        var command = new UpdateJobCategoryCommand(5, new JobCategoryUpdateDto("TEST", "Name", 1));

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeTrue();
    }
}
