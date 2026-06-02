using System.Security.Cryptography;
using System.Text;
using Fgs.User.Infrastructure.Security.Encryption;

namespace Fgs.User.Tests.Infrastructure;

public sealed class AesGcmEncryptionServiceTests
{
    private readonly AesGcmEncryptionService _service = new();

    [Fact]
    public void EncryptDecrypt_RoundTripsPlaintext()
    {
        var key = RandomNumberGenerator.GetBytes(32);
        var plaintext = Encoding.UTF8.GetBytes("{\"ApiKey\":\"secret-value\"}");

        var envelope = _service.Encrypt(plaintext, key);
        var decrypted = _service.Decrypt(envelope, key);

        decrypted.Should().BeEquivalentTo(plaintext);
    }

    [Fact]
    public void Decrypt_WithWrongKey_ThrowsCryptographicException()
    {
        var key = RandomNumberGenerator.GetBytes(32);
        var wrongKey = RandomNumberGenerator.GetBytes(32);
        var envelope = _service.Encrypt(Encoding.UTF8.GetBytes("payload"), key);

        var act = () => _service.Decrypt(envelope, wrongKey);

        act.Should().Throw<CryptographicException>();
    }
}
