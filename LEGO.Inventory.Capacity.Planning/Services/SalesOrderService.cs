using LEGO.Inventory.Capacity.Planning.Domain.Orders;
using LEGO.Inventory.Capacity.Planning.Services.Interfaces;
using LEGO.Inventory.Capacity.Planning.Storage;

namespace LEGO.Inventory.Capacity.Planning.Services;

public class SalesOrderService(IStorage _storage, ILogger<SalesOrderService> _logger) : ISalesOrderService
{
    public async Task<List<SalesOrder>> GetAll()
    {
        return await _storage.GetSalesOrdersAsync();
    }

    public async Task Create(SalesOrder salesOrder)
    {
        var localDistributionCenter = await _storage.GetLocalDistributionCentersByNameAsync(salesOrder.LocalDistributionCenterName);
        if (localDistributionCenter is null)
        {
            _logger.LogError("invalid local distribution center name");
            throw new ArgumentException("invalid local distribution center name");
        }
        else
        {
            await _storage.AddSalesOrderAsync(salesOrder);
            _logger.LogInformation($"new order created: " + salesOrder.FinishedGoodsName + " : " + salesOrder.Quantity + " -LDC: " + salesOrder.LocalDistributionCenterName);
        }
    }
}
