using Fgs.Setup.Application.Abstractions.SetupPaymentTerms;
using Fgs.Setup.Application.Features.SetupPaymentTerms.Commands.CreateFgsSetupPaymentTerm;
using Fgs.Setup.Application.Features.SetupPaymentTerms.Commands.PatchFgsSetupPaymentTerm;
using Fgs.Setup.Application.Features.SetupPaymentTerms.Commands.UpdateFgsSetupPaymentTerm;
using Fgs.Setup.Application.Features.SetupPaymentTerms.Dtos;
using Fgs.Setup.Application.Features.SetupPaymentTerms.Validators;
using Moq;

namespace Fgs.Setup.Tests.SetupPaymentTerms;

public sealed class FgsSetupPaymentTermValidatorTests
{
    private readonly Mock<IFgsSetupPaymentTermReadRepository> _readRepository = new();

    [Fact]
    public async Task CreateValidator_WhenNameMissing_HasValidationError()
    {
        var validator = new CreateFgsSetupPaymentTermCommandValidator(_readRepository.Object);
        var command = new CreateFgsSetupPaymentTermCommand(new FgsSetupPaymentTermCreateDto("", "DueDateMethod value", 60, true, true, true));

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Dto.Name");
    }

    [Fact]
    public async Task UpdateValidator_WhenDuplicateCodeExcludesCurrentId_Passes()
    {

        _readRepository
            .Setup(r => r.ExistsByNameAsync(It.IsAny<string>(), 5, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
        var validator = new UpdateFgsSetupPaymentTermCommandValidator(_readRepository.Object);
        var command = new UpdateFgsSetupPaymentTermCommand(5, new FgsSetupPaymentTermUpdateDto("Name value", "DueDateMethod value", 60, true, true, true));

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeTrue();
    }
}
