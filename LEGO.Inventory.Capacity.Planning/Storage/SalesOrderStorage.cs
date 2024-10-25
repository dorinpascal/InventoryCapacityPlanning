using LEGO.Inventory.Capacity.Planning.Domain.Orders;
using LEGO.Inventory.Capacity.Planning.Storage.Interfaces;

namespace LEGO.Inventory.Capacity.Planning.Storage;

public class SalesOrderStorage : ISalesOrderStorage
{
    private static readonly List<SalesOrder> _salesOrders = [];

    public Task AddAsync(SalesOrder salesOrder)
    {
        _salesOrders.Add(salesOrder);
        return Task.CompletedTask;
    }

    public Task<List<SalesOrder>> GetAllAsync()
    {
        return Task.FromResult(_salesOrders.ToList());
    }
}
