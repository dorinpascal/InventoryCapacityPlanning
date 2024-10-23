using LEGO.Inventory.Capacity.Planning.Domain.DistributionCenters;
using LEGO.Inventory.Capacity.Planning.Domain.Orders;

namespace LEGO.Inventory.Capacity.Planning.Storage;

public interface IStorage
{
    Task<List<SalesOrder>> GetSalesOrdersAsync();
    Task AddSalesOrderAsync(SalesOrder salesOrder);
    Task<List<LocalDistributionCenter>> GetLocalDistributionCentersAsync();
    Task<LocalDistributionCenter?> GetLocalDistributionCentersByNameAsync(string name);
    Task<RegionalDistributionCenter> GetRegionalDistributionCenterAsync();
}
