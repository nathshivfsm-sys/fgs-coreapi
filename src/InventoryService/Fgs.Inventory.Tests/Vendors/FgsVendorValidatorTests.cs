using Fgs.Inventory.Application.Abstractions.Vendors;
using Fgs.Inventory.Application.Features.Vendors.Commands.CreateFgsVendor;
using Fgs.Inventory.Application.Features.Vendors.Commands.UpdateFgsVendor;
using Fgs.Inventory.Application.Features.Vendors.Dtos;
using Fgs.Inventory.Application.Features.Vendors.Validators;
using Fgs.Inventory.Domain.Entities;
using Moq;

namespace Fgs.Inventory.Tests.Vendors;

public sealed class FgsVendorValidatorTests
{
    private readonly Mock<IFgsVendorReadRepository> _readRepository = new();

    private static FgsVendorCreateDto SampleCreateDto(string code = "VEND01") =>
        new(
            code,
            "Acme Supplies",
            "Acme Supplies LLC",
            VendorTypes.Vendor,
            VendorStatuses.Active,
            null,
            null,
            "Jane Doe",
            "Buyer",
            "jane@acme.com",
            null,
            "555-0100",
            null,
            null,
            "https://acme.example",
            "100 Vendor Way",
            null,
            "Austin",
            "TX",
            "78701",
            "US",
            "12-3456789",
            null,
            null,
            "Preferred vendor",
            false);

    [Fact]
    public async Task CreateValidator_WhenVendorCodeMissing_HasValidationError()
    {
        var validator = new CreateFgsVendorCommandValidator(_readRepository.Object);
        var command = new CreateFgsVendorCommand(SampleCreateDto("") with { VendorCode = "" });

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Dto.VendorCode");
    }

    [Fact]
    public async Task CreateValidator_WhenVendorCodeNotUppercase_HasValidationError()
    {
        var validator = new CreateFgsVendorCommandValidator(_readRepository.Object);
        var command = new CreateFgsVendorCommand(SampleCreateDto("vend01"));

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Dto.VendorCode");
    }

    [Fact]
    public async Task UpdateValidator_WhenDuplicateCodeExcludesCurrentId_Passes()
    {
        _readRepository
            .Setup(r => r.ExistsByVendorCodeAsync("VEND01", 5, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
        _readRepository
            .Setup(r => r.ExistsPaymentTermIdAsync(It.IsAny<long>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);
        var validator = new UpdateFgsVendorCommandValidator(_readRepository.Object);
        var updateDto = new FgsVendorUpdateDto(
            "VEND01",
            "Acme Supplies",
            "Acme Supplies LLC",
            VendorTypes.Vendor,
            VendorStatuses.Active,
            null,
            null,
            "Jane Doe",
            "Buyer",
            "jane@acme.com",
            null,
            "555-0100",
            null,
            null,
            "https://acme.example",
            "100 Vendor Way",
            null,
            "Austin",
            "TX",
            "78701",
            "US",
            "12-3456789",
            null,
            null,
            "Preferred vendor",
            false);
        var command = new UpdateFgsVendorCommand(5, updateDto);

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeTrue();
    }
}
