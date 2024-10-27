using LEGO.Inventory.Capacity.Planning.Domain.Orders;

namespace LEGO.Inventory.Capacity.Planning.Storage.Interfaces;

public interface ISalesOrderStorage
{
    Task<List<SalesOrder>> GetAllAsync();
    Task<SalesOrder> AddAsync(SalesOrder salesOrder);
}
