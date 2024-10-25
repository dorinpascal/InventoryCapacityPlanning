using LEGO.Inventory.Capacity.Planning.Domain.GoodsMovement;
using LEGO.Inventory.Capacity.Planning.Domain.Orders;
using LEGO.Inventory.Capacity.Planning.Services.Interfaces;
using LEGO.Inventory.Capacity.Planning.Storage.Interfaces;

namespace LEGO.Inventory.Capacity.Planning.Services;

public class GoodsReceiptService(IGoodsReceiptStorage _goodsReceiptStorage, IStockTransportOrderStorage _transportOrderStorage, ILocalDistributionCenterStorage _distributionCenterStorage, ILogger<GoodsReceipt> logger) : IGoodsReceiptService
{
    public Task<List<GoodsReceipt>> GetAll()
    {
        return _goodsReceiptStorage.GetAllAsync();
    }

    public async Task Create(GoodsReceipt goodsReceipt)
    {
        await _goodsReceiptStorage.AddAsync(goodsReceipt);
        var stockTransportOrders = await _transportOrderStorage.GetAllAsync();
        var stockTransportOrder = stockTransportOrders.Find(sto => sto.Id == goodsReceipt.StockTransportOrderId) ??
            throw new ArgumentException("Missing stock transport order");

        if (stockTransportOrder.Status == StockTransportOrderStatus.Picked)
        {
            var localDistributionCenter = await _distributionCenterStorage.GetByNameAsync(stockTransportOrder.LocalDistributionCenterName);
            localDistributionCenter!.SafetyStockQuantity = localDistributionCenter.SafetyStockThreshold;
            logger.LogInformation(localDistributionCenter.Name + "'s safety stock has been updated to " +
                                   localDistributionCenter.SafetyStockQuantity);
        }
    }
}