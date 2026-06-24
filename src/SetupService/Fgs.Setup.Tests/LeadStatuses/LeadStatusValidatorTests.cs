using Fgs.Setup.Application.Abstractions.LeadStatuses;
using Fgs.Setup.Application.Features.LeadStatuses.Commands.CreateLeadStatus;
using Fgs.Setup.Application.Features.LeadStatuses.Commands.PatchLeadStatus;
using Fgs.Setup.Application.Features.LeadStatuses.Commands.UpdateLeadStatus;
using Fgs.Setup.Application.Features.LeadStatuses.Dtos;
using Fgs.Setup.Application.Features.LeadStatuses.Validators;
using Moq;

namespace Fgs.Setup.Tests.LeadStatuses;

public sealed class LeadStatusValidatorTests
{
    private readonly Mock<ILeadStatusReadRepository> _readRepository = new();

    [Fact]
    public async Task CreateValidator_WhenStatusCodeMissing_HasValidationError()
    {
        var validator = new CreateLeadStatusCommandValidator(_readRepository.Object);
        var command = new CreateLeadStatusCommand(new LeadStatusCreateDto("", "StatusName", "Description", 1, false));

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Dto.StatusCode");
    }

    [Fact]
    public async Task CreateValidator_WhenStatusCodeNotUppercase_HasValidationError()
    {
        var validator = new CreateLeadStatusCommandValidator(_readRepository.Object);
        var args = new LeadStatusCreateDto("TEST", "StatusName", "Description", 1, false);
        var command = new CreateLeadStatusCommand(args with { StatusCode = "test" });

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
        var validator = new UpdateLeadStatusCommandValidator(_readRepository.Object);
        var command = new UpdateLeadStatusCommand(5, new LeadStatusUpdateDto("TEST", "StatusName", "Description", 1, false));

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeTrue();
    }
}
