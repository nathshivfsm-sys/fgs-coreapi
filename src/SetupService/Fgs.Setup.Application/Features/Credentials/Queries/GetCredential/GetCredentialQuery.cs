using Fgs.Foundation.Result;
using Fgs.Setup.Application.Features.Credentials.DTOs;
using Fgs.Setup.Domain.Enums;
using MediatR;

namespace Fgs.Setup.Application.Features.Credentials.Queries.GetCredential;

public sealed record GetCredentialQuery(
    CredentialScope Scope,
    string Id) : IRequest<ApiResponse<CredentialDetailDto>>;
