using Fgs.Foundation.Result;
using Fgs.Setup.Application.Features.Credentials.DTOs;
using Fgs.Setup.Domain.Enums;
using MediatR;

namespace Fgs.Setup.Application.Features.Credentials.Commands.RotateCredential;

public sealed record RotateCredentialCommand(
    CredentialScope Scope,
    string Id,
    CredentialRotationMode RotationMode = CredentialRotationMode.Full) : IRequest<ApiResponse<CredentialMutationResultDto>>;
