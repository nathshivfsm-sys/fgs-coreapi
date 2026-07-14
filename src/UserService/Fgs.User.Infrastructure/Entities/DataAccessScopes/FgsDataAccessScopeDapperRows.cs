using Fgs.User.Application.Features.DataAccessScopes.Dtos;

namespace Fgs.User.Infrastructure.Entities.DataAccessScopes;

internal sealed class FgsDataAccessScopeSummaryRow
{
    public long Id { get; set; }

    public long FgsDataAccessId { get; set; }

    public string ScopeType { get; set; } = null!;

    public string Operator { get; set; } = null!;

    public string? ScopeValue { get; set; }

    public short DisplayOrder { get; set; }

    public FgsDataAccessScopeSummaryDto ToDto() =>
        new(Id, FgsDataAccessId, ScopeType, Operator, ScopeValue, DisplayOrder);
}

internal sealed class FgsDataAccessScopeDetailRow
{
    public long Id { get; set; }

    public long FgsDataAccessId { get; set; }

    public string ScopeType { get; set; } = null!;

    public string Operator { get; set; } = null!;

    public string? ScopeValue { get; set; }

    public short DisplayOrder { get; set; }

    public FgsDataAccessScopeDetailDto ToDto() =>
        new(Id, FgsDataAccessId, ScopeType, Operator, ScopeValue, DisplayOrder);
}
