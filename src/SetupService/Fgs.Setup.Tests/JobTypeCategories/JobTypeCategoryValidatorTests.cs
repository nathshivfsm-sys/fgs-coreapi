using Fgs.Setup.Application.Abstractions.JobTypeCategories;
using Fgs.Setup.Application.Features.JobTypeCategories.Commands.CreateJobTypeCategory;
using Fgs.Setup.Application.Features.JobTypeCategories.Commands.PatchJobTypeCategory;
using Fgs.Setup.Application.Features.JobTypeCategories.Commands.UpdateJobTypeCategory;
using Fgs.Setup.Application.Features.JobTypeCategories.Dtos;
using Fgs.Setup.Application.Features.JobTypeCategories.Validators;
using Moq;

namespace Fgs.Setup.Tests.JobTypeCategories;

public sealed class JobTypeCategoryValidatorTests
{
    private readonly Mock<IJobTypeCategoryReadRepository> _readRepository = new();

    [Fact]
    public async Task CreateValidator_WhenCategoryCodeMissing_HasValidationError()
    {
        var validator = new CreateJobTypeCategoryCommandValidator(_readRepository.Object);
        var command = new CreateJobTypeCategoryCommand(new JobTypeCategoryCreateDto("", "Name value", "Description value", 1));

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Dto.CategoryCode");
    }

    [Fact]
    public async Task CreateValidator_WhenCategoryCodeNotUppercase_HasValidationError()
    {
        var validator = new CreateJobTypeCategoryCommandValidator(_readRepository.Object);
        var args = new JobTypeCategoryCreateDto("TEST", "Name value", "Description value", 1);
        var command = new CreateJobTypeCategoryCommand(args with { CategoryCode = "test" });

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
        var validator = new UpdateJobTypeCategoryCommandValidator(_readRepository.Object);
        var command = new UpdateJobTypeCategoryCommand(5, new JobTypeCategoryUpdateDto("TEST", "Name value", "Description value", 1));

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeTrue();
    }
}
