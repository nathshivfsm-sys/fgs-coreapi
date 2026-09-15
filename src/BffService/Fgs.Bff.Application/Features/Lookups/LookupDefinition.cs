namespace Fgs.Bff.Application.Features.Lookups;

public sealed record LookupDefinition(
    LookupKey Key,
    string Service,
    string RelativePath,
    bool RequiresTenant,
    IReadOnlyList<string> RequiredFilters,
    IReadOnlyList<string> OptionalFilters,
    string Description);
