using Fgs.Setup.Application.Abstractions.SalesDispositionReasons;
using Fgs.Setup.Application.Features.SalesDispositionReasons.Commands.CreateFgsSalesDispositionReason;
using Fgs.Setup.Application.Features.SalesDispositionReasons.Commands.PatchFgsSalesDispositionReason;
using Fgs.Setup.Application.Features.SalesDispositionReasons.Commands.UpdateFgsSalesDispositionReason;
using Fgs.Setup.Application.Features.SalesDispositionReasons.Dtos;
using Fgs.Setup.Application.Features.SalesDispositionReasons.Validators;
using Moq;

namespace Fgs.Setup.Tests.SalesDispositionReasons;

public sealed class FgsSalesDispositionReasonValidatorTests
{
    private readonly Mock<IFgsSalesDispositionReasonReadRepository> _readRepository = new();

    [Fact]
    public async Task CreateValidator_WhenDispositionReasonCodeMissing_HasValidationError()
    {
        var validator = new CreateFgsSalesDispositionReasonCommandValidator(_readRepository.Object);
        var command = new CreateFgsSalesDispositionReasonCommand(new FgsSalesDispositionReasonCreateDto("", "DispositionReasonName", "Description", 5, false, true, false, false, true, true));

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Dto.DispositionReasonCode");
    }

    [Fact]
    public async Task CreateValidator_WhenDispositionReasonCodeNotUppercase_HasValidationError()
    {
        var validator = new CreateFgsSalesDispositionReasonCommandValidator(_readRepository.Object);
        var args = new FgsSalesDispositionReasonCreateDto("TEST", "DispositionReasonName", "Description", 5, false, true, false, false, true, true);
        var command = new CreateFgsSalesDispositionReasonCommand(args with { DispositionReasonCode = "test" });

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Dto.DispositionReasonCode");
    }

    [Fact]
    public async Task UpdateValidator_WhenDuplicateCodeExcludesCurrentId_Passes()
    {

        _readRepository
            .Setup(r => r.ExistsByDispositionReasonCodeAsync("TEST", 5, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
        _readRepository
            .Setup(r => r.ExistsByDispositionReasonNameAsync(It.IsAny<string>(), 5, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
        var validator = new UpdateFgsSalesDispositionReasonCommandValidator(_readRepository.Object);
        var command = new UpdateFgsSalesDispositionReasonCommand(5, new FgsSalesDispositionReasonUpdateDto("TEST", "DispositionReasonName", "Description", 5, false, true, false, false, true, true));

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeTrue();
    }
}
