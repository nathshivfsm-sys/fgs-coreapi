using Fgs.Inventory.Application.Abstractions.InventoryCategories;
using Fgs.Inventory.Application.Features.InventoryCategories.Commands.CreateFgsInventoryCategory;
using Fgs.Inventory.Application.Features.InventoryCategories.Commands.UpdateFgsInventoryCategory;
using Fgs.Inventory.Application.Features.InventoryCategories.Dtos;
using Fgs.Inventory.Application.Features.InventoryCategories.Validators;
using Moq;

namespace Fgs.Inventory.Tests.InventoryCategories;

public sealed class FgsInventoryCategoryValidatorTests
{
    private readonly Mock<IFgsInventoryCategoryReadRepository> _readRepository = new();

    private static FgsInventoryCategoryCreateDto SampleCreateDto(string code = "CAT01") =>
        new(code, "Category One", "Description", "#FFFFFF", "#000000", null, 1);

    [Fact]
    public async Task CreateValidator_WhenCategoryCodeMissing_HasValidationError()
    {
        var validator = new CreateFgsInventoryCategoryCommandValidator(_readRepository.Object);
        var command = new CreateFgsInventoryCategoryCommand(SampleCreateDto("") with { CategoryCode = "" });

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Dto.CategoryCode");
    }

    [Fact]
    public async Task CreateValidator_WhenCategoryCodeNotUppercase_HasValidationError()
    {
        var validator = new CreateFgsInventoryCategoryCommandValidator(_readRepository.Object);
        var command = new CreateFgsInventoryCategoryCommand(SampleCreateDto("cat01"));

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Dto.CategoryCode");
    }

    [Fact]
    public async Task UpdateValidator_WhenDuplicateCodeExcludesCurrentId_Passes()
    {
        _readRepository
            .Setup(r => r.ExistsByCategoryCodeAsync("CAT01", 5, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
        var validator = new UpdateFgsInventoryCategoryCommandValidator(_readRepository.Object);
        var updateDto = new FgsInventoryCategoryUpdateDto("CAT01", "Category One", "Description", "#FFFFFF", "#000000", null, 1);
        var command = new UpdateFgsInventoryCategoryCommand(5, updateDto);

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeTrue();
    }
}
