using Fgs.Contracts.Api;
using Fgs.Foundation.Paging;
using Fgs.User.Application.Common.IdentityCrud;
using Fgs.User.Application.Features.ServiceAccountsSetups.Dtos;
using MediatR;

namespace Fgs.User.Application.Features.ServiceAccountsSetups.Queries.ListFgsTenantServiceAccountsSetups;

public sealed record ListFgsTenantServiceAccountsSetupsQuery(IdentityListQuery Query)
    : IRequest<ApiResponse<PagedResult<FgsTenantServiceAccountsSetupSummaryDto>>>;
