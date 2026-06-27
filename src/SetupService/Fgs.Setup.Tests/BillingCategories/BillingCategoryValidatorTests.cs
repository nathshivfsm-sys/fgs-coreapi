using Fgs.Setup.Application.Abstractions.BillingCategories;
using Fgs.Setup.Application.Features.BillingCategories.Commands.CreateBillingCategory;
using Fgs.Setup.Application.Features.BillingCategories.Commands.PatchBillingCategory;
using Fgs.Setup.Application.Features.BillingCategories.Commands.UpdateBillingCategory;
using Fgs.Setup.Application.Features.BillingCategories.Dtos;
using Fgs.Setup.Application.Features.BillingCategories.Validators;
using Moq;

namespace Fgs.Setup.Tests.BillingCategories;

public sealed class BillingCategoryValidatorTests
{
    private readonly Mock<IBillingCategoryReadRepository> _readRepository = new();

    [Fact]
    public async Task CreateValidator_WhenBillingCategoryTypeMissing_HasValidationError()
    {
        var validator = new CreateBillingCategoryCommandValidator(_readRepository.Object);
        var command = new CreateBillingCategoryCommand(new BillingCategoryCreateDto("", "BillingCategoryName", "Description value", 1, false, false, true));

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Dto.BillingCategoryType");
    }

    [Fact]
    public async Task CreateValidator_WhenBillingCategoryTypeNotUppercase_HasValidationError()
    {
        var validator = new CreateBillingCategoryCommandValidator(_readRepository.Object);
        var args = new BillingCategoryCreateDto("TEST", "BillingCategoryName", "Description value", 1, false, false, true);
        var command = new CreateBillingCategoryCommand(args with { BillingCategoryType = "test" });

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Dto.BillingCategoryType");
    }

    [Fact]
    public async Task UpdateValidator_WhenDuplicateCodeExcludesCurrentId_Passes()
    {

        _readRepository
            .Setup(r => r.ExistsByBillingCategoryTypeAndBillingCategoryNameAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<long?>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
        var validator = new UpdateBillingCategoryCommandValidator(_readRepository.Object);
        var command = new UpdateBillingCategoryCommand(5, new BillingCategoryUpdateDto("TE", "BillingCategoryName", "Description value", 1, false, false, true));

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeTrue();
    }
}
