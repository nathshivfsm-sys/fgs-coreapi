using Fgs.Inventory.Application.Features.Vehicles.Commands.CreateFgsVehicle;
using Fgs.Inventory.Application.Features.Vehicles.Commands.UpdateFgsVehicle;
using Fgs.Inventory.Application.Features.Vehicles.Dtos;
using Fgs.Inventory.Application.Features.Vehicles.Validators;

namespace Fgs.Inventory.Tests.Vehicles;

public sealed class FgsVehicleValidatorTests
{
    [Fact]
    public async Task CreateValidator_WhenVINMissing_HasValidationError()
    {
        var validator = new CreateFgsVehicleCommandValidator();
        var command = new CreateFgsVehicleCommand(new FgsVehicleCreateDto(1, "OwnershipType", "OwnershipCompany", 1, "Make", "Model", "Color", "", "LicensePlate", "LicensePlateState", null, 10.5m, "PurchasedFrom", null, "Notes value"));

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Dto.VIN");
    }

    [Fact]
    public async Task UpdateValidator_WhenValidDto_Passes()
    {
        var validator = new UpdateFgsVehicleCommandValidator();
        var command = new UpdateFgsVehicleCommand(5, new FgsVehicleUpdateDto(1, "OwnershipType", "OwnershipCompany", 1, "Make", "Model", "Color", "VIN", "LicensePlate", "LicensePlateState", null, 10.5m, "PurchasedFrom", null, "Notes value"));

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeTrue();
    }
}
