using Fgs.Foundation.Result;
using Fgs.User.Application.Features.Credentials.DTOs;
using Fgs.User.Domain.Enums;
using MediatR;

namespace Fgs.User.Application.Features.Credentials.Queries.ListCredentials;

public sealed record ListCredentialsQuery(
    CredentialScope Scope,
    long? TenantId = null,
    long? CompanyId = null,
    bool ActiveOnly = true) : IRequest<ApiResponse<IReadOnlyList<CredentialSummaryDto>>>;
