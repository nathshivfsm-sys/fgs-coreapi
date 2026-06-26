using Fgs.Setup.Application.Abstractions.CommunicationTemplates;
using Fgs.Setup.Application.Features.CommunicationTemplates.Commands.CreateFgsSetupCommunicationTemplate;
using Fgs.Setup.Application.Features.CommunicationTemplates.Commands.PatchFgsSetupCommunicationTemplate;
using Fgs.Setup.Application.Features.CommunicationTemplates.Commands.UpdateFgsSetupCommunicationTemplate;
using Fgs.Setup.Application.Features.CommunicationTemplates.Dtos;
using Fgs.Setup.Application.Features.CommunicationTemplates.Validators;
using Moq;

namespace Fgs.Setup.Tests.CommunicationTemplates;

public sealed class FgsSetupCommunicationTemplateValidatorTests
{
    private readonly Mock<IFgsSetupCommunicationTemplateReadRepository> _readRepository = new();

    [Fact]
    public async Task CreateValidator_WhenCodeMissing_HasValidationError()
    {
        var validator = new CreateFgsSetupCommunicationTemplateCommandValidator(_readRepository.Object);
        var command = new CreateFgsSetupCommunicationTemplateCommand(new FgsSetupCommunicationTemplateCreateDto("Email", "TemplateType value", "", "Name value", "Subject value", "Body value", true));

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Dto.Code");
    }

    [Fact]
    public async Task UpdateValidator_WhenDuplicateCodeExcludesCurrentId_Passes()
    {

        _readRepository
            .Setup(r => r.ExistsByCommunicationChannelAndTemplateTypeAndCodeAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), 5, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
        var validator = new UpdateFgsSetupCommunicationTemplateCommandValidator(_readRepository.Object);
        var command = new UpdateFgsSetupCommunicationTemplateCommand(5, new FgsSetupCommunicationTemplateUpdateDto("Email", "TemplateType value", "Code value", "Name value", "Subject value", "Body value", true));

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeTrue();
    }
}
