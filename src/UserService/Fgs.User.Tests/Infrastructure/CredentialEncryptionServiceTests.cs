using Fgs.User.Application.Abstractions.Credentials;
using Fgs.User.Infrastructure.Security.Encryption;
using Moq;

namespace Fgs.User.Tests.Infrastructure;

public sealed class CredentialEncryptionServiceTests
{
    [Fact]
    public async Task EncryptAsync_UsesKmsAndAesServices()
    {
        var kms = new Mock<IKmsService>();
        kms.Setup(x => x.GenerateDataKeyAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new KmsDataKeyResult(
                new byte[32],
                [9, 8, 7],
                "arn:aws:kms:us-east-1:123:key/abc"));

        var aes = new AesGcmEncryptionService();
        var service = new CredentialEncryptionService(kms.Object, aes);
        var plaintext = "{\"token\":\"value\"}"u8.ToArray();

        var result = await service.EncryptAsync(plaintext);

        result.EncryptedDataKey.Should().BeEquivalentTo(new byte[] { 9, 8, 7 });
        result.KeyIdentifier.Should().Be("arn:aws:kms:us-east-1:123:key/abc");
        result.CredentialData.Should().NotBeEmpty();

        kms.Setup(x => x.DecryptDataKeyAsync(result.EncryptedDataKey, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new byte[32]);

        var decrypted = await service.DecryptAsync(result.CredentialData, result.EncryptedDataKey);
        decrypted.Should().BeEquivalentTo(plaintext);
    }
}
