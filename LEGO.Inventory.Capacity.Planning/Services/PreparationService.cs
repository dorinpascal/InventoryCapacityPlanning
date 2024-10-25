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
        var localDistributionCenter = await _localDistributionCenterStorage.GetByNameAsync(salesOrder.LocalDistributionCenterName);
        if (localDistributionCenter == null)
        {
            _logger.LogWarning($"Local distribution center '{salesOrder.LocalDistributionCenterName}' not found.");
            return;
        }

        var requiredQuantity = HandleStockReductionAsync(localDistributionCenter, salesOrder.Quantity);

        if (requiredQuantity > 0)
        {
            await CreateStoAsync(salesOrder, localDistributionCenter, requiredQuantity);
        }

        LogStockQuantities(localDistributionCenter);
    }

    private static int HandleStockReductionAsync(LocalDistributionCenter localDistributionCenter, int orderedQuantity)
    {
        var requiredQuantity = 0;

        if (localDistributionCenter.FinishedGoodsStockQuantity >= orderedQuantity)
        {
            localDistributionCenter.FinishedGoodsStockQuantity -= orderedQuantity;
            return requiredQuantity; // No additional stock required from STO
        }

        requiredQuantity = orderedQuantity - localDistributionCenter.FinishedGoodsStockQuantity;
        localDistributionCenter.FinishedGoodsStockQuantity = 0;

        if (localDistributionCenter.SafetyStockQuantity >= requiredQuantity)
        {
            localDistributionCenter.SafetyStockQuantity -= requiredQuantity;
        }
        else
        {
            requiredQuantity = localDistributionCenter.SafetyStockThreshold;
            localDistributionCenter.SafetyStockQuantity = 0;
        }

        return requiredQuantity;
    }

    private async Task CreateStoAsync(SalesOrder salesOrder, LocalDistributionCenter localDistributionCenter, int requiredQuantity)
    {
        var regionalDistributionCenter = await _regionalDistributionCenterStorage.GetAllAsync();

        var stockTransportOrder = new StockTransportOrder(
            salesOrder.FinishedGoodsName,
            requiredQuantity,
            regionalDistributionCenter.Name,
            localDistributionCenter.Name
        );

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
}