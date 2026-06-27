using Fgs.Inventory.Application.Abstractions.InventoryLocations;
using Fgs.Inventory.Application.Features.InventoryLocations.Commands.CreateFgsInventoryLocation;
using Fgs.Inventory.Application.Features.InventoryLocations.Commands.UpdateFgsInventoryLocation;
using Fgs.Inventory.Application.Features.InventoryLocations.Dtos;
using Fgs.Inventory.Application.Features.InventoryLocations.Validators;
using Fgs.Inventory.Domain.Entities;
using Moq;

namespace Fgs.Inventory.Tests.InventoryLocations;

public sealed class FgsInventoryLocationValidatorTests
{
    private readonly Mock<IFgsInventoryLocationReadRepository> _readRepository = new();

    private static FgsInventoryLocationCreateDto SampleCreateDto(string code = "WH01") =>
        new(
            code,
            "Main Warehouse",
            InventoryLocationTypes.Warehouse,
            null,
            "Primary storage",
            "123 Main St",
            null,
            "Austin",
            "TX",
            "78701",
            "US",
            "Contact",
            "555-0100",
            "wh@example.com",
            false);

    [Fact]
    public async Task CreateValidator_WhenInventoryLocationCodeMissing_HasValidationError()
    {
        var validator = new CreateFgsInventoryLocationCommandValidator(_readRepository.Object);
        var command = new CreateFgsInventoryLocationCommand(SampleCreateDto("") with { InventoryLocationCode = "" });

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Dto.InventoryLocationCode");
    }

    [Fact]
    public async Task CreateValidator_WhenInventoryLocationCodeNotUppercase_HasValidationError()
    {
        var validator = new CreateFgsInventoryLocationCommandValidator(_readRepository.Object);
        var command = new CreateFgsInventoryLocationCommand(SampleCreateDto("wh01"));

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Dto.InventoryLocationCode");
    }

    [Fact]
    public async Task UpdateValidator_WhenDuplicateCodeExcludesCurrentId_Passes()
    {
        _readRepository
            .Setup(r => r.ExistsByInventoryLocationCodeAsync("WH01", 5, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
        var validator = new UpdateFgsInventoryLocationCommandValidator(_readRepository.Object);
        var updateDto = new FgsInventoryLocationUpdateDto(
            "WH01",
            "Main Warehouse",
            InventoryLocationTypes.Warehouse,
            null,
            "Primary storage",
            "123 Main St",
            null,
            "Austin",
            "TX",
            "78701",
            "US",
            "Contact",
            "555-0100",
            "wh@example.com",
            false);
        var command = new UpdateFgsInventoryLocationCommand(5, updateDto);

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeTrue();
    }
}
