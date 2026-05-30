using Fgs.Foundation.Result;
using Fgs.User.Application.Features.Credentials.DTOs;
using MediatR;

namespace Fgs.User.Application.Features.Credentials.Commands.RotateCredential;

public sealed record RotateCredentialCommand : IRequest<ApiResponse<CredentialSecretMetadataDto>>
{
    public Guid SecretId { get; init; }

    public long TenantId { get; init; }

    public long CompanyId { get; init; }

    public string? RotationLambdaArn { get; init; }

    public string? RotatedBy { get; init; }
}
