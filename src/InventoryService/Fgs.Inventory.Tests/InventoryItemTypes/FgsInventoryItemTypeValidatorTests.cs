using Fgs.Inventory.Application.Abstractions.InventoryItemTypes;
using Fgs.Inventory.Application.Features.InventoryItemTypes.Commands.CreateFgsInventoryItemType;
using Fgs.Inventory.Application.Features.InventoryItemTypes.Dtos;
using Fgs.Inventory.Application.Features.InventoryItemTypes.Validators;
using Moq;

namespace Fgs.Inventory.Tests.InventoryItemTypes;

public sealed class FgsInventoryItemTypeValidatorTests
{
    private readonly Mock<IFgsInventoryItemTypeReadRepository> _readRepository = new();

    [Fact]
    public async Task CreateValidator_WhenItemTypeCodeNotUppercase_HasValidationError()
    {
        var validator = new CreateFgsInventoryItemTypeCommandValidator(_readRepository.Object);
        var command = new CreateFgsInventoryItemTypeCommand(
            new FgsInventoryItemTypeCreateDto("parts", "Parts", null, true));

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Dto.ItemTypeCode");
    }
}
