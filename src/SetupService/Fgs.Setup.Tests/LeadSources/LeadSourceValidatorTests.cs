using Fgs.Setup.Application.Abstractions.LeadSources;
using Fgs.Setup.Application.Features.LeadSources.Commands.CreateLeadSource;
using Fgs.Setup.Application.Features.LeadSources.Commands.PatchLeadSource;
using Fgs.Setup.Application.Features.LeadSources.Commands.UpdateLeadSource;
using Fgs.Setup.Application.Features.LeadSources.Dtos;
using Fgs.Setup.Application.Features.LeadSources.Validators;
using Moq;

namespace Fgs.Setup.Tests.LeadSources;

public sealed class LeadSourceValidatorTests
{
    private readonly Mock<ILeadSourceReadRepository> _readRepository = new();

    [Fact]
    public async Task CreateValidator_WhenSourceCodeMissing_HasValidationError()
    {
        var validator = new CreateLeadSourceCommandValidator(_readRepository.Object);
        var command = new CreateLeadSourceCommand(new LeadSourceCreateDto("", "SourceName value", "Description value"));

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Dto.SourceCode");
    }

    [Fact]
    public async Task CreateValidator_WhenSourceCodeNotUppercase_HasValidationError()
    {
        var validator = new CreateLeadSourceCommandValidator(_readRepository.Object);
        var args = new LeadSourceCreateDto("TEST", "SourceName value", "Description value");
        var command = new CreateLeadSourceCommand(args with { SourceCode = "test" });

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Dto.SourceCode");
    }

    [Fact]
    public async Task UpdateValidator_WhenDuplicateCodeExcludesCurrentId_Passes()
    {

        _readRepository
            .Setup(r => r.ExistsBySourceCodeAsync("TEST", 5, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
        var validator = new UpdateLeadSourceCommandValidator(_readRepository.Object);
        var command = new UpdateLeadSourceCommand(5, new LeadSourceUpdateDto("TEST", "SourceName value", "Description value"));

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeTrue();
    }
}
