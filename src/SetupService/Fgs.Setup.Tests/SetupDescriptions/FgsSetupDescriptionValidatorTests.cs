using Fgs.Setup.Application.Abstractions.SetupDescriptions;
using Fgs.Setup.Application.Features.SetupDescriptions.Commands.CreateFgsSetupDescription;
using Fgs.Setup.Application.Features.SetupDescriptions.Commands.PatchFgsSetupDescription;
using Fgs.Setup.Application.Features.SetupDescriptions.Commands.UpdateFgsSetupDescription;
using Fgs.Setup.Application.Features.SetupDescriptions.Dtos;
using Fgs.Setup.Application.Features.SetupDescriptions.Validators;
using Moq;

namespace Fgs.Setup.Tests.SetupDescriptions;

public sealed class FgsSetupDescriptionValidatorTests
{
    private readonly Mock<IFgsSetupDescriptionReadRepository> _readRepository = new();

    [Fact]
    public async Task CreateValidator_WhenDescriptionTypeCodeMissing_HasValidationError()
    {
        var validator = new CreateFgsSetupDescriptionCommandValidator(_readRepository.Object);
        var command = new CreateFgsSetupDescriptionCommand(new FgsSetupDescriptionCreateDto("", "ShortNote", "Body value", null, 1));

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Dto.DescriptionTypeCode");
    }

    [Fact]
    public async Task UpdateValidator_WhenDuplicateCodeExcludesCurrentId_Passes()
    {

        _readRepository
            .Setup(r => r.ExistsByDescriptionTypeCodeAsync("TEST", 5, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
        _readRepository
            .Setup(r => r.ExistsTechTradeIdAsync(It.IsAny<long>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);
        var validator = new UpdateFgsSetupDescriptionCommandValidator(_readRepository.Object);
        var command = new UpdateFgsSetupDescriptionCommand(5, new FgsSetupDescriptionUpdateDto("DescriptionTypeCode value", "ShortNote", "Body value", null, 1));

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeTrue();
    }
}
