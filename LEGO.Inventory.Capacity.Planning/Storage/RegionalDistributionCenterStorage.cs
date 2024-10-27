using LEGO.Inventory.Capacity.Planning.Domain.DistributionCenters;
using LEGO.Inventory.Capacity.Planning.Storage.Interfaces;

namespace LEGO.Inventory.Capacity.Planning.Storage;

public class RegionalDistributionCenterStorage : IRegionalDistributionCenterStorage
{
    private static readonly RegionalDistributionCenter _regionalDistributionCenter =
        new("LEGO European Distribution Center", "Lego - Harry Potter", 10);

    public Task<RegionalDistributionCenter> GetAsync()
    {
        return Task.FromResult(_regionalDistributionCenter);
    }

    public Task UpdateAsync(RegionalDistributionCenter rdc)
    {
        if (!_regionalDistributionCenter.Name.Equals(rdc.Name))
        {
            throw new ArgumentException("Cannot update: mismatched RDC.");
        }
        _regionalDistributionCenter.UpdateQuantity(rdc.FinishedGoodsStockQuantity);
        return Task.CompletedTask;
    }
}
