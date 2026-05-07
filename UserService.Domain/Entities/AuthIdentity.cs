namespace UserService.Domain.Entities;

public sealed class AuthIdentity
{
    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public string Issuer { get; private set; } = null!;
    public string ObjectId { get; private set; } = null!;
    public string? Subject { get; private set; }
    public string? EmailSnapshot { get; private set; }
    public DateTimeOffset LinkedAt { get; private set; }

    public User User { get; private set; } = null!;

    private AuthIdentity()
    {
    }

    public static AuthIdentity LinkEntraUser(
        Guid userId,
        string issuer,
        string objectId,
        string? subject,
        string? emailSnapshot)
    {
        return new AuthIdentity
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Issuer = issuer,
            ObjectId = objectId,
            Subject = subject,
            EmailSnapshot = emailSnapshot,
            LinkedAt = DateTimeOffset.UtcNow
        };
    }
}
