namespace Fgs.User.Application.Credentials;

public static class CredentialAuditRemarks
{
    private const string CorrelationPrefix = "cid:";

    public static string Format(Guid? correlationId, string? remarks)
    {
        if (correlationId is null)
        {
            return remarks ?? string.Empty;
        }

        var prefix = $"{CorrelationPrefix}{correlationId:N}";
        return string.IsNullOrWhiteSpace(remarks) ? prefix : $"{prefix}|{remarks}";
    }
}
