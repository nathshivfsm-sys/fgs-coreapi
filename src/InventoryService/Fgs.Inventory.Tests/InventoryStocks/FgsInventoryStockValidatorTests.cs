using Fgs.Inventory.Application.Abstractions.InventoryStocks;
using Fgs.Inventory.Application.Features.InventoryStocks.Commands.CreateFgsInventoryStock;
using Fgs.Inventory.Application.Features.InventoryStocks.Dtos;
using Fgs.Inventory.Application.Features.InventoryStocks.Validators;
using Moq;

namespace Fgs.Inventory.Tests.InventoryStocks;

public sealed class FgsInventoryStockValidatorTests
{
    private readonly Mock<IFgsInventoryStockReadRepository> _readRepository = new();

    private static FgsInventoryStockCreateDto SampleCreateDto() =>
        new(10, 100m, 10m, 90m, 5.50m, 5.50m, null, null);

    [Fact]
    public async Task CreateValidator_WhenInventoryItemIdMissing_HasValidationError()
    {
        _readRepository
            .Setup(r => r.ExistsInventoryItemAsync(It.IsAny<long>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);
        var validator = new CreateFgsInventoryStockCommandValidator(_readRepository.Object);
        var command = new CreateFgsInventoryStockCommand(SampleCreateDto() with { InventoryItemId = 0 });

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Dto.InventoryItemId");
    }

    [Fact]
    public async Task CreateValidator_WhenNegativeQuantity_HasValidationError()
    {
        _readRepository
            .Setup(r => r.ExistsInventoryItemAsync(10, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);
        _readRepository
            .Setup(r => r.ExistsByInventoryItemIdAsync(10, null, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
        var validator = new CreateFgsInventoryStockCommandValidator(_readRepository.Object);
        var command = new CreateFgsInventoryStockCommand(SampleCreateDto() with { QuantityOnHand = -1m });

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Dto.QuantityOnHand");
    }
}
