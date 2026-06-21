using Fgs.Setup.Application.Abstractions.Vehicles;
using Fgs.Setup.Application.Features.Vehicles.Commands.CreateFgsVehicle;
using Fgs.Setup.Application.Features.Vehicles.Commands.PatchFgsVehicle;
using Fgs.Setup.Application.Features.Vehicles.Commands.UpdateFgsVehicle;
using Fgs.Setup.Application.Features.Vehicles.Dtos;
using Fgs.Setup.Application.Features.Vehicles.Validators;
using Moq;

namespace Fgs.Setup.Tests.Vehicles;

public sealed class FgsVehicleValidatorTests
{
    private readonly Mock<IFgsVehicleReadRepository> _readRepository = new();

    [Fact]
    public async Task CreateValidator_WhenVINMissing_HasValidationError()
    {
        var validator = new CreateFgsVehicleCommandValidator(_readRepository.Object);
        var command = new CreateFgsVehicleCommand(new FgsVehicleCreateDto(1, "OwnershipType", "OwnershipCompany", 1, "Make", "Model", "Color", "", "LicensePlate", "LicensePlateState", null, 10.5m, "PurchasedFrom", null, "Notes value"));

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Dto.VIN");
    }

    [Fact]
    public async Task UpdateValidator_WhenDuplicateCodeExcludesCurrentId_Passes()
    {

        _readRepository
            .Setup(r => r.ExistsWarehouseIdAsync(It.IsAny<long>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);
        var validator = new UpdateFgsVehicleCommandValidator(_readRepository.Object);
        var command = new UpdateFgsVehicleCommand(5, new FgsVehicleUpdateDto(1, "OwnershipType", "OwnershipCompany", 1, "Make", "Model", "Color", "VIN", "LicensePlate", "LicensePlateState", null, 10.5m, "PurchasedFrom", null, "Notes value"));

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeTrue();
    }
}
