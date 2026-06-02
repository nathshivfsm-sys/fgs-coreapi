using Fgs.Foundation.Result;
using Fgs.User.Application.Features.Credentials.DTOs;
using Fgs.User.Domain.Enums;
using MediatR;

namespace Fgs.User.Application.Features.Credentials.Queries.GetCredential;

public sealed record GetCredentialQuery(
    CredentialScope Scope,
    string Id) : IRequest<ApiResponse<CredentialDetailDto>>;
