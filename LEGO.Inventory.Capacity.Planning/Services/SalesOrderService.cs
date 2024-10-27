using LEGO.Inventory.Capacity.Planning.Domain.Orders;
using LEGO.Inventory.Capacity.Planning.Services.Interfaces;
using LEGO.Inventory.Capacity.Planning.Storage.Interfaces;

namespace LEGO.Inventory.Capacity.Planning.Services;

public class SalesOrderService(ISalesOrderStorage _salesOrderStorage, ILocalDistributionCenterStorage _distributionCenterStorage, ILogger<SalesOrderService> _logger) : ISalesOrderService
{
    public async Task<List<SalesOrder>> GetAll()
    {
        return await _salesOrderStorage.GetAllAsync();
    }

    public async Task<SalesOrder> Create(SalesOrder salesOrder)
    {
        _ = await _distributionCenterStorage.GetByNameAsync(salesOrder.LocalDistributionCenterName) ?? throw new ArgumentException($"Invalid local distribution center name: {salesOrder.LocalDistributionCenterName}");
        var newOrder = await _salesOrderStorage.AddAsync(salesOrder);
        _logger.LogInformation("New sales order created. FinishedGoods: {FinishedGoodsName}, Quantity: {Quantity}, LocalDistributionCenter: {LDCName}", salesOrder.FinishedGoodsName, salesOrder.Quantity, salesOrder.LocalDistributionCenterName);
        return newOrder;
    }
}
