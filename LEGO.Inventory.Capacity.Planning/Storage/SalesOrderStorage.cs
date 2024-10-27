using LEGO.Inventory.Capacity.Planning.Domain.Orders;
using LEGO.Inventory.Capacity.Planning.Storage.Interfaces;

namespace LEGO.Inventory.Capacity.Planning.Storage;

public class SalesOrderStorage : ISalesOrderStorage
{
    private static readonly List<SalesOrder> _salesOrders = [];

    public Task<SalesOrder> AddAsync(SalesOrder salesOrder)
    {
        _salesOrders.Add(salesOrder);
        return Task.FromResult(salesOrder);
    }

    public Task<List<SalesOrder>> GetAllAsync()
    {
        return Task.FromResult(_salesOrders.ToList());
    }
}
