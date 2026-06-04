using Fgs.Foundation.Result;
using Fgs.Setup.Application.Features.Credentials.DTOs;
using Fgs.Setup.Domain.Enums;
using MediatR;

namespace Fgs.Setup.Application.Features.Credentials.Commands.CreateCredential;

public sealed record CreateCredentialCommand(
    CredentialScope Scope,
    string ProviderCode,
    string CredentialName,
    string Payload,
    string? Description = null,
    long? TenantId = null,
    long? CompanyId = null) : IRequest<ApiResponse<CredentialMutationResultDto>>;
