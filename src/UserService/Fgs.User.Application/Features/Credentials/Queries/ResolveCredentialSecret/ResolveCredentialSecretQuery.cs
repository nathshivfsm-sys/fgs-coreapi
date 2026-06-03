using Fgs.Foundation.Result;
using Fgs.User.Application.Features.Credentials.DTOs;
using Fgs.User.Domain.Enums;
using MediatR;

namespace Fgs.User.Application.Features.Credentials.Queries.ResolveCredentialSecret;

public sealed record ResolveCredentialSecretQuery(
    CredentialScope Scope,
    string Id) : IRequest<ApiResponse<CredentialSecretDto>>;
