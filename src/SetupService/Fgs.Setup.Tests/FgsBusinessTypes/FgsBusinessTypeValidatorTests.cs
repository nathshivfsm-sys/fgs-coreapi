using Fgs.Setup.Application.Abstractions.FgsBusinessTypes;
using Fgs.Setup.Application.Features.FgsBusinessTypes.Commands.CreateFgsBusinessType;
using Fgs.Setup.Application.Features.FgsBusinessTypes.Commands.PatchFgsBusinessType;
using Fgs.Setup.Application.Features.FgsBusinessTypes.Commands.UpdateFgsBusinessType;
using Fgs.Setup.Application.Features.FgsBusinessTypes.Dtos;
using Fgs.Setup.Application.Features.FgsBusinessTypes.Validators;
using Moq;

namespace Fgs.Setup.Tests.FgsBusinessTypes;

public sealed class FgsBusinessTypeValidatorTests
{
    private readonly Mock<IFgsBusinessTypeReadRepository> _readRepository = new();

    [Fact]
    public async Task CreateValidator_WhenCodeMissing_HasValidationError()
    {
        var validator = new CreateFgsBusinessTypeCommandValidator(_readRepository.Object);
        var command = new CreateFgsBusinessTypeCommand(new FgsBusinessTypeCreateDto("", "Name value", "Description value", 1));

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Dto.Code");
    }

    [Fact]
    public async Task CreateValidator_WhenCodeNotUppercase_HasValidationError()
    {
        var validator = new CreateFgsBusinessTypeCommandValidator(_readRepository.Object);
        var args = new FgsBusinessTypeCreateDto("TEST", "Name value", "Description value", 1);
        var command = new CreateFgsBusinessTypeCommand(args with { Code = "test" });

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Dto.Code");
    }

    [Fact]
    public async Task UpdateValidator_WhenDuplicateCodeExcludesCurrentId_Passes()
    {

        _readRepository
            .Setup(r => r.ExistsByCodeAsync("TEST", 5, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
        var validator = new UpdateFgsBusinessTypeCommandValidator(_readRepository.Object);
        var command = new UpdateFgsBusinessTypeCommand(5, new FgsBusinessTypeUpdateDto("TEST", "Name value", "Description value", 1));

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeTrue();
    }
}
