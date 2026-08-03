using Fgs.Contracts.Api;
using Fgs.Inventory.Application.Features.VendorInventoryItems.Dtos;
using MediatR;

namespace Fgs.Inventory.Application.Features.VendorInventoryItems.Queries.LookupVendorInventoryItems;

public sealed record LookupVendorInventoryItemsQuery(bool ActiveOnly = true)
    : IRequest<ApiResponse<IReadOnlyList<FgsVendorInventoryItemLookupDto>>>;
