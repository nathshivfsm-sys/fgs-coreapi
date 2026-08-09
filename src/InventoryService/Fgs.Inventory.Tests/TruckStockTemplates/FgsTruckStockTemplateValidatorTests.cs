using Fgs.Inventory.Application.Abstractions.TruckStockTemplates;
using Fgs.Inventory.Application.Features.TruckStockTemplates.Commands.CreateFgsTruckStockTemplate;
using Fgs.Inventory.Application.Features.TruckStockTemplates.Commands.UpdateFgsTruckStockTemplate;
using Fgs.Inventory.Application.Features.TruckStockTemplates.Dtos;
using Fgs.Inventory.Application.Features.TruckStockTemplates.Validators;
using Moq;

namespace Fgs.Inventory.Tests.TruckStockTemplates;

public sealed class FgsTruckStockTemplateValidatorTests
{
    private readonly Mock<IFgsTruckStockTemplateReadRepository> _templateReadRepository = new();

    [Fact]
    public async Task CreateValidator_WhenTemplateCodeMissing_HasValidationError()
    {
        var validator = new CreateFgsTruckStockTemplateCommandValidator(_templateReadRepository.Object);
        var command = new CreateFgsTruckStockTemplateCommand(new FgsTruckStockTemplateCreateDto("", "Name", null));

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Dto.TemplateCode");
    }

    [Fact]
    public async Task CreateValidator_WhenTemplateCodeNotUppercase_HasValidationError()
    {
        var validator = new CreateFgsTruckStockTemplateCommandValidator(_templateReadRepository.Object);
        var command = new CreateFgsTruckStockTemplateCommand(
            new FgsTruckStockTemplateCreateDto("truck-std", "Name", null));

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Dto.TemplateCode");
    }

    [Fact]
    public async Task UpdateValidator_WhenDuplicateCodeExcludesCurrentId_Passes()
    {
        _templateReadRepository
            .Setup(r => r.ExistsByTemplateCodeAsync("TRUCK-STD", 5, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
        var validator = new UpdateFgsTruckStockTemplateCommandValidator(_templateReadRepository.Object);
        var command = new UpdateFgsTruckStockTemplateCommand(
            5,
            new FgsTruckStockTemplateUpdateDto("TRUCK-STD", "Standard Truck", null));

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public async Task CreateValidator_WhenItemTargetBelowMinimum_HasValidationError()
    {
        _templateReadRepository
            .Setup(r => r.ExistsByTemplateCodeAsync(It.IsAny<string>(), null, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
        _templateReadRepository
            .Setup(r => r.ExistsInventoryItemAsync(10, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var validator = new CreateFgsTruckStockTemplateCommandValidator(_templateReadRepository.Object);
        var command = new CreateFgsTruckStockTemplateCommand(
            new FgsTruckStockTemplateCreateDto(
                "TRUCK-STD",
                "Name",
                null,
                [new FgsTruckStockTemplateItemDto(null, 10, TargetQuantity: 1m, MinimumQuantity: 5m, DisplayOrder: 1)]));

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorMessage.Contains("TargetQuantity"));
    }
}
