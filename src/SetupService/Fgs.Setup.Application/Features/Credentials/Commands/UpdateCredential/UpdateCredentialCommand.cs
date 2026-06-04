using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.Credentials.DTOs;
using Fgs.Setup.Domain.Enums;
using MediatR;

namespace Fgs.Setup.Application.Features.Credentials.Commands.UpdateCredential;

public sealed record UpdateCredentialCommand(
    CredentialScope Scope,
    string Id,
    string CredentialName,
    string? Description = null,
    string? Payload = null,
    bool? IsActive = null) : IRequest<ApiResponse<CredentialMutationResultDto>>;

