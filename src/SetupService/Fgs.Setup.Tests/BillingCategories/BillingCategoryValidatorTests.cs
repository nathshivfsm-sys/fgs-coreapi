using Fgs.Setup.Application.Abstractions.BillingCategories;
using Fgs.Setup.Application.Abstractions.GloLookups;
using Fgs.Setup.Application.Features.BillingCategories.Commands.CreateBillingCategory;
using Fgs.Setup.Application.Features.BillingCategories.Commands.DeleteBillingCategory;
using Fgs.Setup.Application.Features.BillingCategories.Commands.PatchBillingCategory;
using Fgs.Setup.Application.Features.BillingCategories.Commands.UpdateBillingCategory;
using Fgs.Setup.Application.Features.BillingCategories.Dtos;
using Fgs.Setup.Application.Features.BillingCategories.Validators;
using Moq;

namespace Fgs.Setup.Tests.BillingCategories;

public sealed class BillingCategoryValidatorTests
{
    private readonly Mock<IBillingCategoryReadRepository> _readRepository = new();
    private readonly Mock<IGloBillingCategoryReadRepository> _gloReadRepository = new();

    [Fact]
    public async Task CreateValidator_WhenBillingCategoryTypeMissing_HasValidationError()
    {
        var validator = CreateCreateValidator();
        var command = new CreateBillingCategoryCommand(
            new BillingCategoryCreateDto("", "BillingCategoryName", "Description value", 1, false, false, true));

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Dto.BillingCategoryType");
    }

    [Fact]
    public async Task CreateValidator_WhenBillingCategoryTypeNotUppercase_HasValidationError()
    {
        SetupGloTypeExists();
        var validator = CreateCreateValidator();
        var command = new CreateBillingCategoryCommand(
            new BillingCategoryCreateDto("lb", "BillingCategoryName", "Description value", 1, false, false, true));

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Dto.BillingCategoryType");
    }

    [Fact]
    public async Task CreateValidator_WhenBillingCategoryTypeUnknown_HasValidationError()
    {
        _gloReadRepository
            .Setup(r => r.ExistsByBillingCategoryTypeAsync("ZZ", It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
        SetupDuplicateDoesNotExist();
        var validator = CreateCreateValidator();
        var command = new CreateBillingCategoryCommand(
            new BillingCategoryCreateDto("ZZ", "BillingCategoryName", "Description value", 1, false, false, true));

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e =>
            e.PropertyName == "Dto.BillingCategoryType"
            && e.ErrorMessage == "BillingCategoryType is not a valid billing category type.");
    }

    [Fact]
    public async Task CreateValidator_WhenDescriptionExceeds700_HasValidationError()
    {
        SetupGloTypeExists();
        SetupDuplicateDoesNotExist();
        var validator = CreateCreateValidator();
        var command = new CreateBillingCategoryCommand(
            new BillingCategoryCreateDto("LB", "BillingCategoryName", new string('x', 701), 1, false, false, true));

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Dto.Description");
    }

    [Fact]
    public async Task CreateValidator_WhenValidLaborTypeAndDescription_Passes()
    {
        SetupGloTypeExists();
        SetupDuplicateDoesNotExist();
        var validator = CreateCreateValidator();
        var command = new CreateBillingCategoryCommand(
            new BillingCategoryCreateDto("LB", "Labor", new string('x', 700), 1, false, true, true));

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public async Task UpdateValidator_WhenDuplicateCodeExcludesCurrentId_Passes()
    {
        SetupGloTypeExists();
        SetupDuplicateDoesNotExist();
        var validator = new UpdateBillingCategoryCommandValidator(
            _readRepository.Object,
            _gloReadRepository.Object);
        var command = new UpdateBillingCategoryCommand(
            5,
            new BillingCategoryUpdateDto("LB", "BillingCategoryName", "Description value", 1, false, false, true));

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public async Task UpdateValidator_WhenSystemDefined_HasValidationError()
    {
        _readRepository
            .Setup(r => r.GetByIdAsync(5, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new BillingCategoryDetailDto(5, "LB", "Labor", "Seeded", 1, true, true, true, true));
        SetupGloTypeExists();
        SetupDuplicateDoesNotExist();
        var validator = new UpdateBillingCategoryCommandValidator(
            _readRepository.Object,
            _gloReadRepository.Object);
        var command = new UpdateBillingCategoryCommand(
            5,
            new BillingCategoryUpdateDto("LB", "Labor - Updated", "Description value", 1, false, false, true));

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e =>
            e.PropertyName == "Id"
            && e.ErrorMessage == "System-defined billing categories cannot be edited.");
    }

    [Fact]
    public async Task PatchValidator_WhenSystemDefined_HasValidationError()
    {
        _readRepository
            .Setup(r => r.GetByIdAsync(5, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new BillingCategoryDetailDto(5, "LB", "Labor", "Seeded", 1, true, true, true, true));
        var validator = new PatchBillingCategoryCommandValidator(
            _readRepository.Object,
            _gloReadRepository.Object);
        var command = new PatchBillingCategoryCommand(
            5,
            new BillingCategoryPatchDto(null, null, null, null, null, false, null, null));

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e =>
            e.PropertyName == "Id"
            && e.ErrorMessage == "System-defined billing categories cannot be edited.");
    }

    [Fact]
    public async Task DeleteValidator_WhenSystemDefined_HasValidationError()
    {
        _readRepository
            .Setup(r => r.GetByIdAsync(5, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new BillingCategoryDetailDto(5, "LB", "Labor", "Seeded", 1, true, true, true, true));
        var validator = new DeleteBillingCategoryCommandValidator(_readRepository.Object);

        var result = await validator.ValidateAsync(new DeleteBillingCategoryCommand(5));

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e =>
            e.PropertyName == "Id"
            && e.ErrorMessage == "System-defined billing categories cannot be edited.");
    }

    private CreateBillingCategoryCommandValidator CreateCreateValidator() =>
        new(_readRepository.Object, _gloReadRepository.Object);

    private void SetupGloTypeExists() =>
        _gloReadRepository
            .Setup(r => r.ExistsByBillingCategoryTypeAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

    private void SetupDuplicateDoesNotExist() =>
        _readRepository
            .Setup(r => r.ExistsByBillingCategoryTypeAndBillingCategoryNameAsync(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<long?>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
}
