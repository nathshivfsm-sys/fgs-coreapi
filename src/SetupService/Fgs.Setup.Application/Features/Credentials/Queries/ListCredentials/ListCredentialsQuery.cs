using Fgs.Foundation.Result;
using Fgs.Setup.Application.Features.Credentials.DTOs;
using Fgs.Setup.Domain.Enums;
using MediatR;

namespace Fgs.Setup.Application.Features.Credentials.Queries.ListCredentials;

public sealed record ListCredentialsQuery(
    CredentialScope Scope,
    long? TenantId = null,
    long? CompanyId = null,
    bool ActiveOnly = true) : IRequest<ApiResponse<IReadOnlyList<CredentialSummaryDto>>>;
