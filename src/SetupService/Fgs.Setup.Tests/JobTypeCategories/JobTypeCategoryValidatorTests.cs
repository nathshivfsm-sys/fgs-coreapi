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
    public async Task UpdateValidator_WhenDuplicateCodeExcludesCurrentId_Passes()
    {

        _readRepository
            .Setup(r => r.ExistsByJobTypeIdAndJobCategoryIdAsync(It.IsAny<long>(), It.IsAny<long>(), 5, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
        _readRepository
            .Setup(r => r.ExistsJobTypeIdAsync(It.IsAny<long>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);
        _readRepository
            .Setup(r => r.ExistsJobCategoryIdAsync(It.IsAny<long>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);
        var validator = new UpdateJobTypeCategoryCommandValidator(_readRepository.Object);
        var command = new UpdateJobTypeCategoryCommand(5, new JobTypeCategoryUpdateDto(1, 1, 1));

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeTrue();
    }
}
