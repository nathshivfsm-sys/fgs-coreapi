using Fgs.Inventory.Application.Abstractions.InventoryTransactions;
using Fgs.Inventory.Application.Features.InventoryTransactions.Commands.CreateFgsInventoryTransaction;
using Fgs.Inventory.Application.Features.InventoryTransactions.Dtos;
using Fgs.Inventory.Application.Features.InventoryTransactions.Validators;
using Fgs.Inventory.Domain.Entities;
using Moq;

namespace Fgs.Inventory.Tests.InventoryTransactions;

public sealed class FgsInventoryTransactionValidatorTests
{
    private readonly Mock<IFgsInventoryTransactionReadRepository> _readRepository = new();

    private static FgsInventoryTransactionCreateDto SampleCreateDto() =>
        new(
            "TXN-001",
            10,
            null,
            InventoryTransactionTypes.PurchaseReceipt,
            5m,
            null,
            1,
            9.99m,
            null,
            "PO",
            100,
            "Receipt");

    [Fact]
    public async Task CreateValidator_WhenQuantityIsZero_HasValidationError()
    {
        _readRepository
            .Setup(r => r.ExistsByTransactionNumberAsync(It.IsAny<string>(), null, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
        _readRepository
            .Setup(r => r.ExistsInventoryItemAsync(It.IsAny<long>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);
        var validator = new CreateFgsInventoryTransactionCommandValidator(_readRepository.Object);
        var command = new CreateFgsInventoryTransactionCommand(SampleCreateDto() with { Quantity = 0m });

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Dto.Quantity");
    }

    [Fact]
    public async Task CreateValidator_WhenInvalidTransactionType_HasValidationError()
    {
        _readRepository
            .Setup(r => r.ExistsByTransactionNumberAsync(It.IsAny<string>(), null, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
        _readRepository
            .Setup(r => r.ExistsInventoryItemAsync(10, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);
        var validator = new CreateFgsInventoryTransactionCommandValidator(_readRepository.Object);
        var command = new CreateFgsInventoryTransactionCommand(SampleCreateDto() with { TransactionType = "INVALID" });

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Dto.TransactionType");
    }
}
