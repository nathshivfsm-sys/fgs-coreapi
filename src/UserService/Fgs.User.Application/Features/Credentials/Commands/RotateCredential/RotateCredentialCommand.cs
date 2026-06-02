using Fgs.Foundation.Result;
using Fgs.User.Application.Features.Credentials.DTOs;
using Fgs.User.Domain.Enums;
using MediatR;

namespace Fgs.User.Application.Features.Credentials.Commands.RotateCredential;

public sealed record RotateCredentialCommand(
    CredentialScope Scope,
    string Id,
    CredentialRotationMode RotationMode = CredentialRotationMode.Full) : IRequest<ApiResponse<CredentialMutationResultDto>>;
