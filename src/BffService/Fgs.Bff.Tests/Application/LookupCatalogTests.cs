using Fgs.Bff.Application.Features.Lookups;
using Fgs.Bff.Application.Features.Lookups.Queries.ListLookupKeys;

namespace Fgs.Bff.Tests.Application;

public sealed class LookupCatalogTests
{
    [Fact]
    public void Catalog_HasDefinitionForEveryLookupKey()
    {
        foreach (var key in Enum.GetValues<LookupKey>())
        {
            LookupCatalog.TryGet(key, out var definition).Should().BeTrue($"missing catalog entry for {key}");
            definition.Key.Should().Be(key);
            definition.Service.Should().NotBeNullOrWhiteSpace();
            definition.RelativePath.Should().NotBeNullOrWhiteSpace();
        }
    }

    [Fact]
    public async Task ListLookupKeys_CountMatchesCatalogAndEnum()
    {
        var handler = new ListLookupKeysQueryHandler();
        var response = await handler.Handle(new ListLookupKeysQuery(), CancellationToken.None);

        response.Success.Should().BeTrue();
        response.Data.Should().HaveCount(LookupCatalog.All.Count);
        response.Data.Should().HaveCount(Enum.GetValues<LookupKey>().Length);
    }
}
