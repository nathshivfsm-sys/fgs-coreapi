using Fgs.User.Infrastructure.Common.Security;

namespace Fgs.User.Tests.Infrastructure;

public sealed class EmailNormalizerTests
{
    private readonly EmailNormalizer _normalizer = new();

    [Fact]
    public void Normalize_TrimsAndUppercases()
    {
        _normalizer.Normalize("  Admin@Acme.com ").Should().Be("ADMIN@ACME.COM");
    }
}
