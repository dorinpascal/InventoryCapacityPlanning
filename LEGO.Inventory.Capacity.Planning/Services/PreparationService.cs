using LEGO.Inventory.Capacity.Planning.Domain;
using LEGO.Inventory.Capacity.Planning.Domain.DistributionCenters;
using LEGO.Inventory.Capacity.Planning.Domain.Orders;
using LEGO.Inventory.Capacity.Planning.Services.Interfaces;
using LEGO.Inventory.Capacity.Planning.Storage;

namespace LEGO.Inventory.Capacity.Planning.Services;

public class PreparationService(IStockTransportOrderService _stockTransportOrderService, IStorage _storage,
    ILogger<PreparationService> _logger) : ISalesPreparationService
{
    public async Task Prepare(SalesOrder salesOrder)
    {
        var localDistributionCenter = await _storage.GetLocalDistributionCentersByNameAsync(salesOrder.LocalDistributionCenterName);

        var requiredQuantity = 0;

        if (localDistributionCenter.FinishedGoodsStockQuantity >= salesOrder.Quantity)
        {
            localDistributionCenter.FinishedGoodsStockQuantity = localDistributionCenter.FinishedGoodsStockQuantity - salesOrder.Quantity;
            _logger.LogInformation(localDistributionCenter.Name + "'s new stock quantity: " + localDistributionCenter.FinishedGoodsStockQuantity);
            return;
        }
        else if (localDistributionCenter.FinishedGoodsStockQuantity < salesOrder.Quantity)
        {
            var shortfall = salesOrder.Quantity - localDistributionCenter.FinishedGoodsStockQuantity;
            localDistributionCenter.FinishedGoodsStockQuantity = 0;

            if (localDistributionCenter.SafetyStockQuantity >= shortfall)
            {
                localDistributionCenter.SafetyStockQuantity -= shortfall;
                requiredQuantity = shortfall;
            }
            else
            {
                requiredQuantity = localDistributionCenter.SafetyStockThreshold;
                localDistributionCenter.SafetyStockQuantity = 0;
            }

            if (localDistributionCenter.SafetyStockQuantity == 0)
            {
                CreateSto(salesOrder, localDistributionCenter, requiredQuantity);
            }
            else if (localDistributionCenter.SafetyStockQuantity < localDistributionCenter.SafetyStockThreshold)
            {
                CreateSto(salesOrder, localDistributionCenter, (localDistributionCenter.SafetyStockThreshold - localDistributionCenter.SafetyStockQuantity));
            }

            _logger.LogWarning(localDistributionCenter.Name + "'s new safety stock quantity: " + localDistributionCenter.SafetyStockQuantity);
            return;
        }

        else if (salesOrder.Quantity > localDistributionCenter.FinishedGoodsStockQuantity
        && salesOrder.Quantity <= localDistributionCenter.FinishedGoodsStockQuantity + localDistributionCenter.SafetyStockQuantity)

        {
            int requiredFromSafetyStock = salesOrder.Quantity - localDistributionCenter.FinishedGoodsStockQuantity;
            localDistributionCenter.FinishedGoodsStockQuantity = 0;
            localDistributionCenter.SafetyStockQuantity = localDistributionCenter.SafetyStockQuantity - requiredFromSafetyStock;

            if (localDistributionCenter.SafetyStockQuantity == 0
                || localDistributionCenter.SafetyStockQuantity < localDistributionCenter.SafetyStockThreshold)
            {
                CreateSto(salesOrder, localDistributionCenter, requiredFromSafetyStock);
            }
            _logger.LogInformation(localDistributionCenter.Name + "'s new stock quantity: " + localDistributionCenter.FinishedGoodsStockQuantity);
            _logger.LogWarning(localDistributionCenter.Name + "'s new safety stock quantity: " + localDistributionCenter.SafetyStockQuantity);
        }
    }

    private void CreateSto(SalesOrder salesOrder, LocalDistributionCenter _localDistributionCenter,
        int requiredQuantity)
    {
        _stockTransportOrderService.Create(new StockTransportOrder(
            salesOrder.FinishedGoodsName,
            requiredQuantity,
            _storage.RegionalDistributionCenter.Name,
            _localDistributionCenter.Name));

        _logger.LogInformation($"new STO created: " + salesOrder.FinishedGoodsName + "Quantity: " + requiredQuantity
                               + ", from : " + _storage.RegionalDistributionCenter.Name + " to " +
                               _localDistributionCenter.Name);
    }
}