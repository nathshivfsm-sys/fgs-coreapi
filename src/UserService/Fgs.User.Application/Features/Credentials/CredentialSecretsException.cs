namespace Fgs.User.Application.Features.Credentials;

/// <summary>
/// Raised when AWS Secrets Manager operations fail. Never includes secret values in the message.
/// </summary>
public sealed class CredentialSecretsException : Exception
{
    public CredentialSecretsException(
        string message,
        string? awsErrorCode = null,
        bool isAccessDenied = false,
        Exception? innerException = null)
        : base(message, innerException)
    {
        AwsErrorCode = awsErrorCode;
        IsAccessDenied = isAccessDenied;
    }

    public string? AwsErrorCode { get; }

    public bool IsAccessDenied { get; }
}
