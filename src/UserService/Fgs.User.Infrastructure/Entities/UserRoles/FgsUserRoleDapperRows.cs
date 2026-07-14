using Fgs.User.Application.Features.UserRoles.Dtos;

namespace Fgs.User.Infrastructure.Entities.UserRoles;

internal sealed class FgsUserRoleRow
{
    public long Id { get; set; }
    public Guid UserId { get; set; }
    public long FgsRoleId { get; set; }
    public DateTimeOffset CreatedOn { get; set; }
    public string CreatedBy { get; set; } = null!;

    public FgsUserRoleSummaryDto ToSummaryDto() => new(Id, UserId, FgsRoleId, CreatedOn, CreatedBy);
    public FgsUserRoleDetailDto ToDetailDto() => new(Id, UserId, FgsRoleId, CreatedOn, CreatedBy);
}
