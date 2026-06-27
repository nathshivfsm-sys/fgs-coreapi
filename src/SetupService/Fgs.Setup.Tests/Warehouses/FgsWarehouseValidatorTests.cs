using Fgs.Setup.Application.Abstractions.Warehouses;
using Fgs.Setup.Application.Features.Warehouses.Commands.CreateFgsWarehouse;
using Fgs.Setup.Application.Features.Warehouses.Commands.PatchFgsWarehouse;
using Fgs.Setup.Application.Features.Warehouses.Commands.UpdateFgsWarehouse;
using Fgs.Setup.Application.Features.Warehouses.Dtos;
using Fgs.Setup.Application.Features.Warehouses.Validators;
using Moq;

namespace Fgs.Setup.Tests.Warehouses;

public sealed class FgsWarehouseValidatorTests
{
    private readonly Mock<IFgsWarehouseReadRepository> _readRepository = new();

    [Fact]
    public async Task CreateValidator_WhenWarehouseCodeMissing_HasValidationError()
    {
        var validator = new CreateFgsWarehouseCommandValidator(_readRepository.Object);
        var command = new CreateFgsWarehouseCommand(new FgsWarehouseCreateDto("", "Name", "WarehouseType", null, "Description value", false));

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Dto.WarehouseCode");
    }

    [Fact]
    public async Task CreateValidator_WhenWarehouseCodeNotUppercase_HasValidationError()
    {
        var validator = new CreateFgsWarehouseCommandValidator(_readRepository.Object);
        var args = new FgsWarehouseCreateDto("TEST", "Name", "WarehouseType", null, "Description value", false);
        var command = new CreateFgsWarehouseCommand(args with { WarehouseCode = "test" });

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Dto.WarehouseCode");
    }

    [Fact]
    public async Task UpdateValidator_WhenDuplicateCodeExcludesCurrentId_Passes()
    {

        _readRepository
            .Setup(r => r.ExistsByWarehouseCodeAsync("TEST", 5, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
        var validator = new UpdateFgsWarehouseCommandValidator(_readRepository.Object);
        var command = new UpdateFgsWarehouseCommand(5, new FgsWarehouseUpdateDto("TEST", "Name", "WarehouseType", null, "Description value", false));

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeTrue();
    }
}
