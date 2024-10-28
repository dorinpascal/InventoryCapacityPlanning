using LEGO.Inventory.Capacity.Planning.Domain.Orders;
using LEGO.Inventory.Capacity.Planning.Services.Interfaces;
using LEGO.Inventory.Capacity.Planning.Storage.Interfaces;

namespace LEGO.Inventory.Capacity.Planning.Services;

public class RegionalDistributionCenterService(IRegionalDistributionCenterStorage _regionalDistributionCenterStorage, IStockTransportOrderStorage _stockTransportOrderStorage, ILogger<RegionalDistributionCenterService> _logger) : IRegionalDistributionCenterService
{
    public async Task<int> TryPickSTOAsync(Guid stockTransportOrderId)
    {
        // Retrieve the STO from storage
        var sto = await _stockTransportOrderStorage.GetByIdAsync(stockTransportOrderId) ?? throw new ArgumentException($"Stock transport order with ID {stockTransportOrderId} not found.");

        if (sto.Status is not StockTransportOrderStatus.Open)
        {
            throw new InvalidOperationException("Stock transport order must be open to be picked.");
        }
        // Retrieve the regional distribution center
        var rdc = await _regionalDistributionCenterStorage.GetAsync();

        // Check if there's enough stock to pick the STO
        if (sto.Quantity > rdc.FinishedGoodsStockQuantity)
        {
            throw new InvalidOperationException(
                $"Insufficient stock for STO {stockTransportOrderId}. Ordered: {sto.Quantity}, Available: {rdc.FinishedGoodsStockQuantity}");
        }

        // Update the STO status to Picked
        sto.UpdateStatus(StockTransportOrderStatus.Picked);
        await _stockTransportOrderStorage.UpdateAsync(sto);

        // Update the regional distribution center's stock quantity
        rdc.UpdateQuantity(rdc.FinishedGoodsStockQuantity - sto.Quantity);

        // Persist RDC stock update
        await _regionalDistributionCenterStorage.UpdateAsync(rdc);

        _logger.LogInformation(
            "STO {STOId} picked successfully. Quantity picked: {QuantityPicked}. Remaining stock at RDC {RDCName}: {RemainingStock}", sto.Id, sto.Quantity, rdc.Name, rdc.FinishedGoodsStockQuantity);

        return sto.Quantity;
    }

}
