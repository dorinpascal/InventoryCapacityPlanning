using LEGO.Inventory.Capacity.Planning.Domain.Orders;
using LEGO.Inventory.Capacity.Planning.Services.Interfaces;
using LEGO.Inventory.Capacity.Planning.Storage.Interfaces;

namespace LEGO.Inventory.Capacity.Planning.Services;

public class RegionalDistributionCenterService(IRegionalDistributionCenterStorage _regionalDistributionCenterStorage, IStockTransportOrderStorage _stockTransportOrderStorage, ILogger<RegionalDistributionCenterService> _logger) : IRegionalDistributionCenterService
{
    public async Task<int> TryPickSTOAsync(Guid stockTransportOrderId)
    {
        // Retrieve the STO from storage
        var stockTransportOrder = (await _stockTransportOrderStorage.GetAllAsync())
            .Find(sto => sto.Id == stockTransportOrderId)
            ?? throw new ArgumentException($"Stock transport order with ID {stockTransportOrderId} not found");

        // Retrieve the regional distribution center
        var regionalDistributionCenter = await _regionalDistributionCenterStorage.GetAsync();

        // Check if there's enough stock to pick the STO
        if (stockTransportOrder.Quantity > regionalDistributionCenter.FinishedGoodsStockQuantity)
        {
            throw new InvalidOperationException(
                $"Insufficient stock for STO {stockTransportOrderId}. Ordered: {stockTransportOrder.Quantity}, Available: {regionalDistributionCenter.FinishedGoodsStockQuantity}");
        }

        // Update the STO status to Picked
        stockTransportOrder.UpdateStatus(StockTransportOrderStatus.Picked);
        await _stockTransportOrderStorage.AddAsync(stockTransportOrder);

        // Update the regional distribution center's stock quantity
        regionalDistributionCenter.UpdateQuantity(regionalDistributionCenter.FinishedGoodsStockQuantity - stockTransportOrder.Quantity);

        _logger.LogInformation(
            "STO {STOId} picked successfully. Quantity picked: {QuantityPicked}. Remaining stock at RDC {RDCName}: {RemainingStock}", stockTransportOrder.Id, stockTransportOrder.Quantity, regionalDistributionCenter.Name, regionalDistributionCenter.FinishedGoodsStockQuantity);
        return stockTransportOrder.Quantity;
    }

}
