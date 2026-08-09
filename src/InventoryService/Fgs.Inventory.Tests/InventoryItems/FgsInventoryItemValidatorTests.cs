using Fgs.Inventory.Application.Abstractions.InventoryCategories;
using Fgs.Inventory.Application.Abstractions.InventoryItems;
using Fgs.Inventory.Application.Abstractions.InventoryItemTypes;
using Fgs.Inventory.Application.Abstractions.InventorySubCategories;
using Fgs.Inventory.Application.Features.InventoryItems.Commands.CreateFgsInventoryItem;
using Fgs.Inventory.Application.Features.InventoryItems.Dtos;
using Fgs.Inventory.Application.Features.InventoryItems.Validators;
using Moq;

namespace Fgs.Inventory.Tests.InventoryItems;

public sealed class FgsInventoryItemValidatorTests
{
    private readonly Mock<IFgsInventoryItemReadRepository> _itemReadRepository = new();
    private readonly Mock<IFgsInventoryItemTypeReadRepository> _itemTypeReadRepository = new();
    private readonly Mock<IFgsInventoryCategoryReadRepository> _categoryReadRepository = new();
    private readonly Mock<IFgsInventorySubCategoryReadRepository> _subCategoryReadRepository = new();

    [Fact]
    public async Task CreateValidator_WhenItemCodeMissing_HasValidationError()
    {
        var validator = new CreateFgsInventoryItemCommandValidator(
            _itemReadRepository.Object,
            _itemTypeReadRepository.Object,
            _categoryReadRepository.Object,
            _subCategoryReadRepository.Object);
        var command = new CreateFgsInventoryItemCommand(
            new FgsInventoryItemCreateDto(1, "", "Name"));

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Dto.ItemCode");
    }

    [Fact]
    public async Task CreateValidator_WhenDtoNull_HasValidationError()
    {
        var validator = new CreateFgsInventoryItemCommandValidator(
            _itemReadRepository.Object,
            _itemTypeReadRepository.Object,
            _categoryReadRepository.Object,
            _subCategoryReadRepository.Object);

        var result = await validator.ValidateAsync(new CreateFgsInventoryItemCommand(null!));

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Dto");
    }
}
