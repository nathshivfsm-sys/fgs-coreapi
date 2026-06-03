using Fgs.Foundation.Result;
using Fgs.User.Application.Features.Credentials.DTOs;
using Fgs.User.Domain.Enums;
using MediatR;

namespace Fgs.User.Application.Features.Credentials.Commands.CreateCredential;

public sealed record CreateCredentialCommand(
    CredentialScope Scope,
    string ProviderCode,
    string CredentialName,
    string Payload,
    string? Description = null,
    long? TenantId = null,
    long? CompanyId = null) : IRequest<ApiResponse<CredentialMutationResultDto>>;
