using Fgs.Foundation.Result;
using Fgs.User.Application.Features.Credentials.DTOs;
using Fgs.User.Domain.Enums;
using MediatR;

namespace Fgs.User.Application.Features.Credentials.Commands.UpdateCredential;

public sealed record UpdateCredentialCommand(
    CredentialScope Scope,
    string Id,
    string CredentialName,
    string? Description = null,
    string? Payload = null,
    bool? IsActive = null) : IRequest<ApiResponse<CredentialMutationResultDto>>;
