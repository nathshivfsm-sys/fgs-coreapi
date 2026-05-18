namespace Fgs.User.Domain.Enums;

public enum OutboxMessageStatus
{
    Pending = 1,
    Processing = 2,
    Published = 3,
    Failed = 4
}
