using Fgs.Setup.Application.Abstractions.TechTrades;
using Fgs.Setup.Application.Features.TechTrades.Commands.CreateTechTrade;
using Fgs.Setup.Application.Features.TechTrades.Commands.PatchTechTrade;
using Fgs.Setup.Application.Features.TechTrades.Commands.UpdateTechTrade;
using Fgs.Setup.Application.Features.TechTrades.Dtos;
using Fgs.Setup.Application.Features.TechTrades.Validators;
using Moq;

namespace Fgs.Setup.Tests.TechTrades;

public sealed class TechTradeValidatorTests
{
    private readonly Mock<ITechTradeReadRepository> _readRepository = new();

    [Fact]
    public async Task CreateValidator_WhenTradeCodeMissing_HasValidationError()
    {
        var validator = new CreateTechTradeCommandValidator(_readRepository.Object);
        var command = new CreateTechTradeCommand(new TechTradeCreateDto(string.Empty, "HVAC", null, 0));

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Dto.TradeCode");
    }

    [Fact]
    public async Task CreateValidator_WhenTradeCodeNotUppercase_HasValidationError()
    {
        var validator = new CreateTechTradeCommandValidator(_readRepository.Object);
        var command = new CreateTechTradeCommand(new TechTradeCreateDto("hvac", "HVAC", null, 0));

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Dto.TradeCode");
    }

    [Fact]
    public async Task CreateValidator_WhenDuplicateTradeCode_HasValidationError()
    {
        _readRepository
            .Setup(r => r.ExistsByTradeCodeAsync("HVAC", null, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var validator = new CreateTechTradeCommandValidator(_readRepository.Object);
        var command = new CreateTechTradeCommand(new TechTradeCreateDto("HVAC", "Heating", null, 0));

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Dto.TradeCode");
    }

    [Fact]
    public async Task CreateValidator_WhenSortOrderNegative_HasValidationError()
    {
        var validator = new CreateTechTradeCommandValidator(_readRepository.Object);
        var command = new CreateTechTradeCommand(new TechTradeCreateDto("HVAC", "Heating", null, -1));

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Dto.SortOrder");
    }

    [Fact]
    public async Task UpdateValidator_WhenDuplicateTradeCodeExcludesCurrentId_Passes()
    {
        _readRepository
            .Setup(r => r.ExistsByTradeCodeAsync("HVAC", 5, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
        _readRepository
            .Setup(r => r.ExistsByNameAsync("Heating", 5, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        var validator = new UpdateTechTradeCommandValidator(_readRepository.Object);
        var command = new UpdateTechTradeCommand(5, new TechTradeUpdateDto("HVAC", "Heating", null, 1));

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public async Task PatchValidator_WhenNameProvidedAndEmpty_HasValidationError()
    {
        var validator = new PatchTechTradeCommandValidator(_readRepository.Object);
        var command = new PatchTechTradeCommand(1, new TechTradePatchDto(null, " ", null, null, null));

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Dto.Name");
    }
}
