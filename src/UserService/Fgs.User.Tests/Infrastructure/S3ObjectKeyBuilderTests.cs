using Fgs.User.Infrastructure.Storage;

namespace Fgs.User.Tests.Infrastructure;

public sealed class S3ObjectKeyBuilderTests
{
    private readonly S3ObjectKeyBuilder _builder = new();

    [Fact]
    public void BuildCompanyAssetKey_UsesExpectedPath()
    {
        var key = _builder.BuildCompanyAssetKey(2001, "work-orders", 80012, "file-1.jpg");
        key.Should().Be("company-assets/2001/work-orders/80012/file-1.jpg");
    }

    [Fact]
    public void CompanyGeneralPrefix_UsesGeneralFolderUnderCompanyAssets()
    {
        S3ObjectKeyBuilder.CompanyGeneralPrefix(42)
            .Should().Be("company-assets/42/General/");
    }

    [Fact]
    public void BuildThumbnailKey_AppendsThumbBeforeExtension()
    {
        var key = _builder.BuildThumbnailKey("company-assets/2001/work-orders/80012/file-1.jpg");
        key.Should().Be("company-assets/2001/work-orders/80012/file-1-thumb.jpg");
    }
}
