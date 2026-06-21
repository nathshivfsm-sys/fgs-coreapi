using Fgs.Setup.Application.Abstractions.SalesActivityTypes;
using Fgs.Setup.Application.Features.SalesActivityTypes.Commands.CreateFgsSalesActivityType;
using Fgs.Setup.Application.Features.SalesActivityTypes.Commands.PatchFgsSalesActivityType;
using Fgs.Setup.Application.Features.SalesActivityTypes.Commands.UpdateFgsSalesActivityType;
using Fgs.Setup.Application.Features.SalesActivityTypes.Dtos;
using Fgs.Setup.Application.Features.SalesActivityTypes.Validators;
using Moq;

namespace Fgs.Setup.Tests.SalesActivityTypes;

public sealed class FgsSalesActivityTypeValidatorTests
{
    private readonly Mock<IFgsSalesActivityTypeReadRepository> _readRepository = new();

    [Fact]
    public async Task CreateValidator_WhenActivityTypeCodeMissing_HasValidationError()
    {
        var validator = new CreateFgsSalesActivityTypeCommandValidator(_readRepository.Object);
        var command = new CreateFgsSalesActivityTypeCommand(new FgsSalesActivityTypeCreateDto("", "ActivityTypeName", "Description", 5, false, true, true, true));

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Dto.ActivityTypeCode");
    }

    [Fact]
    public async Task CreateValidator_WhenActivityTypeCodeNotUppercase_HasValidationError()
    {
        var validator = new CreateFgsSalesActivityTypeCommandValidator(_readRepository.Object);
        var args = new FgsSalesActivityTypeCreateDto("TEST", "ActivityTypeName", "Description", 5, false, true, true, true);
        var command = new CreateFgsSalesActivityTypeCommand(args with { ActivityTypeCode = "test" });

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Dto.ActivityTypeCode");
    }

    [Fact]
    public async Task UpdateValidator_WhenDuplicateCodeExcludesCurrentId_Passes()
    {

        _readRepository
            .Setup(r => r.ExistsByActivityTypeCodeAsync("TEST", 5, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
        _readRepository
            .Setup(r => r.ExistsByActivityTypeNameAsync(It.IsAny<string>(), 5, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
        var validator = new UpdateFgsSalesActivityTypeCommandValidator(_readRepository.Object);
        var command = new UpdateFgsSalesActivityTypeCommand(5, new FgsSalesActivityTypeUpdateDto("TEST", "ActivityTypeName", "Description", 5, false, true, true, true));

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeTrue();
    }
}
