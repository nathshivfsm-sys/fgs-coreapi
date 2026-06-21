using Fgs.Setup.Application.Abstractions.SalesActivityOutcomes;
using Fgs.Setup.Application.Features.SalesActivityOutcomes.Commands.CreateFgsSalesActivityOutcome;
using Fgs.Setup.Application.Features.SalesActivityOutcomes.Commands.PatchFgsSalesActivityOutcome;
using Fgs.Setup.Application.Features.SalesActivityOutcomes.Commands.UpdateFgsSalesActivityOutcome;
using Fgs.Setup.Application.Features.SalesActivityOutcomes.Dtos;
using Fgs.Setup.Application.Features.SalesActivityOutcomes.Validators;
using Moq;

namespace Fgs.Setup.Tests.SalesActivityOutcomes;

public sealed class FgsSalesActivityOutcomeValidatorTests
{
    private readonly Mock<IFgsSalesActivityOutcomeReadRepository> _readRepository = new();

    [Fact]
    public async Task CreateValidator_WhenOutcomeCodeMissing_HasValidationError()
    {
        var validator = new CreateFgsSalesActivityOutcomeCommandValidator(_readRepository.Object);
        var command = new CreateFgsSalesActivityOutcomeCommand(new FgsSalesActivityOutcomeCreateDto("", "OutcomeName", "Description", 5, false, true, true, null, false, false, true));

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Dto.OutcomeCode");
    }

    [Fact]
    public async Task CreateValidator_WhenOutcomeCodeNotUppercase_HasValidationError()
    {
        var validator = new CreateFgsSalesActivityOutcomeCommandValidator(_readRepository.Object);
        var args = new FgsSalesActivityOutcomeCreateDto("TEST", "OutcomeName", "Description", 5, false, true, true, null, false, false, true);
        var command = new CreateFgsSalesActivityOutcomeCommand(args with { OutcomeCode = "test" });

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Dto.OutcomeCode");
    }

    [Fact]
    public async Task UpdateValidator_WhenDuplicateCodeExcludesCurrentId_Passes()
    {

        _readRepository
            .Setup(r => r.ExistsByOutcomeCodeAsync("TEST", 5, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
        _readRepository
            .Setup(r => r.ExistsByOutcomeNameAsync(It.IsAny<string>(), 5, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
        _readRepository
            .Setup(r => r.ExistsSalesPipelineStatusIdAsync(It.IsAny<long>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);
        var validator = new UpdateFgsSalesActivityOutcomeCommandValidator(_readRepository.Object);
        var command = new UpdateFgsSalesActivityOutcomeCommand(5, new FgsSalesActivityOutcomeUpdateDto("TEST", "OutcomeName", "Description", 5, false, true, true, null, false, false, true));

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeTrue();
    }
}
