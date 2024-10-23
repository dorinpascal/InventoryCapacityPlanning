using LEGO.Inventory.Capacity.Planning.Domain.DistributionCenters;
using LEGO.Inventory.Capacity.Planning.Domain.Orders;

namespace LEGO.Inventory.Capacity.Planning.Storage;

public class Storage : IStorage
{
    private readonly List<SalesOrder> _salesOrders = [];
    private readonly List<LocalDistributionCenter> _localDistributionCenters =
    [
        new LocalDistributionCenter("Central Warehouse Europe", "LEGO European Distribution Center", "Lego - Harry Potter", 50, 20, 20),
        new LocalDistributionCenter("Eastern Warehouse Europe", "LEGO European Distribution Center", "Lego - Harry Potter", 60, 20, 20),
        new LocalDistributionCenter("Western Warehouse Europe", "LEGO European Distribution Center", "Lego - Harry Potter", 70, 20, 20)
    ];

    private readonly RegionalDistributionCenter _regionalDistributionCenter =
        new("LEGO European Distribution Center", "Lego - Harry Potter", 10);

    public Task<List<SalesOrder>> GetSalesOrdersAsync()
    {
        return Task.FromResult(_salesOrders.ToList());
    }

    public Task AddSalesOrderAsync(SalesOrder salesOrder)
    {
        _salesOrders.Add(salesOrder);
        return Task.CompletedTask;
    }

    public Task<List<LocalDistributionCenter>> GetLocalDistributionCentersAsync()
    {
        return Task.FromResult(_localDistributionCenters);
    }

    public Task<RegionalDistributionCenter> GetRegionalDistributionCenterAsync()
    {
        return Task.FromResult(_regionalDistributionCenter);
    }
}
