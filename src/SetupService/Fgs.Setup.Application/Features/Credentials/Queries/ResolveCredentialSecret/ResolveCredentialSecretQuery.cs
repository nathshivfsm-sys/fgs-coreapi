using Fgs.Foundation.Result;
using Fgs.Setup.Application.Features.Credentials.DTOs;
using Fgs.Setup.Domain.Enums;
using MediatR;

namespace Fgs.Setup.Application.Features.Credentials.Queries.ResolveCredentialSecret;

public sealed record ResolveCredentialSecretQuery(
    CredentialScope Scope,
    string Id) : IRequest<ApiResponse<CredentialSecretDto>>;
