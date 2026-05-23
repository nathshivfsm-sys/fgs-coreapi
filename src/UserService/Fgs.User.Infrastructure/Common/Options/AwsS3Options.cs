namespace Fgs.User.Infrastructure.Common.Options;

public sealed class AwsS3Options
{
    public const string SectionName = "AwsS3";

    public string AccessKeyId { get; set; } = string.Empty;

    public string SecretAccessKey { get; set; } = string.Empty;

    public string KmsKeyArn { get; set; } = string.Empty;

    public string Region { get; set; } = "us-east-1";

    public string BucketNamePrefix { get; set; } = "fgs-prod-tenant";
}
