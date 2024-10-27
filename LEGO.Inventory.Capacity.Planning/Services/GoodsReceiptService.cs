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
        var stockTransportOrder = await _transportOrderStorage.GetByIdAsync(goodsReceipt.StockTransportOrderId) ?? throw new ArgumentException("Missing stock transport order");            
        if (stockTransportOrder.Status == StockTransportOrderStatus.Picked)
        {
            var localDistributionCenter = await _distributionCenterStorage.GetByNameAsync(stockTransportOrder.LocalDistributionCenterName) ?? throw new ArgumentException("Invalid local distribution center name");
            localDistributionCenter.FinishedGoodsStockQuantity += stockTransportOrder.Quantity; // Update Finished Goods Stock
            localDistributionCenter.SafetyStockQuantity = localDistributionCenter.SafetyStockThreshold; // Restore Safety Stock
            logger.LogInformation(localDistributionCenter.Name + "'s safety stock has been updated to " + localDistributionCenter.SafetyStockQuantity);
        }
        return await _goodsReceiptStorage.AddAsync(goodsReceipt);
    }
}