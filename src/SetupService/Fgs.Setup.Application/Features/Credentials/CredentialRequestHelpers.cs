using System.Text;
using Fgs.Foundation.Result;
using Fgs.Setup.Application.Features.Credentials.DTOs;
using Fgs.Setup.Domain.Enums;

namespace Fgs.Setup.Application.Features.Credentials;

internal static class CredentialRequestHelpers
{
    public static byte[] ParsePayload(string payload)
    {
        if (string.IsNullOrWhiteSpace(payload))
        {
            throw new InvalidOperationException(CredentialErrorMessages.InvalidPayload);
        }

        return Encoding.UTF8.GetBytes(payload);
    }

    public static ApiResponse<T> MapException<T>(Exception exception) =>
        exception switch
        {
            InvalidOperationException invalidOperation => ApiResponse<T>.Fail(
                [invalidOperation.Message],
                invalidOperation.Message.Contains("not found", StringComparison.OrdinalIgnoreCase)
                    ? ApiStatusCodes.NotFound
                    : ApiStatusCodes.Conflict),
            _ => ApiResponse<T>.Fail([exception.Message], ApiStatusCodes.InternalServerError)
        };

    public static bool TryParseGlobalId(string id, out int parsed) => int.TryParse(id, out parsed);

    public static bool TryParseTenantId(string id, out Guid parsed) => Guid.TryParse(id, out parsed);

    public static CredentialMutationResultDto ToMutationResult(CredentialScope scope, string id, string providerCode, string credentialName) =>
        new(scope, id, providerCode, credentialName);
}
