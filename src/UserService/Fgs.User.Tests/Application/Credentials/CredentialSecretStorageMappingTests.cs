using Fgs.User.Application.Credentials;
using Fgs.User.Domain.Entities;

namespace Fgs.User.Tests.Application.Credentials;

public sealed class CredentialSecretStorageMappingTests
{
    [Fact]
    public void Maps_aws_metadata_to_existing_columns()
    {
        var entity = new FgsCredentialSecret();
        CredentialSecretStorageMapping.SetAwsSecretArn(entity, "arn:aws:secretsmanager:us-east-1:123:secret:x");
        CredentialSecretStorageMapping.SetRegionName(entity, "us-east-1");
        CredentialSecretStorageMapping.SetKmsKeyArn(entity, "arn:aws:kms:us-east-1:123:key/abc");

        entity.EncryptedSecretValue.Should().StartWith("arn:aws:secretsmanager");
        entity.EncryptedDek.Should().Be("us-east-1");
        entity.EncryptionKeyId.Should().StartWith("arn:aws:kms");
    }
}
