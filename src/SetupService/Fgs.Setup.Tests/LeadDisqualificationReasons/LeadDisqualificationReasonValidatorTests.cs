using Fgs.Setup.Application.Abstractions.LeadDisqualificationReasons;
using Fgs.Setup.Application.Features.LeadDisqualificationReasons.Commands.CreateLeadDisqualificationReason;
using Fgs.Setup.Application.Features.LeadDisqualificationReasons.Commands.PatchLeadDisqualificationReason;
using Fgs.Setup.Application.Features.LeadDisqualificationReasons.Commands.UpdateLeadDisqualificationReason;
using Fgs.Setup.Application.Features.LeadDisqualificationReasons.Dtos;
using Fgs.Setup.Application.Features.LeadDisqualificationReasons.Validators;
using Moq;

namespace Fgs.Setup.Tests.LeadDisqualificationReasons;

public sealed class LeadDisqualificationReasonValidatorTests
{
    private readonly Mock<ILeadDisqualificationReasonReadRepository> _readRepository = new();

    [Fact]
    public async Task CreateValidator_WhenReasonCodeMissing_HasValidationError()
    {
        var validator = new CreateLeadDisqualificationReasonCommandValidator(_readRepository.Object);
        var command = new CreateLeadDisqualificationReasonCommand(new LeadDisqualificationReasonCreateDto("", "ReasonName value", "Description value", 1, false));

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Dto.ReasonCode");
    }

    [Fact]
    public async Task CreateValidator_WhenReasonCodeNotUppercase_HasValidationError()
    {
        var validator = new CreateLeadDisqualificationReasonCommandValidator(_readRepository.Object);
        var args = new LeadDisqualificationReasonCreateDto("TEST", "ReasonName value", "Description value", 1, false);
        var command = new CreateLeadDisqualificationReasonCommand(args with { ReasonCode = "test" });

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Dto.ReasonCode");
    }

    [Fact]
    public async Task UpdateValidator_WhenDuplicateCodeExcludesCurrentId_Passes()
    {

        _readRepository
            .Setup(r => r.ExistsByReasonCodeAsync("TEST", 5, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
        _readRepository
            .Setup(r => r.ExistsByReasonNameAsync(It.IsAny<string>(), 5, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
        var validator = new UpdateLeadDisqualificationReasonCommandValidator(_readRepository.Object);
        var command = new UpdateLeadDisqualificationReasonCommand(5, new LeadDisqualificationReasonUpdateDto("TEST", "ReasonName value", "Description value", 1, false));

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeTrue();
    }
}
