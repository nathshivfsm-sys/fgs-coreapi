using Fgs.Contracts.Api;
using Fgs.Inventory.Application.Features.Vendors.Dtos;
using MediatR;

namespace Fgs.Inventory.Application.Features.Vendors.Queries.LookupVendors;

public sealed record LookupVendorsQuery(bool ActiveOnly = true)
    : IRequest<ApiResponse<IReadOnlyList<FgsVendorLookupDto>>>;
