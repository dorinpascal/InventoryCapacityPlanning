using LEGO.Inventory.Capacity.Planning.Domain.DistributionCenters;
using LEGO.Inventory.Capacity.Planning.Domain.GoodsMovement;
using LEGO.Inventory.Capacity.Planning.Domain.Orders;
using LEGO.Inventory.Capacity.Planning.Services.Interfaces;
using LEGO.Inventory.Capacity.Planning.Storage.Interfaces;

namespace LEGO.Inventory.Capacity.Planning.Services;

public class GoodsReceiptService(IGoodsReceiptStorage _goodsReceiptStorage, IStockTransportOrderStorage _transportOrderStorage, ILocalDistributionCenterStorage _distributionCenterStorage, ILogger<GoodsReceiptService> logger) : IGoodsReceiptService
{
    public Task<List<GoodsReceipt>> GetAll()
    {
        return _goodsReceiptStorage.GetAllAsync();
    }

    public async Task<GoodsReceipt> Create(GoodsReceipt goodsReceipt)
    {
        // Fetch the stock transport order, throw exception if not found
        var stockTransportOrder = await _transportOrderStorage.GetByIdAsync(goodsReceipt.StockTransportOrderId)
            ?? throw new ArgumentException($"Stock transport order with ID {goodsReceipt.StockTransportOrderId} not found");

        // Check if the stock transport order has been picked
        if (stockTransportOrder.Status == StockTransportOrderStatus.Picked)
        {
            var localDistributionCenter = await _distributionCenterStorage.GetByNameAsync(stockTransportOrder.LocalDistributionCenterName)
                ?? throw new ArgumentException($"Local distribution center '{stockTransportOrder.LocalDistributionCenterName}' not found");

            // Update the stock quantities based on the received quantity and safety stock requirements
            await AdjustStockQuantities(localDistributionCenter);

            // Structured log for clear context
            logger.LogInformation(
                "Updated stock for {LDCName}: Finished Goods Stock = {StockQuantity}, Safety Stock = {SafetyStockQuantity}",
                localDistributionCenter.Name,
                localDistributionCenter.FinishedGoodsStockQuantity,
                localDistributionCenter.SafetyStockQuantity);
        }
        else
        {
            logger.LogWarning("Stock transport order with ID {STOId} has not been picked, so no stock update was applied", goodsReceipt.StockTransportOrderId);
        }

        // Add goods receipt to storage and log result
        var createdGoodsReceipt = await _goodsReceiptStorage.AddAsync(goodsReceipt);
        logger.LogInformation("Goods receipt with ID {GoodsReceiptId} created for stock transport order {STOId}", createdGoodsReceipt.StockTransportOrderId, goodsReceipt.StockTransportOrderId);

        return createdGoodsReceipt;
    }

    private async Task AdjustStockQuantities(LocalDistributionCenter ldc)
    {
        // Calculate the difference between SafetyStockThreshold and current SafetyStockQuantity
        int safetyStockDifference = Math.Max(0, ldc.SafetyStockThreshold - ldc.SafetyStockQuantity);

        // Increase SafetyStockQuantity to match SafetyStockThreshold
        ldc.SafetyStockQuantity = ldc.SafetyStockThreshold;

        // Allocate only the difference to Finished Goods Stock
        ldc.FinishedGoodsStockQuantity += safetyStockDifference;

        await _distributionCenterStorage.UpdateAsync(ldc);
    }
}