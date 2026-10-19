using Fgs.File.Application.Common.Options;
using Fgs.File.Application.Features.Attachments.Commands.UploadAttachment;
using FluentValidation.TestHelper;
using Microsoft.Extensions.Options;

namespace Fgs.File.Tests;

public sealed class UploadAttachmentCommandValidatorTests
{
    private static UploadAttachmentCommandValidator CreateValidator() =>
        new(Options.Create(new AttachmentValidationOptions()));

    private static UploadAttachmentCommand CreateValidCommand() =>
        new(
            Stream.Null,
            "photo.jpg",
            "image/jpeg",
            1024,
            "WorkOrder",
            100,
            "general",
            null,
            null,
            true,
            true,
            null);

    [Fact]
    public void ValidCommand_PassesValidation()
    {
        var result = CreateValidator().TestValidate(CreateValidCommand());
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void EmployeeEntityType_PassesValidation()
    {
        var command = CreateValidCommand() with { EntityType = "Employee" };
        var result = CreateValidator().TestValidate(command);
        result.ShouldNotHaveValidationErrorFor(x => x.EntityType);
    }

    [Fact]
    public void UnsupportedEntityType_FailsValidation()
    {
        var command = CreateValidCommand() with { EntityType = "Unknown" };
        var result = CreateValidator().TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.EntityType)
            .WithErrorMessage("Unsupported entity type.");
    }

    [Fact]
    public void UnsupportedExtension_FailsValidation()
    {
        var command = CreateValidCommand() with { OriginalFileName = "malware.exe" };
        var result = CreateValidator().TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.OriginalFileName);
    }

    [Fact]
    public void LogoVariantRequired_WhenCategoryIsLogoAndVariantInvalid()
    {
        var command = CreateValidCommand() with { Category = "logo", LogoVariant = "invalid" };
        var result = CreateValidator().TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.LogoVariant);
    }
}
