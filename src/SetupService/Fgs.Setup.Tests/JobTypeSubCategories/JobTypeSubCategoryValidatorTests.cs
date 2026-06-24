using Fgs.Setup.Application.Abstractions.JobTypeSubCategories;
using Fgs.Setup.Application.Features.JobTypeSubCategories.Commands.CreateJobTypeSubCategory;
using Fgs.Setup.Application.Features.JobTypeSubCategories.Commands.PatchJobTypeSubCategory;
using Fgs.Setup.Application.Features.JobTypeSubCategories.Commands.UpdateJobTypeSubCategory;
using Fgs.Setup.Application.Features.JobTypeSubCategories.Dtos;
using Fgs.Setup.Application.Features.JobTypeSubCategories.Validators;
using Moq;

namespace Fgs.Setup.Tests.JobTypeSubCategories;

public sealed class JobTypeSubCategoryValidatorTests
{
    private readonly Mock<IJobTypeSubCategoryReadRepository> _readRepository = new();

    [Fact]
    public async Task CreateValidator_WhenSubCategoryCodeMissing_HasValidationError()
    {
        var validator = new CreateJobTypeSubCategoryCommandValidator(_readRepository.Object);
        var command = new CreateJobTypeSubCategoryCommand(new JobTypeSubCategoryCreateDto("", "Name", "Description value", 1));

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Dto.SubCategoryCode");
    }

    [Fact]
    public async Task CreateValidator_WhenSubCategoryCodeNotUppercase_HasValidationError()
    {
        var validator = new CreateJobTypeSubCategoryCommandValidator(_readRepository.Object);
        var args = new JobTypeSubCategoryCreateDto("TEST", "Name", "Description value", 1);
        var command = new CreateJobTypeSubCategoryCommand(args with { SubCategoryCode = "test" });

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Dto.SubCategoryCode");
    }

    [Fact]
    public async Task UpdateValidator_WhenDuplicateCodeExcludesCurrentId_Passes()
    {

        _readRepository
            .Setup(r => r.ExistsBySubCategoryCodeAsync("TEST", 5, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
        var validator = new UpdateJobTypeSubCategoryCommandValidator(_readRepository.Object);
        var command = new UpdateJobTypeSubCategoryCommand(5, new JobTypeSubCategoryUpdateDto("TEST", "Name", "Description value", 1));

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeTrue();
    }
}
