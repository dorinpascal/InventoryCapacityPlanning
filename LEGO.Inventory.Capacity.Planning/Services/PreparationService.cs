using LEGO.Inventory.Capacity.Planning.Domain;
using LEGO.Inventory.Capacity.Planning.Domain.DistributionCenters;
using LEGO.Inventory.Capacity.Planning.Domain.Orders;
using LEGO.Inventory.Capacity.Planning.Services.Interfaces;
using LEGO.Inventory.Capacity.Planning.Storage.Interfaces;

namespace LEGO.Inventory.Capacity.Planning.Services;

public class PreparationService(IStockTransportOrderService _stockTransportOrderService, ILocalDistributionCenterStorage _localDistributionCenterStorage, IRegionalDistributionCenterStorage _regionalDistributionCenterStorage,
    ILogger<PreparationService> _logger) : ISalesPreparationService
{
    public async Task Prepare(SalesOrder salesOrder)
    {
        var localDistributionCenter = await _localDistributionCenterStorage.GetByNameAsync(salesOrder.LocalDistributionCenterName) ?? throw new ArgumentException("Invalid local distribution center name");

        var requiredQuantity = await HandleStockReductionAsync(localDistributionCenter, salesOrder.Quantity);

        if (requiredQuantity > 0)
        {
            await CreateStoAsync(salesOrder, localDistributionCenter, requiredQuantity);
        }

        LogStockQuantities(localDistributionCenter);
    }

    #region private
    private async Task<int> HandleStockReductionAsync(LocalDistributionCenter localDistributionCenter, int orderedQuantity)
    {
        var requiredQuantity = 0;
        // Case 1: LDC has enough finished goods to fulfill the order
        if (localDistributionCenter.FinishedGoodsStockQuantity >= orderedQuantity)
        {
            localDistributionCenter.FinishedGoodsStockQuantity -= orderedQuantity;
            await _localDistributionCenterStorage.UpdateAsync(localDistributionCenter);
            return requiredQuantity; // No additional stock required from STO
        }

        // Case 2: LDC does not have enough finished goods, calculate the required quantity
        requiredQuantity = orderedQuantity - localDistributionCenter.FinishedGoodsStockQuantity;
        localDistributionCenter.FinishedGoodsStockQuantity = 0;

        // Try to cover the remaining quantity using Safety Stock
        if (localDistributionCenter.SafetyStockQuantity >= requiredQuantity)
        {
            localDistributionCenter.SafetyStockQuantity -= requiredQuantity;
            requiredQuantity = localDistributionCenter.SafetyStockQuantity < localDistributionCenter.SafetyStockThreshold ? localDistributionCenter.SafetyStockThreshold - localDistributionCenter.SafetyStockQuantity : 0;
        }
        else
        {
            requiredQuantity -= localDistributionCenter.SafetyStockQuantity;
            localDistributionCenter.SafetyStockQuantity = 0;
        }

        await _localDistributionCenterStorage.UpdateAsync(localDistributionCenter);
        return requiredQuantity;
    }


    private async Task CreateStoAsync(SalesOrder salesOrder, LocalDistributionCenter localDistributionCenter, int requiredQuantity)
    {
        var regionalDistributionCenter = await _regionalDistributionCenterStorage.GetAsync();

        var stockTransportOrder = new StockTransportOrder(salesOrder.FinishedGoodsName, requiredQuantity, regionalDistributionCenter.Name, localDistributionCenter.Name);

        await _stockTransportOrderService.Create(stockTransportOrder);

        _logger.LogInformation("New STO created for product {Product}, Quantity: {Quantity}, from: {RDC} to {LDC}",
            salesOrder.FinishedGoodsName, requiredQuantity, regionalDistributionCenter.Name, localDistributionCenter.Name);
    }

    private void LogStockQuantities(LocalDistributionCenter localDistributionCenter)
    {
        _logger.LogInformation("{LDC}'s updated stock quantity: {StockQuantity}",
            localDistributionCenter.Name, localDistributionCenter.FinishedGoodsStockQuantity);

        _logger.LogWarning("{LDC}'s updated safety stock quantity: {SafetyStockQuantity}",
            localDistributionCenter.Name, localDistributionCenter.SafetyStockQuantity);
    }
    #endregion
}