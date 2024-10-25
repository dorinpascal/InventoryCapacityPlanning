using LEGO.Inventory.Capacity.Planning.Domain.Orders;
using LEGO.Inventory.Capacity.Planning.Services.Interfaces;
using LEGO.Inventory.Capacity.Planning.Storage.Interfaces;

namespace LEGO.Inventory.Capacity.Planning.Services;

public class RegionalDistributionCenterService(IRegionalDistributionCenterStorage _regionalDistributionCenterStorage, IStockTransportOrderStorage _stockTransportOrderStorage, ILogger<RegionalDistributionCenterService> _logger) : IRegionalDistributionCenterService
{
    public async Task<int> TryPickSTOAsync(Guid stockTransportOrderId)
    {
        // Retrieve the STO from storage
        var stockTransportOrders = await _stockTransportOrderStorage.GetAllAsync();
        var stockTransportOrder = stockTransportOrders.Find(sto => sto.Id == stockTransportOrderId)
            ?? throw new ArgumentException("Missing stock transport order");

        // Retrieve the regional distribution center
        var regionalDistributionCenter = await _regionalDistributionCenterStorage.GetAsync();

        // Check if there's enough stock to pick the STO
        if (stockTransportOrder.Quantity > regionalDistributionCenter.FinishedGoodsStockQuantity)
        {
            throw new InvalidOperationException(
                $@"Couldn't pick stock transport order {stockTransportOrder.Id}. 
                    Insufficient stock for product {regionalDistributionCenter.FinishedGoodsName}.
                    Ordered stock: {stockTransportOrder.Quantity}, current stock: {regionalDistributionCenter.FinishedGoodsStockQuantity}");
        }

        // Update the STO status to Picked
        stockTransportOrder.UpdateStatus(StockTransportOrderStatus.Picked);
        await _stockTransportOrderStorage.AddAsync(stockTransportOrder);

        // Update the regional distribution center's stock quantity
        regionalDistributionCenter.UpdateQuantity(regionalDistributionCenter.FinishedGoodsStockQuantity - stockTransportOrder.Quantity);

        _logger.LogInformation($"STO with Id: {stockTransportOrder.Id} has been picked. Remaining stock: {regionalDistributionCenter.FinishedGoodsStockQuantity}");

        return stockTransportOrder.Quantity;
    }
}
