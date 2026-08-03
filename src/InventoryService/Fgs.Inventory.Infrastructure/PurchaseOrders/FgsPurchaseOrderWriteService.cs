using Fgs.Inventory.Application.Abstractions.PurchaseOrders;
using Fgs.Inventory.Application.Features.PurchaseOrders.Dtos;
using Fgs.Inventory.Domain.Entities;
using Fgs.Inventory.Infrastructure.Common;
using Fgs.Inventory.Infrastructure.Database;
using Fgs.Kernel.Entities;
using Fgs.Persistence.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace Fgs.Inventory.Infrastructure.PurchaseOrders;

public sealed class FgsPurchaseOrderWriteService : IFgsPurchaseOrderWriteService
{
    private readonly FgsInventoryDbContext _context;
    private readonly IUnitOfWork _unitOfWork;
    private readonly InventoryEntityAuditHelper _auditHelper;

    public FgsPurchaseOrderWriteService(
        FgsInventoryDbContext context,
        IUnitOfWork unitOfWork,
        InventoryEntityAuditHelper auditHelper)
    {
        _context = context;
        _unitOfWork = unitOfWork;
        _auditHelper = auditHelper;
    }

    public async Task<FgsPurchaseOrderDetailDto> CreateAsync(
        FgsPurchaseOrderCreateDto dto,
        CancellationToken cancellationToken = default)
    {
        var entity = MapCreateDto(dto);
        _auditHelper.StampForCreate((FgsEntityBase)entity, entity);
        await _context.FgsPurchaseOrders.AddAsync(entity, cancellationToken);
        await SyncDetailsAsync(entity, dto.Details ?? [], cancellationToken);
        await SaveChangesAsync(cancellationToken);

        return MapToDetail(entity);
    }

    public async Task<FgsPurchaseOrderDetailDto> UpdateAsync(
        long id,
        FgsPurchaseOrderUpdateDto dto,
        CancellationToken cancellationToken = default)
    {
        var entity = await FindEntityAsync(id, includeDetails: true, cancellationToken)
            ?? throw new KeyNotFoundException($"Purchase order '{id}' was not found.");

        ApplyUpdateDto(entity, dto);
        _auditHelper.StampForUpdate(entity);
        await SyncDetailsAsync(entity, dto.Details ?? [], cancellationToken);
        await SaveChangesAsync(cancellationToken);

        return MapToDetail(entity);
    }

    public async Task<FgsPurchaseOrderDetailDto> PatchAsync(
        long id,
        FgsPurchaseOrderPatchDto dto,
        CancellationToken cancellationToken = default)
    {
        var entity = await FindEntityAsync(id, includeDetails: dto.Details is not null, cancellationToken)
            ?? throw new KeyNotFoundException($"Purchase order '{id}' was not found.");

        ApplyPatchDto(entity, dto);
        _auditHelper.StampForUpdate(entity);

        if (dto.Details is not null)
        {
            await SyncDetailsAsync(entity, dto.Details, cancellationToken);
        }

        await SaveChangesAsync(cancellationToken);

        if (dto.Details is null)
        {
            await _context.Entry(entity).Collection(e => e.Details).LoadAsync(cancellationToken);
        }

        return MapToDetail(entity);
    }

    private async Task SyncDetailsAsync(
        FgsPurchaseOrder purchaseOrder,
        IReadOnlyList<FgsPurchaseOrderLineDto> details,
        CancellationToken cancellationToken)
    {
        if (!_context.Entry(purchaseOrder).Collection(p => p.Details).IsLoaded
            && purchaseOrder.Id != 0)
        {
            await _context.Entry(purchaseOrder).Collection(p => p.Details).LoadAsync(cancellationToken);
        }

        InventoryChildCollectionSync.Sync(
            _context,
            purchaseOrder.Details,
            details,
            dto => dto.Id,
            _ => new FgsPurchaseOrderDetail { PurchaseOrderId = purchaseOrder.Id },
            (entity, dto, _) =>
            {
                entity.LineNumber = dto.LineNumber;
                entity.ItemId = dto.ItemId;
                entity.VendorPartNumber = TrimOrNull(dto.VendorPartNumber);
                entity.ItemDescription = dto.ItemDescription.Trim();
                entity.UnitOfMeasureCode = dto.UnitOfMeasureCode.Trim();
                entity.OrderedQuantity = dto.OrderedQuantity;
                entity.ReceivedQuantity = dto.ReceivedQuantity;
                entity.UnitCost = dto.UnitCost;
                entity.DiscountAmount = dto.DiscountAmount;
                entity.IsTaxable = dto.IsTaxable;
                entity.ExtendedAmount = dto.ExtendedAmount;
                entity.ExpectedDeliveryDate = dto.ExpectedDeliveryDate;
                entity.Notes = TrimOrNull(dto.Notes);
            },
            detail => _auditHelper.StampForCreate((FgsEntityBase)detail, detail),
            detail => _auditHelper.StampForUpdate((FgsEntityBase)detail),
            $"Purchase order line '{{0}}' was not found on purchase order '{purchaseOrder.Id}'.");
    }

