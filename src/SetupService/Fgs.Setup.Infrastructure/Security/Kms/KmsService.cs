using Amazon.KeyManagementService;
using Amazon.KeyManagementService.Model;
using Fgs.Setup.Application.Abstractions.Credentials;
using Fgs.Setup.Infrastructure.Common.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Fgs.Setup.Infrastructure.Security.Kms;

public sealed class KmsService : IKmsService
{
    private readonly IAmazonKeyManagementService _kmsClient;
    private readonly AwsCredentialsOptions _options;
    private readonly ILogger<KmsService> _logger;

    public KmsService(
        IAmazonKeyManagementService kmsClient,
        IOptions<AwsCredentialsOptions> options,
        ILogger<KmsService> logger)
    {
        _kmsClient = kmsClient;
        _options = options.Value;
        _logger = logger;
    }

    public async Task<KmsDataKeyResult> GenerateDataKeyAsync(CancellationToken cancellationToken = default)
    {
        var keyId = RequireKmsKeyArn();
        var response = await _kmsClient.GenerateDataKeyAsync(
            new GenerateDataKeyRequest
            {
                KeyId = keyId,
                KeySpec = DataKeySpec.AES_256
            },
            cancellationToken);

        _logger.LogDebug("Generated KMS data key using key {KeyIdentifier}.", keyId);
        return new KmsDataKeyResult(
            response.Plaintext.ToArray(),
            response.CiphertextBlob.ToArray(),
            keyId);
    }

    public async Task<byte[]> DecryptDataKeyAsync(byte[] encryptedDataKey, CancellationToken cancellationToken = default)
    {
        var response = await _kmsClient.DecryptAsync(
            new DecryptRequest
            {
                CiphertextBlob = new MemoryStream(encryptedDataKey)
            },
            cancellationToken);

        return response.Plaintext.ToArray();
    }

    public async Task<byte[]> ReEncryptDataKeyAsync(
        byte[] encryptedDataKey,
        string? sourceKeyIdentifier,
        CancellationToken cancellationToken = default)
    {
        var destinationKeyId = RequireKmsKeyArn();
        var request = new ReEncryptRequest
        {
            CiphertextBlob = new MemoryStream(encryptedDataKey),
            DestinationKeyId = destinationKeyId
        };

        if (!string.IsNullOrWhiteSpace(sourceKeyIdentifier))
        {
            request.SourceKeyId = sourceKeyIdentifier;
        }

        var response = await _kmsClient.ReEncryptAsync(request, cancellationToken);
        return response.CiphertextBlob.ToArray();
    }

    public async Task<(byte[] EncryptedDataKey, string KeyIdentifier)> ReEncryptDataKeyWithMetadataAsync(
        byte[] encryptedDataKey,
        string? sourceKeyIdentifier,
        CancellationToken cancellationToken = default)
    {
        var destinationKeyId = RequireKmsKeyArn();
        var request = new ReEncryptRequest
        {
            CiphertextBlob = new MemoryStream(encryptedDataKey),
            DestinationKeyId = destinationKeyId
        };

        if (!string.IsNullOrWhiteSpace(sourceKeyIdentifier))
        {
            request.SourceKeyId = sourceKeyIdentifier;
        }

        var response = await _kmsClient.ReEncryptAsync(request, cancellationToken);
        var keyIdentifier = response.KeyId ?? destinationKeyId;
        return (response.CiphertextBlob.ToArray(), keyIdentifier);
    }

    private string RequireKmsKeyArn()
    {
        if (string.IsNullOrWhiteSpace(_options.KmsKeyArn))
        {
            throw new InvalidOperationException("AwsCredentials:KmsKeyArn is not configured.");
        }

        return _options.KmsKeyArn;
    }
}
