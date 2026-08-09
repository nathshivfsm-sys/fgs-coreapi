using Fgs.Setup.Application.Abstractions.SetupLaborRateTypes;
using Fgs.Setup.Application.Abstractions.SetupPricingMatrices;
using Fgs.Setup.Application.Abstractions.SetupTechSkillLevels;
using Fgs.Setup.Application.Features.SetupLaborRateTypes.Dtos;
using Fgs.Setup.Application.Features.SetupPricingMatrices.Dtos;
using Fgs.Setup.Application.Features.SetupPricingMatrixLabors.Commands.CreateFgsSetupPricingMatrixLabor;
using Fgs.Setup.Application.Features.SetupPricingMatrixLabors.Dtos;
using Fgs.Setup.Application.Features.SetupPricingMatrixLabors.Validators;
using Fgs.Setup.Application.Features.SetupTechSkillLevels.Dtos;
using Moq;

namespace Fgs.Setup.Tests.SetupPricingMatrixLabors;

public sealed class FgsSetupPricingMatrixLaborValidatorTests
{
    private readonly Mock<IFgsSetupPricingMatrixReadRepository> _matrices = new();
    private readonly Mock<IFgsSetupLaborRateTypeReadRepository> _rateTypes = new();
    private readonly Mock<IFgsSetupTechSkillLevelReadRepository> _skills = new();

    [Fact]
    public async Task CreateValidator_WhenParentMissing_HasPricingMatrixIdError()
    {
        _matrices.Setup(x => x.ExistsByIdAsync(99, It.IsAny<CancellationToken>())).ReturnsAsync(false);
        SetupRateType();

        var result = await Validator().ValidateAsync(
            new CreateFgsSetupPricingMatrixLaborCommand(
                new FgsSetupPricingMatrixLaborCreateDto(99, 1, null, 75m, null, null, null)));

        result.Errors.Should().Contain(e => e.PropertyName == "Dto.PricingMatrixId");
    }

    [Fact]
    public async Task CreateValidator_WhenSkillLevelRequiredButMissing_HasError()
    {
        SetupMatrix(new(1, false, true, 1, true));
        SetupRateType();

        var result = await Validator().ValidateAsync(
            new CreateFgsSetupPricingMatrixLaborCommand(
                new FgsSetupPricingMatrixLaborCreateDto(1, 1, null, 75m, null, null, null)));

        result.Errors.Should().Contain(e =>
            e.PropertyName == "Dto.TechSkillLevelId" && e.ErrorMessage.Contains("required"));
    }

    [Fact]
    public async Task CreateValidator_WhenFlatLaborIsValid_Passes()
    {
        SetupMatrix(new(1, false, false, 1, true));
        SetupRateType();
        _skills.Setup(x => x.GetByIdAsync(It.IsAny<long>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new FgsSetupTechSkillLevelDetailDto(100, "JOURNEY", "Journeyman", null, 1, true));

        var result = await Validator().ValidateAsync(
            new CreateFgsSetupPricingMatrixLaborCommand(
                new FgsSetupPricingMatrixLaborCreateDto(1, 1, null, 75m, 1.5m, 2m, 10m)));

        result.IsValid.Should().BeTrue();
    }

    private void SetupMatrix(FgsSetupPricingMatrixFlagsDto flags)
    {
        _matrices.Setup(x => x.ExistsByIdAsync(flags.Id, It.IsAny<CancellationToken>())).ReturnsAsync(true);
        _matrices.Setup(x => x.GetFlagsByIdAsync(flags.Id, It.IsAny<CancellationToken>())).ReturnsAsync(flags);
    }

    private void SetupRateType() =>
        _rateTypes.Setup(x => x.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new FgsSetupLaborRateTypeDetailDto(1, "Standard", null, 1, true));

    private CreateFgsSetupPricingMatrixLaborCommandValidator Validator() =>
        new(_matrices.Object, _rateTypes.Object, _skills.Object);
}
