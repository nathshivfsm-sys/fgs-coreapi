using Fgs.File.Application.Common;
using Fgs.File.Application.Common.Options;
using Fgs.File.Infrastructure.Storage;

namespace Fgs.File.Tests;

public sealed class AttachmentDeletionTagsTests
{
    [Fact]
    public void IsActive_ReturnsFalseWhenDeletedTagPresent()
    {
        Assert.False(AttachmentDeletionTags.IsActive(["category:invoice", "deleted"]));
    }

    [Fact]
    public void MarkDeleted_AddsDeletedTag()
    {
        var tags = AttachmentDeletionTags.MarkDeleted(["category:logo"]);
        Assert.Contains("deleted", tags);
        Assert.Contains("category:logo", tags);
    }
}

public sealed class AttachmentCategoryTagsTests
{
    [Fact]
    public void ToTag_FormatsCategoryPrefix()
    {
        Assert.Equal("category:general", AttachmentCategoryTags.ToTag("General"));
    }

    [Fact]
    public void TryGetCategory_ReturnsCategoryFromTags()
    {
        var tags = new[] { "category:logo", "full" };
        Assert.True(AttachmentCategoryTags.TryGetCategory(tags, out var category));
        Assert.Equal("logo", category);
    }

    [Fact]
    public void MergeTags_IncludesCategoryAndUserTags()
    {
        var tags = AttachmentCategoryTags.MergeTags("invoice", ["urgent", "category:ignored"]);
        Assert.Contains("category:invoice", tags);
        Assert.Contains("urgent", tags);
        Assert.DoesNotContain("category:ignored", tags);
    }
}

public sealed class S3ObjectKeyBuilderTests
{
    [Fact]
    public void BuildThumbnailKey_UsesOriginalFileNameWithThumbnailSuffix()
    {
        var builder = new S3ObjectKeyBuilder();
        var mainKey = builder.BuildCompanyAssetKey(10, "WorkOrder", 42, "stored-abc.pdf");
        var thumbKey = builder.BuildThumbnailKey(mainKey, "invoice.pdf");

        Assert.Equal("company-assets/10/WorkOrder/42/invoice_thumbnail.pdf", thumbKey);
    }

    [Fact]
    public void BuildThumbnailKey_PreservesImageExtension()
    {
        var builder = new S3ObjectKeyBuilder();
        var mainKey = builder.BuildCompanyAssetKey(1, "Company", 1, "photo-guid.jpg");
        var thumbKey = builder.BuildThumbnailKey(mainKey, "photo.jpg");

        Assert.EndsWith("photo_thumbnail.jpg", thumbKey);
    }
}

public sealed class AttachmentFileValidatorTests
{
    private static AttachmentValidationOptions CreateOptions() => new();

    [Fact]
    public void IsAllowedExtension_AcceptsPdf()
    {
        Assert.True(AttachmentFileValidator.IsAllowedExtension("report.pdf", CreateOptions()));
    }

    [Fact]
    public void IsAllowedExtension_RejectsUnknownExtension()
    {
        Assert.False(AttachmentFileValidator.IsAllowedExtension("script.exe", CreateOptions()));
    }

    [Fact]
    public void BuildStoredFileName_SanitizesAndAddsGuid()
    {
        var stored = AttachmentFileValidator.BuildStoredFileName("My Report.pdf");
        Assert.EndsWith(".pdf", stored);
        Assert.DoesNotContain(" ", stored);
    }
}

public sealed class AttachmentThumbnailGeneratorTests
{
    [Fact]
    public async Task GenerateAsync_CreatesPngIconForPdf()
    {
        var generator = new AttachmentThumbnailGenerator();
        await using var pdfHeader = new MemoryStream([0x25, 0x50, 0x44, 0x46, 0x2D, 0x31, 0x2E, 0x34]);

        var result = await generator.GenerateAsync(pdfHeader, "application/pdf", "invoice.pdf");

        Assert.NotNull(result);
        Assert.Equal("invoice_thumbnail.png", result!.ThumbnailFileName);
        Assert.Equal("image/png", result.ContentType);
        Assert.NotEmpty(result.Content);
    }
}
