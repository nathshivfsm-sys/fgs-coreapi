using System.Text.Json;

namespace Fgs.User.Application.Abstractions.Credentials;

public interface ICredentialPayloadDeserializer
{
    T Deserialize<T>(string providerTypeCode, string secretJson) where T : class;

    object Deserialize(string providerTypeCode, string secretJson, Type payloadType);
}