    private async Task<FgsPurchaseOrder?> FindEntityAsync(
        long id,
        bool includeDetails,
        CancellationToken cancellationToken)
    {
        IQueryable<FgsPurchaseOrder> query = _context.FgsPurchaseOrders;
        if (includeDetails)
        {
            query = query.Include(e => e.Details);
        }

        return await query.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
    }

    private async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        try
        {
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException ex) when (IsUniqueViolation(ex))
        {
            throw new InvalidOperationException(
                "A purchase order with the same number already exists.",
                ex);
        }
    }

    private static bool IsUniqueViolation(DbUpdateException exception) =>
        exception.InnerException?.Message.Contains("duplicate", StringComparison.OrdinalIgnoreCase) == true
        || exception.InnerException?.Message.Contains("unique", StringComparison.OrdinalIgnoreCase) == true
        || exception.InnerException?.Message.Contains("23505", StringComparison.Ordinal) == true;

    private static FgsPurchaseOrder MapCreateDto(FgsPurchaseOrderCreateDto dto) =>
        new()
        {
            PurchaseOrderNumber = dto.PurchaseOrderNumber.Trim(),
            VendorId = dto.VendorId,
            PurchaseOrderStatus = dto.PurchaseOrderStatus.Trim().ToUpperInvariant(),
            PurchaseOrderDate = dto.PurchaseOrderDate,
            ExpectedDeliveryDate = dto.ExpectedDeliveryDate,
            RequestedByEmployeeId = dto.RequestedByEmployeeId,
            RequestedByName = TrimOrNull(dto.RequestedByName),
            BuyerEmployeeId = dto.BuyerEmployeeId,
            ShipToInventoryLocationId = dto.ShipToInventoryLocationId,
            ShipToServiceLocationId = dto.ShipToServiceLocationId,
            ShipToName = TrimOrNull(dto.ShipToName),
            ShipToAddress1 = TrimOrNull(dto.ShipToAddress1),
            ShipToAddress2 = TrimOrNull(dto.ShipToAddress2),
            ShipToCity = TrimOrNull(dto.ShipToCity),
            ShipToStateProvince = TrimOrNull(dto.ShipToStateProvince),
            ShipToPostalCode = TrimOrNull(dto.ShipToPostalCode),
            ShipToCountry = TrimOrNull(dto.ShipToCountry),
            VendorReferenceNumber = TrimOrNull(dto.VendorReferenceNumber),
            VendorContactName = TrimOrNull(dto.VendorContactName),
            VendorEmail = TrimOrNull(dto.VendorEmail),
            VendorPhoneNumber = TrimOrNull(dto.VendorPhoneNumber),
            Subtotal = dto.Subtotal,
            DiscountAmount = dto.DiscountAmount,
            TaxableAmount = dto.TaxableAmount,
            PurchaseTaxJson = dto.PurchaseTaxJson,
            FreightAmount = dto.FreightAmount,
            OtherCharges = dto.OtherCharges,
            TotalAmount = dto.TotalAmount,
            VendorNotes = TrimOrNull(dto.VendorNotes),
            InternalNotes = TrimOrNull(dto.InternalNotes)
        };

    private static void ApplyUpdateDto(FgsPurchaseOrder entity, FgsPurchaseOrderUpdateDto dto)
    {
        entity.PurchaseOrderNumber = dto.PurchaseOrderNumber.Trim();
        entity.VendorId = dto.VendorId;
        entity.PurchaseOrderStatus = dto.PurchaseOrderStatus.Trim().ToUpperInvariant();
        entity.PurchaseOrderDate = dto.PurchaseOrderDate;
        entity.ExpectedDeliveryDate = dto.ExpectedDeliveryDate;
        entity.RequestedByEmployeeId = dto.RequestedByEmployeeId;
        entity.RequestedByName = TrimOrNull(dto.RequestedByName);
        entity.BuyerEmployeeId = dto.BuyerEmployeeId;
        entity.ShipToInventoryLocationId = dto.ShipToInventoryLocationId;
        entity.ShipToServiceLocationId = dto.ShipToServiceLocationId;
        entity.ShipToName = TrimOrNull(dto.ShipToName);
        entity.ShipToAddress1 = TrimOrNull(dto.ShipToAddress1);
        entity.ShipToAddress2 = TrimOrNull(dto.ShipToAddress2);
        entity.ShipToCity = TrimOrNull(dto.ShipToCity);
        entity.ShipToStateProvince = TrimOrNull(dto.ShipToStateProvince);
        entity.ShipToPostalCode = TrimOrNull(dto.ShipToPostalCode);
        entity.ShipToCountry = TrimOrNull(dto.ShipToCountry);
        entity.VendorReferenceNumber = TrimOrNull(dto.VendorReferenceNumber);
        entity.VendorContactName = TrimOrNull(dto.VendorContactName);
        entity.VendorEmail = TrimOrNull(dto.VendorEmail);
        entity.VendorPhoneNumber = TrimOrNull(dto.VendorPhoneNumber);
        entity.Subtotal = dto.Subtotal;
        entity.DiscountAmount = dto.DiscountAmount;
        entity.TaxableAmount = dto.TaxableAmount;
        entity.PurchaseTaxJson = dto.PurchaseTaxJson;
        entity.FreightAmount = dto.FreightAmount;
        entity.OtherCharges = dto.OtherCharges;
        entity.TotalAmount = dto.TotalAmount;
        entity.VendorNotes = TrimOrNull(dto.VendorNotes);
        entity.InternalNotes = TrimOrNull(dto.InternalNotes);
    }

    private static void ApplyPatchDto(FgsPurchaseOrder entity, FgsPurchaseOrderPatchDto dto)
    {
        if (dto.PurchaseOrderNumber is not null)
        {
            entity.PurchaseOrderNumber = dto.PurchaseOrderNumber.Trim();
        }

        if (dto.VendorId.HasValue)
        {
            entity.VendorId = dto.VendorId.Value;
        }

        if (dto.PurchaseOrderStatus is not null)
        {
            entity.PurchaseOrderStatus = dto.PurchaseOrderStatus.Trim().ToUpperInvariant();
        }

        if (dto.PurchaseOrderDate.HasValue)
        {
            entity.PurchaseOrderDate = dto.PurchaseOrderDate.Value;
        }

        if (dto.ExpectedDeliveryDate.HasValue)
        {
            entity.ExpectedDeliveryDate = dto.ExpectedDeliveryDate;
        }

        if (dto.RequestedByEmployeeId.HasValue)
        {
            entity.RequestedByEmployeeId = dto.RequestedByEmployeeId;
        }

        if (dto.RequestedByName is not null)
        {
            entity.RequestedByName = TrimOrNull(dto.RequestedByName);
        }

        if (dto.BuyerEmployeeId.HasValue)
        {
            entity.BuyerEmployeeId = dto.BuyerEmployeeId;
        }

        if (dto.ShipToInventoryLocationId.HasValue)
        {
            entity.ShipToInventoryLocationId = dto.ShipToInventoryLocationId;
        }

        if (dto.ShipToServiceLocationId.HasValue)
        {
            entity.ShipToServiceLocationId = dto.ShipToServiceLocationId;
        }

        if (dto.ShipToName is not null)
        {
            entity.ShipToName = TrimOrNull(dto.ShipToName);
        }

        if (dto.ShipToAddress1 is not null)
        {
            entity.ShipToAddress1 = TrimOrNull(dto.ShipToAddress1);
        }

        if (dto.ShipToAddress2 is not null)
        {
            entity.ShipToAddress2 = TrimOrNull(dto.ShipToAddress2);
        }

        if (dto.ShipToCity is not null)
        {
            entity.ShipToCity = TrimOrNull(dto.ShipToCity);
        }

        if (dto.ShipToStateProvince is not null)
        {
            entity.ShipToStateProvince = TrimOrNull(dto.ShipToStateProvince);
        }

        if (dto.ShipToPostalCode is not null)
        {
            entity.ShipToPostalCode = TrimOrNull(dto.ShipToPostalCode);
        }

        if (dto.ShipToCountry is not null)
        {
            entity.ShipToCountry = TrimOrNull(dto.ShipToCountry);
        }

        if (dto.VendorReferenceNumber is not null)
        {
            entity.VendorReferenceNumber = TrimOrNull(dto.VendorReferenceNumber);
        }

        if (dto.VendorContactName is not null)
        {
            entity.VendorContactName = TrimOrNull(dto.VendorContactName);
        }

        if (dto.VendorEmail is not null)
        {
            entity.VendorEmail = TrimOrNull(dto.VendorEmail);
        }

        if (dto.VendorPhoneNumber is not null)
        {
            entity.VendorPhoneNumber = TrimOrNull(dto.VendorPhoneNumber);
        }

        if (dto.Subtotal.HasValue)
        {
            entity.Subtotal = dto.Subtotal.Value;
        }

        if (dto.DiscountAmount.HasValue)
        {
            entity.DiscountAmount = dto.DiscountAmount.Value;
        }

        if (dto.TaxableAmount.HasValue)
        {
            entity.TaxableAmount = dto.TaxableAmount.Value;
        }

        if (dto.PurchaseTaxJson is not null)
        {
            entity.PurchaseTaxJson = dto.PurchaseTaxJson;
        }

        if (dto.FreightAmount.HasValue)
        {
            entity.FreightAmount = dto.FreightAmount.Value;
        }

        if (dto.OtherCharges.HasValue)
        {
            entity.OtherCharges = dto.OtherCharges.Value;
        }

        if (dto.TotalAmount.HasValue)
        {
            entity.TotalAmount = dto.TotalAmount.Value;
        }

        if (dto.VendorNotes is not null)
        {
            entity.VendorNotes = TrimOrNull(dto.VendorNotes);
        }

        if (dto.InternalNotes is not null)
        {
            entity.InternalNotes = TrimOrNull(dto.InternalNotes);
        }
    }

    private static string? TrimOrNull(string? value) =>
        string.IsNullOrWhiteSpace(value) ? null : value.Trim();

    private static FgsPurchaseOrderDetailDto MapToDetail(FgsPurchaseOrder entity) =>
        new(
            entity.Id,
            entity.PurchaseOrderNumber,
            entity.VendorId,
            entity.PurchaseOrderStatus,
            entity.PurchaseOrderDate,
            entity.ExpectedDeliveryDate,
            entity.RequestedByEmployeeId,
            entity.RequestedByName,
            entity.BuyerEmployeeId,
            entity.ShipToInventoryLocationId,
            entity.ShipToServiceLocationId,
            entity.ShipToName,
            entity.ShipToAddress1,
            entity.ShipToAddress2,
            entity.ShipToCity,
            entity.ShipToStateProvince,
            entity.ShipToPostalCode,
            entity.ShipToCountry,
            entity.VendorReferenceNumber,
            entity.VendorContactName,
            entity.VendorEmail,
            entity.VendorPhoneNumber,
            entity.Subtotal,
            entity.DiscountAmount,
            entity.TaxableAmount,
            entity.PurchaseTaxJson,
            entity.FreightAmount,
            entity.OtherCharges,
            entity.TotalAmount,
            entity.VendorNotes,
            entity.InternalNotes,
            entity.Details
                .OrderBy(d => d.LineNumber)
                .ThenBy(d => d.Id)
                .Select(d => new FgsPurchaseOrderLineDetailDto(
                    d.Id,
                    d.LineNumber,
                    d.ItemId,
                    d.VendorPartNumber,
                    d.ItemDescription,
                    d.UnitOfMeasureCode,
                    d.OrderedQuantity,
                    d.ReceivedQuantity,
                    d.UnitCost,
                    d.DiscountAmount,
                    d.IsTaxable,
                    d.ExtendedAmount,
                    d.ExpectedDeliveryDate,
                    d.Notes))
                .ToList());
}
