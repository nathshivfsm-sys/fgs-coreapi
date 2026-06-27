using Fgs.Setup.Application.Abstractions.SalesPipelineStatuses;
using Fgs.Setup.Application.Features.SalesPipelineStatuses.Commands.CreateFgsSalesPipelineStatus;
using Fgs.Setup.Application.Features.SalesPipelineStatuses.Commands.PatchFgsSalesPipelineStatus;
using Fgs.Setup.Application.Features.SalesPipelineStatuses.Commands.UpdateFgsSalesPipelineStatus;
using Fgs.Setup.Application.Features.SalesPipelineStatuses.Dtos;
using Fgs.Setup.Application.Features.SalesPipelineStatuses.Validators;
using Moq;

namespace Fgs.Setup.Tests.SalesPipelineStatuses;

public sealed class FgsSalesPipelineStatusValidatorTests
{
    private readonly Mock<IFgsSalesPipelineStatusReadRepository> _readRepository = new();

    [Fact]
    public async Task CreateValidator_WhenStatusCodeMissing_HasValidationError()
    {
        var validator = new CreateFgsSalesPipelineStatusCommandValidator(_readRepository.Object);
        var command = new CreateFgsSalesPipelineStatusCommand(new FgsSalesPipelineStatusCreateDto("", "StatusName", "Description", 5, false, true, false, false, true));

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Dto.StatusCode");
    }

    [Fact]
    public async Task CreateValidator_WhenStatusCodeNotUppercase_HasValidationError()
    {
        var validator = new CreateFgsSalesPipelineStatusCommandValidator(_readRepository.Object);
        var args = new FgsSalesPipelineStatusCreateDto("TEST", "StatusName", "Description", 5, false, true, false, false, true);
        var command = new CreateFgsSalesPipelineStatusCommand(args with { StatusCode = "test" });

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Dto.StatusCode");
    }

    [Fact]
    public async Task UpdateValidator_WhenDuplicateCodeExcludesCurrentId_Passes()
    {

        _readRepository
            .Setup(r => r.ExistsByStatusCodeAsync("TEST", 5, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
        _readRepository
            .Setup(r => r.ExistsByStatusNameAsync(It.IsAny<string>(), 5, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
        var validator = new UpdateFgsSalesPipelineStatusCommandValidator(_readRepository.Object);
        var command = new UpdateFgsSalesPipelineStatusCommand(5, new FgsSalesPipelineStatusUpdateDto("TEST", "StatusName", "Description", 5, false, true, false, false, true));

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeTrue();
    }
}
