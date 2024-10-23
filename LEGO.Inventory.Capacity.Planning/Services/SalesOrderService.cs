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

    public async Task Create(SalesOrder salesOrder)
    {
        var localDistributionCenter = await _distributionCenterStorage.GetByNameAsync(salesOrder.LocalDistributionCenterName);
        if (localDistributionCenter is null)
        {
            _logger.LogError("invalid local distribution center name");
            throw new ArgumentException("invalid local distribution center name");
        }
        else
        {
            await _salesOrderStorage.AddAsync(salesOrder);
            _logger.LogInformation($"new order created: " + salesOrder.FinishedGoodsName + " : " + salesOrder.Quantity + " -LDC: " + salesOrder.LocalDistributionCenterName);
        }
    }
}
