using Fgs.Setup.Application.Abstractions.VehicleMaintenances;
using Fgs.Setup.Application.Features.VehicleMaintenances.Commands.CreateFgsVehicleMaintenance;
using Fgs.Setup.Application.Features.VehicleMaintenances.Commands.PatchFgsVehicleMaintenance;
using Fgs.Setup.Application.Features.VehicleMaintenances.Commands.UpdateFgsVehicleMaintenance;
using Fgs.Setup.Application.Features.VehicleMaintenances.Dtos;
using Fgs.Setup.Application.Features.VehicleMaintenances.Validators;
using Moq;

namespace Fgs.Setup.Tests.VehicleMaintenances;

public sealed class FgsVehicleMaintenanceValidatorTests
{
    private readonly Mock<IFgsVehicleMaintenanceReadRepository> _readRepository = new();

    [Fact]
    public async Task UpdateValidator_WhenDuplicateCodeExcludesCurrentId_Passes()
    {

        _readRepository
            .Setup(r => r.ExistsVehicleIdAsync(It.IsAny<long>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);
        _readRepository
            .Setup(r => r.ExistsGloVehicleMaintenanceTypeIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);
        var validator = new UpdateFgsVehicleMaintenanceCommandValidator(_readRepository.Object);
        var command = new UpdateFgsVehicleMaintenanceCommand(5, new FgsVehicleMaintenanceUpdateDto(1, 1, DateOnly.FromDateTime(DateTime.UtcNow), 60, "ServiceProvider", "InvoiceNumber", 10.5m, null, 60, true, "Description", "Notes value"));

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeTrue();
    }
}
