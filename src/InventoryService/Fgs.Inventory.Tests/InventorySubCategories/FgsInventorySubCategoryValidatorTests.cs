using Fgs.Inventory.Application.Abstractions.InventoryCategories;
using Fgs.Inventory.Application.Abstractions.InventorySubCategories;
using Fgs.Inventory.Application.Features.InventorySubCategories.Commands.CreateFgsInventorySubCategory;
using Fgs.Inventory.Application.Features.InventorySubCategories.Commands.UpdateFgsInventorySubCategory;
using Fgs.Inventory.Application.Features.InventorySubCategories.Dtos;
using Fgs.Inventory.Application.Features.InventorySubCategories.Validators;
using Moq;

namespace Fgs.Inventory.Tests.InventorySubCategories;

public sealed class FgsInventorySubCategoryValidatorTests
{
    private readonly Mock<IFgsInventorySubCategoryReadRepository> _readRepository = new();
    private readonly Mock<IFgsInventoryCategoryReadRepository> _categoryReadRepository = new();

    private static FgsInventorySubCategoryCreateDto SampleCreateDto(string code = "SUB01") =>
        new(1, code, "Sub Category One", "Description", "#FFFFFF", "#000000", null, 1);

    [Fact]
    public async Task CreateValidator_WhenSubCategoryCodeMissing_HasValidationError()
    {
        var validator = new CreateFgsInventorySubCategoryCommandValidator(_readRepository.Object, _categoryReadRepository.Object);
        var command = new CreateFgsInventorySubCategoryCommand(SampleCreateDto("") with { SubCategoryCode = "" });

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Dto.SubCategoryCode");
    }

    [Fact]
    public async Task CreateValidator_WhenSubCategoryCodeNotUppercase_HasValidationError()
    {
        var validator = new CreateFgsInventorySubCategoryCommandValidator(_readRepository.Object, _categoryReadRepository.Object);
        var command = new CreateFgsInventorySubCategoryCommand(SampleCreateDto("sub01"));

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Dto.SubCategoryCode");
    }

    [Fact]
    public async Task UpdateValidator_WhenDuplicateCodeExcludesCurrentId_Passes()
    {
        _categoryReadRepository
            .Setup(r => r.ExistsAsync(1, true, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);
        _readRepository
            .Setup(r => r.ExistsBySubCategoryCodeAsync(1, "SUB01", 5, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
        var validator = new UpdateFgsInventorySubCategoryCommandValidator(_readRepository.Object, _categoryReadRepository.Object);
        var updateDto = new FgsInventorySubCategoryUpdateDto(1, "SUB01", "Sub Category One", "Description", "#FFFFFF", "#000000", null, 1);
        var command = new UpdateFgsInventorySubCategoryCommand(5, updateDto);

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeTrue();
    }
}
