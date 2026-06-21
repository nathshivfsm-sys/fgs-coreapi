using Fgs.Setup.Application.Abstractions.Tags;
using Fgs.Setup.Application.Features.Tags.Commands.CreateFgsTag;
using Fgs.Setup.Application.Features.Tags.Commands.PatchFgsTag;
using Fgs.Setup.Application.Features.Tags.Commands.UpdateFgsTag;
using Fgs.Setup.Application.Features.Tags.Dtos;
using Fgs.Setup.Application.Features.Tags.Validators;
using Moq;

namespace Fgs.Setup.Tests.Tags;

public sealed class FgsTagValidatorTests
{
    private readonly Mock<IFgsTagReadRepository> _readRepository = new();

    [Fact]
    public async Task UpdateValidator_WhenDuplicateCodeExcludesCurrentId_Passes()
    {

        var validator = new UpdateFgsTagCommandValidator(_readRepository.Object);
        var command = new UpdateFgsTagCommand(5, new FgsTagUpdateDto("TEST", "Name", "Description value", "BackgroundColor", "TextColor", null));

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeTrue();
    }
}
