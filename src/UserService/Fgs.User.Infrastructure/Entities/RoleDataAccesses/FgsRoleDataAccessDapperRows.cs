using Fgs.User.Application.Features.RoleDataAccesses.Dtos;

namespace Fgs.User.Infrastructure.Entities.RoleDataAccesses;

internal sealed class FgsRoleDataAccessRow
{
    public long Id { get; set; }

    public long FgsRoleId { get; set; }

    public long FgsDataAccessId { get; set; }

    public DateTimeOffset CreatedOn { get; set; }

    public string CreatedBy { get; set; } = null!;

    public FgsRoleDataAccessSummaryDto ToSummaryDto() =>
        new(Id, FgsRoleId, FgsDataAccessId, CreatedOn, CreatedBy);

    public FgsRoleDataAccessDetailDto ToDetailDto() =>
        new(Id, FgsRoleId, FgsDataAccessId, CreatedOn, CreatedBy);
}
