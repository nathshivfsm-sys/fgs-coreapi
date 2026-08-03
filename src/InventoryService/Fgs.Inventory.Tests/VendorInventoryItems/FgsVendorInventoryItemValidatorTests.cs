using Fgs.Inventory.Application.Abstractions.VendorInventoryItems;
using Fgs.Inventory.Application.Abstractions.Vendors;
using Fgs.Inventory.Application.Features.VendorInventoryItems.Commands.CreateFgsVendorInventoryItem;
using Fgs.Inventory.Application.Features.VendorInventoryItems.Dtos;
using Fgs.Inventory.Application.Features.VendorInventoryItems.Validators;
using Moq;

namespace Fgs.Inventory.Tests.VendorInventoryItems;

public sealed class FgsVendorInventoryItemValidatorTests
{
    private readonly Mock<IFgsVendorReadRepository> _vendorReadRepository = new();
    private readonly Mock<IFgsVendorInventoryItemReadRepository> _readRepository = new();

    private static FgsVendorInventoryItemCreateDto SampleCreateDto() =>
        new(1, 10, "VP-001", "Vendor Part", 9.99m, null, null, 1, 5);

    [Fact]
    public async Task CreateValidator_WhenVendorIdMissing_HasValidationError()
    {
        _vendorReadRepository
            .Setup(r => r.ExistsAsync(It.IsAny<long>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);
        _readRepository
            .Setup(r => r.ExistsInventoryItemAsync(It.IsAny<long>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);
        var validator = new CreateFgsVendorInventoryItemCommandValidator(
            _vendorReadRepository.Object,
            _readRepository.Object);
        var command = new CreateFgsVendorInventoryItemCommand(SampleCreateDto() with { VendorId = 0 });

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Dto.VendorId");
    }

    [Fact]
    public async Task CreateValidator_WhenDuplicateVendorAndItem_HasValidationError()
    {
        _vendorReadRepository
            .Setup(r => r.ExistsAsync(1, It.IsAny<bool>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);
        _readRepository
            .Setup(r => r.ExistsInventoryItemAsync(10, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);
        _readRepository
            .Setup(r => r.ExistsByVendorAndItemAsync(1, 10, null, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);
        var validator = new CreateFgsVendorInventoryItemCommandValidator(
            _vendorReadRepository.Object,
            _readRepository.Object);
        var command = new CreateFgsVendorInventoryItemCommand(SampleCreateDto());

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Dto");
    }

    [Fact]
    public async Task CreateValidator_WhenDtoNull_HasValidationError()
    {
        var validator = new CreateFgsVendorInventoryItemCommandValidator(
            _vendorReadRepository.Object,
            _readRepository.Object);

        var result = await validator.ValidateAsync(new CreateFgsVendorInventoryItemCommand(null!));

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Dto");
        result.Errors.Should().Contain(e => e.ErrorMessage.Contains("Request body is required", StringComparison.Ordinal));
    }
}
