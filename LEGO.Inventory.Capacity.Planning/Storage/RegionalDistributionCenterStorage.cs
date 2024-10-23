using LEGO.Inventory.Capacity.Planning.Domain.DistributionCenters;
using LEGO.Inventory.Capacity.Planning.Storage.Interfaces;

namespace LEGO.Inventory.Capacity.Planning.Storage;

public class RegionalDistributionCenterStorage : IRegionalDistributionCenterStorage
{
    private readonly RegionalDistributionCenter _regionalDistributionCenter =
        new("LEGO European Distribution Center", "Lego - Harry Potter", 10);

    public Task<RegionalDistributionCenter> GetAllAsync()
    {
        return Task.FromResult(_regionalDistributionCenter);
    }
}
